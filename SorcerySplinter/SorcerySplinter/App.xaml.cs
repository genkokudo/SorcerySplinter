using Bbbb;
using Prism.Ioc;
using Prism.Modularity;
using SorcerySplinter.Modules.Common;
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
        /// <summary>
        /// 最初のウインドウを登録
        /// </summary>
        /// <returns></returns>
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        /// <summary>
        /// サービスの登録
        /// </summary>
        /// <param name="containerRegistry"></param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IMessageService, MessageService>(); // シングルトン
            containerRegistry.Register<IAaaaService, AaaaService>(); // 都度生成
        }

        /// <summary>
        /// モジュールの登録
        /// </summary>
        /// <param name="moduleCatalog"></param>
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleNameModule>();
            moduleCatalog.AddModule<BbbbModule>();
            moduleCatalog.AddModule<CommonModule>();
        }
    }
}
