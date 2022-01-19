using Microsoft.Extensions.DependencyInjection;
using Moody.Core.DialogControls;
using Moody.Core.Settings;
using Moody.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Moody.Core.Interfaces;
using Moody.Core.Models;

namespace Moody.Core.Services
{
    /// <inheritdoc />
    public class DialogService : IDialogService
    {
        private readonly IServiceProvider _serviceProvider;

        static Dictionary<Type, Type> _mappings = new Dictionary<Type, Type>();
        static Dictionary<Type, DialogWindowShell> _windowMappings = new Dictionary<Type, DialogWindowShell>();

        /// <inheritdoc />
        public static string ViewModelNamespace { get; set; } = string.Empty;

        /// <inheritdoc />
        public DefaultDialogSettings Settings { get; private set; } = new DefaultDialogSettings();

        /// <inheritdoc />
        public object ReturnParameters { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="serviceProvider">The provider needed for Dependency Injection</param>
        public DialogService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public static void RegisterDialog<TView, TViewModel>()
        {
            if (_mappings.Contains(new KeyValuePair<Type, Type>(typeof(TView), typeof(TViewModel)))) return;

            _mappings.Add(typeof(TViewModel), typeof(TView));
        }

        /// <inheritdoc />
        public static void AutoRegisterDialogs<T>()
        {
            var type = typeof(T);

            foreach (var exportedType in type.GetTypeInfo().Assembly.DefinedTypes.Where(t => t.GetCustomAttribute<DialogModuleAttribute>() != null))
            {
                Type vm;

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

        /// <inheritdoc />
        public void ShowDialog<TViewModel>()
        {
            var type = _mappings[typeof(TViewModel)];
            ShowDialogInternal(type, null, null);
        }

        /// <inheritdoc />
        public void ShowDialog<TViewModel>(DialogParameters dialogParameters)
        {
            var type = _mappings[typeof(TViewModel)];
            ShowDialogInternal(type, null, dialogParameters);
        }

        /// <inheritdoc />
        public void ShowDialog<TViewModel>(Action closeCallback)
        {
            var type = _mappings[typeof(TViewModel)];
            ShowDialogInternal(type, closeCallback, null);
        }

        /// <inheritdoc />
        public void ShowDialog<TViewModel>(DialogParameters dialogParameters, Action closeCallback)
        {
            var type = _mappings[typeof(TViewModel)];
            ShowDialogInternal(type, closeCallback, dialogParameters);
        }

        /// <inheritdoc />
        public TReturn ShowDialog<TViewModel, TReturn>()
        {
            var type = _mappings[typeof(TViewModel)];
            return ShowDialogReturnInternal<TReturn>(type, null, null);
        }

        /// <inheritdoc />
        public TReturn ShowDialog<TViewModel, TReturn>(DialogParameters dialogParameters)
        {
            var type = _mappings[typeof(TViewModel)];
            return ShowDialogReturnInternal<TReturn>(type, null, dialogParameters);
        }

        /// <inheritdoc />
        public TReturn ShowDialog<TViewModel, TReturn>(Action closeCallback)
        {
            var type = _mappings[typeof(TViewModel)];
            return ShowDialogReturnInternal<TReturn>(type, closeCallback, null);
        }

        /// <inheritdoc />
        public TReturn ShowDialog<TViewModel, TReturn>(DialogParameters dialogParameters, Action closeCallback)
        {
            var type = _mappings[typeof(TViewModel)];
            return ShowDialogReturnInternal<TReturn>(type, closeCallback, dialogParameters);
        }

        /// <inheritdoc />
        public void CloseDialog<TViewModel>()
        {
            var type = _mappings[typeof(TViewModel)];

            if (!_windowMappings.ContainsKey(type)) return;

            var dialogToClose = _windowMappings[type];
            if (dialogToClose == null) return;

            dialogToClose.Close();
            _windowMappings.Remove(type);
        }

        /// <inheritdoc />
        public TReturn GetReturnParameters<TReturn>()
        {
            try { return (TReturn)ReturnParameters; }
            catch { return default(TReturn); }
        }

        /// <inheritdoc />
        public void SetReturnParameters(object returnParameters)
        {
            ReturnParameters = returnParameters;
        }

        /// <inheritdoc />
        public void SetDefaultDialogSettings(DefaultDialogSettings dialogSettings)
        {
            Settings = dialogSettings;
        }

        /// <inheritdoc />
        public DefaultDialogSettings GetDefaultDialogSettings()
        {
            return Settings;
        }

        private void ShowDialogInternal(Type type, Action closeCallback, DialogParameters dialogParameters)
        {
            ReturnParameters = default;

            var dialog = CreateDialogInternal(type);

            FrameworkElement frameworkElement = dialog.Content as FrameworkElement;

            SetupEventHandlersInternal(closeCallback, dialog, frameworkElement, dialogParameters);
            SetupViewModelBindingsInternal(type, frameworkElement, dialogParameters);

            if (frameworkElement != null)
            {
                dialog.Height = frameworkElement.Height;
                dialog.Width = frameworkElement.Width;
            }

            _windowMappings.Add(type, dialog);

            dialog.ShowDialog();

            _windowMappings.Remove(type);
        }

        private TReturn ShowDialogReturnInternal<TReturn>(Type type, Action closeCallback, DialogParameters dialogParameters)
        {
            ShowDialogInternal(type, closeCallback, dialogParameters);

            try { return (TReturn)ReturnParameters; }
            catch { return default(TReturn); }
        }

        private DialogWindowShell CreateDialogInternal(Type type)
        {
            var dialog = new DialogWindowShell();
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

            return dialog;
        }

        private void SetupEventHandlersInternal(Action closeCallback, DialogWindowShell dialog, FrameworkElement frameworkElement, DialogParameters dialogParameters)
        {
            EventHandler closeEventHandler = null;
            RoutedEventHandler shownEventHandler = null;

            closeEventHandler = (s, e) =>
            {
                if (frameworkElement?.DataContext is IDialogAware dialogAware)
                {
                    dialogAware.OnDialogClosed(dialogParameters);
                }

                if (closeCallback != null) closeCallback();

                dialog.Closed -= closeEventHandler;
            };

            dialog.Closed += closeEventHandler;

            shownEventHandler = (s, e) =>
            {
                if (frameworkElement?.DataContext is IDialogAware dialogAware)
                {
                    dialogAware.OnDialogShown(dialogParameters);
                }

                dialog.Loaded -= shownEventHandler;
            };

            dialog.Loaded += shownEventHandler;
        }

        private void SetupViewModelBindingsInternal(Type type, FrameworkElement frameworkElement, DialogParameters dialogParameters = null)
        {
            if (frameworkElement == null) return;

            if (frameworkElement.DataContext == null)
            {
                var vmType = _mappings.FirstOrDefault(x => x.Value == type).Key;
                frameworkElement.DataContext = ActivatorUtilities.CreateInstance(_serviceProvider, vmType);
            }

            if (frameworkElement.DataContext is IDialogAware dialogAware)
            {
                dialogAware.OnDialogInitialized(dialogParameters);
            }
        }
    }
}