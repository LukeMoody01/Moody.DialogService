using System.Windows;

namespace Moody.Core.Settings
{
    public class DefaultDialogSettings
    {
        public WindowStyle DialogWindowDefaultStyle { get; set; } = WindowStyle.SingleBorderWindow;

        public string DialogWindowDefaultTitle { get; set; } = string.Empty;
    }
}
