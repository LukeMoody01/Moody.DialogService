using Microsoft.Extensions.DependencyInjection;
using Moody.DialogControls;
using Moody.DialogService.Attributes;
using Moody.DialogService.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Moody.DialogService
{
    public class DialogService : IDialogService
    {
        private readonly IServiceProvider _serviceProvider;

        static Dictionary<Type, Type> _mappings = new Dictionary<Type, Type>();
        static Dictionary<Type, DialogWindowShell> _windowMappings = new Dictionary<Type, DialogWindowShell>();

        public DefaultDialogSettings Settings { get; set; } = new DefaultDialogSettings();

        public object? ReturnParameters { get; set; }

        public DialogService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public static void RegisterDialog<TView, TViewModel>()
        {
            if (_mappings.Contains(new KeyValuePair<Type, Type>(typeof(TView), typeof(TViewModel)))) return;

            _mappings.Add(typeof(TViewModel), typeof(TView));
        }

        public static void AutoRegisterDialogs<T>()
        {
            var type = typeof(T);

            foreach (var exportedType in type.GetTypeInfo().Assembly.DefinedTypes.Where(t => t.GetCustomAttribute<DialogModuleAttribute>() != null))
            {
                var vm = Type.GetType($"{exportedType.FullName}ViewModel, {exportedType.Assembly.FullName}");

                if (vm == null) throw new Exception($"ViewModel not found for {exportedType.Name} at {exportedType.Namespace}ViewModel. Make sure it follows the MVVM pattern (Example, namespace.thing.Page1 -> namespace.thing.Page1ViewModel).");

                if (_mappings.ContainsKey(vm)) continue;

                _mappings.Add(vm, exportedType);
            }
        }

        public void ShowDialog<TViewModel>()
        {
            var type = _mappings[typeof(TViewModel)];
            ShowDialogInternal(type, null);
        }

        public void ShowDialog<TViewModel>(Action<string> closeCallback)
        {
            var type = _mappings[typeof(TViewModel)];
            ShowDialogInternal(type, closeCallback);
        }

        public TReturn ShowDialog<TViewModel, TReturn>()
        {
            var type = _mappings[typeof(TViewModel)];
            return ShowDialogReturnInternal<TReturn>(type);
        }

        public TReturn ShowDialog<TViewModel, TReturn>(Action<string> closeCallback)
        {
            var type = _mappings[typeof(TViewModel)];
            return ShowDialogReturnInternal<TReturn>(type, closeCallback);
        }

        public void CloseDialog<TViewModel>()
        {
            var type = _mappings[typeof(TViewModel)];

            if (!_windowMappings.ContainsKey(type)) return;

            var dialogToClose = _windowMappings[type];
            if (dialogToClose == null) return;

            dialogToClose.Close();
            _windowMappings.Remove(type);
        }

        public object? GetReturnParameters<TReturn>()
        {
            try { return (TReturn)ReturnParameters; }
            catch { return default(TReturn); }
        }

        public void SetReturnParameters<TReturn>(object? returnParameters) => ReturnParameters = returnParameters;

        public void SetDefaultDialogSettings(DefaultDialogSettings dialogSettings) => Settings = dialogSettings;

        public DefaultDialogSettings GetDefaultDialogSettings(DefaultDialogSettings dialogSettings) => Settings;

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

            if (content is FrameworkElement viewForName && 
                DialogSettings.GetDialogName(viewForName) != null)
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