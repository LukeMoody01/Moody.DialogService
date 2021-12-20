using Moody.Core.Interfaces;
using Moody.Core.Services;
using Moody.Core.Models;
using Moody.WPF.Commands;
using System.Windows.Input;
using Moody.WPF.Constants;

namespace Moody.WPF
{
    public class DialogOneViewModel : IDialogAware
    {
        private readonly IDialogService _dialogService;

        public ICommand ShowDialogTwoCommand { get; }
        public ICommand CloseThisDialogCommand { get; }

        public DialogOneViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;

            ShowDialogTwoCommand = new DelegateCommand(ShowDialogTwo);
            CloseThisDialogCommand = new DelegateCommand(CloseThisDialog);
        }

        private void ShowDialogTwo()
        {
            var isSuccess = _dialogService.ShowDialog<DialogTwoViewModel, bool>();

            if (isSuccess)
            {
                //Do some code
            }
        }

        private void CloseThisDialog()
        {
            _dialogService.CloseDialog<DialogOneViewModel>();
        }

        public void OnDialogShown(DialogParameters dialogParameters)
        {
            if (dialogParameters.TryGetValue(AppConstants.MY_BOOL_OBJECT, out var myBool))
            {
                // Somecode based on your object
            }
        }

        public void OnDialogInitialized(DialogParameters dialogParameters)
        {
            if (dialogParameters.TryGetValue(AppConstants.MY_BOOL_OBJECT, out var myBool))
            {
                // Somecode based on your object
            }
        }
    }
}
