using System.Windows;

namespace Moody.DialogControls
{
    public partial class DialogWindowShell : Window
    {
        public DialogWindowShell()
        {
            InitializeComponent();

            Owner = Application.Current.MainWindow;
        }
    }
}