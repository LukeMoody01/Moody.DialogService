namespace Moody.WPF.IoC
{
    //TODO: Find a better way for this, maybe Code Gen? Maybe create another nuget for this?
    public class ViewModelLocator
    {
        public MainWindowViewModel MainWindowViewModel
        {
            get { return IocKernel.Get<MainWindowViewModel>(); }
        }

        public DialogOneViewModel DialogOneViewModel
        {
            get { return IocKernel.Get<DialogOneViewModel>(); }
        }

        public DialogTwoViewModel DialogTwoViewModel
        {
            get { return IocKernel.Get<DialogTwoViewModel>(); }
        }
    }
}
