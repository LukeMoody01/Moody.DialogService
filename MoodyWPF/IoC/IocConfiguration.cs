using Microsoft.Extensions.DependencyInjection;
using Moody.Core.Services;
using Ninject.Modules;
using System;

namespace Moody.WPF.IoC
{
    public class IocConfiguration : NinjectModule
    {
        public override void Load()
        {
            RegisterServices();
            RegisterViews();
        }

        private void RegisterViews()
        {
            Bind<MainWindowViewModel>().ToSelf().InTransientScope();
            Bind<DialogOneViewModel>().ToSelf().InTransientScope();
            Bind<DialogTwoViewModel>().ToSelf().InTransientScope();
        }

        private void RegisterServices()
        {
            Bind<IServiceProvider>().ToConstant(App.ServiceProvider).InSingletonScope();
            Bind<IDialogService>().To<DialogService>().InSingletonScope();
        }
    }
}
