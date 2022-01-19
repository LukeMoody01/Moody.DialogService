using Moody.Core.Models;
using System.Threading.Tasks;

namespace Moody.Core.Interfaces
{
    /// <summary>
    /// Provides a way for dialog view models to be notified of dialog activities.
    /// </summary>
    public interface IDialogAware
    {
        /// <summary>
        /// Called when the Dialog has been shown.
        /// </summary>
        void OnDialogShown(DialogParameters dialogParameters);

        /// <summary>
        /// Called when the Dialog has been initialized.
        /// </summary>
        void OnDialogInitialized(DialogParameters dialogParameters);

        /// <summary>
        /// Called when the Dialog is closed
        /// </summary>
        void OnDialogClosed(DialogParameters dialogParameters);
    }
}
