using Microsoft.Extensions.DependencyInjection;
using Moody.Core.DialogControls;
using Moody.Core.Settings;
using Moody.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Moody.Core.Services
{
    /// <summary>
    /// Represents implementation Dialog Service.
    /// </summary>
    /// <remarks>
    /// A ViewModel that injects this interface can open, close, 
    /// and return parameters during the showing and closing of Dialogs.
    /// </remarks>
    public class DialogService : IDialogService
    {
        private readonly IServiceProvider _serviceProvider;

        static Dictionary<Type, Type> _mappings = new Dictionary<Type, Type>();
        static Dictionary<Type, DialogWindowShell> _windowMappings = new Dictionary<Type, DialogWindowShell>();

        /// <summary>
        /// The namespace of the ViewModels
        /// </summary>
        public static string ViewModelNamespace { get; set; } = string.Empty;

        /// <summary>
        /// The default settings for the dialog windows
        /// </summary>
        public DefaultDialogSettings Settings { get; private set; } = new DefaultDialogSettings();

        /// <summary>
        /// The return parameters from the dialog
        /// </summary>
        public object? ReturnParameters { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider">The provider needed for Dependency Injection</param>
        public DialogService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Registers the view against the requeted ViewModel
        /// </summary>
        /// <typeparam name="TView">Dialog View</typeparam>
        /// <typeparam name="TViewModel">Dialog ViewModel</typeparam>
        public static void RegisterDialog<TView, TViewModel>()
        {
            if (_mappings.Contains(new KeyValuePair<Type, Type>(typeof(TView), typeof(TViewModel)))) return;

            _mappings.Add(typeof(TViewModel), typeof(TView));
        }

        /// <summary>
        /// Automatically registers the Views against the ViewModels
        /// Using the Dialog Attribute in the code behind
        /// </summary>
        /// <typeparam name="T">The assembly of the App</typeparam>
        /// <exception cref="Exception">The exception thrown when a ViewModel cannot be located</exception>
        public static void AutoRegisterDialogs<T>()
        {
            var type = typeof(T);

            foreach (var exportedType in type.GetTypeInfo().Assembly.DefinedTypes.Where(t => t.GetCustomAttribute<DialogModuleAttribute>() != null))
            {
                Type? vm;

                if (string.IsNullOrWhiteSpace(ViewModelNamespace))
                {
                    vm = Type.GetType($"{exportedType.FullName}ViewModel, {exportedType.Assembly.FullName}");
                }
                else
                {
                    vm = Type.GetType($"{ViewModelNamespace}.{exportedType.Name}ViewModel, {exportedType.Assembly.FullName}");
                }

                if (vm == null) throw new Exception($"ViewModel not found for {exportedType.Name} at {exportedType.Namespace}ViewModel. Make sure it follows the MVVM pattern (Example, namespace.thing.Page1 -> namespace.thing.Page1ViewModel).");

                if (_mappings.ContainsKey(vm)) continue;

                _mappings.Add(vm, exportedType);
            }
        }

        /// <summary>
        /// Shows the dialog associated with the passed ViewModel.
        /// </summary>
        /// <typeparam name="TViewModel">The associated ViewModel to the requested View</typeparam>
        public void ShowDialog<TViewModel>()
        {
            var type = _mappings[typeof(TViewModel)];
            ShowDialogInternal(type, null);
        }

        /// <summary>
        ///  Shows the dialog associated with the passed ViewModel.
        ///  Once the dialog has been closed, the callback will be called
        /// </summary>
        /// <typeparam name="TViewModel">The associated ViewModel to the requested View</typeparam>
        /// <param name="closeCallback">The callback called once the dialog has been requested to close</param>
        public void ShowDialog<TViewModel>(Action<string> closeCallback)
        {
            var type = _mappings[typeof(TViewModel)];
            ShowDialogInternal(type, closeCallback);
        }

        /// <summary>
        ///  Shows the dialog associated with the passed ViewModel.
        /// </summary>
        /// <typeparam name="TViewModel">The associated ViewModel to the requested View</typeparam>
        /// <typeparam name="TReturn">The expected return type</typeparam>
        /// <returns>Returns the 'ReturnParameters' set in the dialog ViewModel</returns>
        public TReturn ShowDialog<TViewModel, TReturn>()
        {
            var type = _mappings[typeof(TViewModel)];
            return ShowDialogReturnInternal<TReturn>(type);
        }

        /// <summary>
        ///  Shows the dialog associated with the passed ViewModel.
        /// </summary>
        /// <typeparam name="TViewModel">The associated ViewModel to the requested View</typeparam>
        /// <typeparam name="TReturn">The expected return type</typeparam>
        /// <param name="closeCallback">The callback called once the dialog has been requested to close</param>
        /// <returns>Returns the 'ReturnParameters' set in the dialog ViewModel</returns>
        public TReturn ShowDialog<TViewModel, TReturn>(Action<string> closeCallback)
        {
            var type = _mappings[typeof(TViewModel)];
            return ShowDialogReturnInternal<TReturn>(type, closeCallback);
        }

        /// <summary>
        /// Closes the dialog associated with the passed ViewModel
        /// </summary>
        /// <typeparam name="TViewModel">The associated ViewModel to the requested View</typeparam>
        public void CloseDialog<TViewModel>()
        {
            var type = _mappings[typeof(TViewModel)];

            if (!_windowMappings.ContainsKey(type)) return;

            var dialogToClose = _windowMappings[type];
            if (dialogToClose == null) return;

            dialogToClose.Close();
            _windowMappings.Remove(type);
        }

        /// <summary>
        /// Get's the current ReturnParameters
        /// </summary>
        /// <typeparam name="TReturn">The expected return type</typeparam>
        /// <returns>Returns the ReturnParameters as the requested type</returns>
        public TReturn? GetReturnParameters<TReturn>()
        {
            try { return (TReturn)ReturnParameters; }
            catch { return default(TReturn); }
        }

        /// <summary>
        /// Set's the ReturnParameters
        /// </summary>
        /// <param name="returnParameters">The value of the expected return parameters</param>
        public void SetReturnParameters(object? returnParameters)
        {
            ReturnParameters = returnParameters;
        }

        /// <summary>
        /// Set's the DefaultDialogSettings
        /// </summary>
        /// <param name="dialogSettings">The value of the expected default settings</param>
        public void SetDefaultDialogSettings(DefaultDialogSettings dialogSettings)
        {
            Settings = dialogSettings;
        }

        /// <summary>
        /// Get's the current Default Dialog Settings
        /// </summary>
        /// <returns>Returns the Default Dialog Settings</returns>
        public DefaultDialogSettings GetDefaultDialogSettings()
        {
            return Settings;
        }

        private void ShowDialogInternal(Type type, Action<string>? closeCallback)
        {
            ReturnParameters = default;

            var dialog = new DialogWindowShell();

            EventHandler? closeEventHandler = null;

            if (closeCallback != null)
            {

                closeEventHandler = (s, e) =>
                {
                    closeCallback(dialog.DialogResult.ToString());
                    dialog.Closed -= closeEventHandler;
                };

                dialog.Closed += closeEventHandler;
            }

            var content = ActivatorUtilities.CreateInstance(_serviceProvider, type);

            if (content is FrameworkElement viewForName 
                && DialogSettings.GetDialogName(viewForName) != null)
            {
                dialog.Title = DialogSettings.GetDialogName(viewForName);
            }

            if (content is FrameworkElement viewForStyle
                && DialogSettings.GetWindowStyle(viewForStyle) != WindowStyle.SingleBorderWindow)
            {
                dialog.WindowStyle = DialogSettings.GetWindowStyle(viewForStyle);
            }

            dialog.Content = content;
            dialog.WindowStyle = dialog.WindowStyle == WindowStyle.SingleBorderWindow ? Settings.DialogWindowDefaultStyle : dialog.WindowStyle;
            dialog.Title = dialog.Title ?? Settings.DialogWindowDefaultTitle;

            _windowMappings.Add(type, dialog); 

            dialog.ShowDialog();

            _windowMappings.Remove(type);
        }

        private TReturn ShowDialogReturnInternal<TReturn>(Type type, Action<string>? closeCallback = null)
        {
            ShowDialogInternal(type, closeCallback);

            try { return (TReturn)ReturnParameters; }
            catch { return default(TReturn); }
        }
    }
}