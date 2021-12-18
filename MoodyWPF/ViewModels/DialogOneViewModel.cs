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
            _dialogService.ShowDialog<DialogTwoViewModel>();
        }

        private void CloseThisDialog()
        {
            _dialogService.CloseDialog<DialogOneViewModel>();
        }
    }
}
