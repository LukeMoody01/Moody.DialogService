using System.Windows;

namespace Moody.Core.DialogControls
{
    public partial class DialogWindowShell : Window
    {
        /// <summary>
        /// The Shell of all dialogs
        /// </summary>
        public DialogWindowShell()
        {
            InitializeComponent();

            Owner = Application.Current.MainWindow;
        }
    }
}