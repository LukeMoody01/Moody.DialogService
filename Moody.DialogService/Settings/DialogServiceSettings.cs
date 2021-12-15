using System.Windows;

namespace Moody.DialogService
{
    public class DialogServiceSettings
    {
        public WindowStyle DialogWindowStyle { get; set; } = WindowStyle.SingleBorderWindow;

        public string DialogWindowTitle { get; set; } = string.Empty;
    }
}
