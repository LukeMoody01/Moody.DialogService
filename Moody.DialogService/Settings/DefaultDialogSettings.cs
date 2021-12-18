using System.Windows;

namespace Moody.Core.Settings
{
    /// <summary>
    /// Represents Default Settings for the Dialogs
    /// </summary>
    public class DefaultDialogSettings
    {
        /// <summary>
        /// Default Dialog Window Style
        /// </summary>
        public WindowStyle DialogWindowDefaultStyle { get; set; } = WindowStyle.SingleBorderWindow;

        /// <summary>
        /// Default Dialog Window Title
        /// </summary>
        public string DialogWindowDefaultTitle { get; set; } = string.Empty;
    }
}
