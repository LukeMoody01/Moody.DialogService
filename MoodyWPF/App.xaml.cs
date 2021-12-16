using Microsoft.Extensions.DependencyInjection;
using Moody.DialogService;
using System.Windows;

namespace Moody.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Window window = new MainWindow();
            window.DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>();
            window.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDialogService, DialogService.DialogService>();
            services.AddScoped<MainWindowViewModel>();

            // You can register your dialogs like this, or through the module attribute (Seen below)
            DialogService.DialogService.RegisterDialog<DialogOne, DialogOneViewModel>();

            DialogService.DialogService.AutoRegisterDialogs<App>();
        }
    }
}
