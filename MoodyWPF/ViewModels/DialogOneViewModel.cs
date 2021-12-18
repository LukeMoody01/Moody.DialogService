using Moody.Core.Services;
using Moody.WPF.Commands;
using System.Windows.Input;

namespace Moody.WPF
{
    public class DialogOneViewModel 
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
    }
}
