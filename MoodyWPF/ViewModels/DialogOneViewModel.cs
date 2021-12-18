using Moody.DialogService;

namespace Moody.WPF
{
    public class DialogOneViewModel 
    {
        private readonly IDialogService _dialogService;

        public DialogOneViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }
    }
}
