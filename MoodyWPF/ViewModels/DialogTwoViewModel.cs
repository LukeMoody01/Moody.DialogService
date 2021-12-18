using Moody.Core.Services;
using Moody.WPF.Commands;
using System.Windows.Input;

namespace Moody.WPF
{
    public class DialogTwoViewModel
    {
        private readonly IDialogService _dialogService;

        public ICommand CloseThisDialogCommand { get; }


        public DialogTwoViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;

            CloseThisDialogCommand = new DelegateCommand(CloseThisDialog);
        }

        private void CloseThisDialog()
        {
            _dialogService.CloseDialog<DialogTwoViewModel>();
        }
    }
}
