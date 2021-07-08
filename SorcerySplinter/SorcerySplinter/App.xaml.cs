using Prism.Ioc;
using Prism.Modularity;
using SorcerySplinter.Modules.ModuleName;
using SorcerySplinter.Services;
using SorcerySplinter.Services.Interfaces;
using SorcerySplinter.Views;
using System.Windows;

namespace SorcerySplinter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IMessageService, MessageService>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleNameModule>();
        }
    }
}
