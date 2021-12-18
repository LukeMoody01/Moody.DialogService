using Microsoft.Extensions.DependencyInjection;
using Moody.Core.Services;
using Moody.WPF.Constants;
using Moody.WPF.IoC;
using System.Windows;

namespace Moody.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //TODO: Look more into this... not sure how I feel about it
        public static ServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            ConfigureServiceCollection();
            ConfigureDialogs();
        }

        private void ConfigureServiceCollection()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IDialogService, DialogService>();

            ServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureDialogs()
        {
            DialogService.ViewModelNamespace = AppConstants.ViewModelNameSpace;
            DialogService.AutoRegisterDialogs<App>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            IocKernel.Initialize(new IocConfiguration());

            base.OnStartup(e);
        }
    }
}
