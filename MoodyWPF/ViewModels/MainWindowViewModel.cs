using Moody.DialogService;
using Moody.WPF.Commands;
using System;
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
            _dialogService.SetDialogSettings(new DialogServiceSettings()
            {
                DialogWindowStyle = System.Windows.WindowStyle.None,
                DialogWindowTitle = "Dialog One Title",
            });
        }

        public void ShowDialog()
        {
            _dialogService.ShowDialog<DialogOneViewModel>();
        }
    }
}
