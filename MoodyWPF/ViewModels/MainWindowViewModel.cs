using Moody.Core.Models;
using Moody.Core.Services;
using Moody.Core.Settings;
using Moody.WPF.Commands;
using Moody.WPF.Constants;
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

            var defaultSettings = _dialogService.GetDefaultDialogSettings();
        }

        public void ShowDialog()
        {
            var parameters = new DialogParameters()
            {
                 { AppConstants.MY_BOOL_OBJECT, true }
            };

            _dialogService.ShowDialog<DialogOneViewModel>(parameters);
        }
    }
}
