﻿using Moody.DialogService;
using Moody.WPF.Commands;
using System.Windows.Input;

namespace Moody.WPF
{
    public class MainWindowViewModel
    {
        private readonly IDialogService _dialogService;

        public ICommand ShowDialogOneCommand { get; }

        public MainWindowViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            ShowDialogOneCommand = new DelegateCommand(ShowDialog);

            SetupDialogSettings();
        }

        private void SetupDialogSettings()
        {
            _dialogService.SetDefaultDialogSettings(new DefaultDialogSettings()
            {
                DialogWindowDefaultStyle = System.Windows.WindowStyle.None,
                DialogWindowDefaultTitle = "Window",
            });
        }

        public void ShowDialog()
        {
            _dialogService.ShowDialog<DialogOneViewModel>();
        }
    }
}
