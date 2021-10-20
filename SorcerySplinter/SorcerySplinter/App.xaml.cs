using Microsoft.Extensions.Options;
using MithrilCube.Services;
using Prism.Ioc;
using Prism.Modularity;
using SnippetGenerator;
using SorcerySplinter.Core;
using SorcerySplinter.Modules.Common;
using SorcerySplinter.Services;
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
            //containerRegistry.RegisterSingleton<IMessageService, MessageService>(); // シングルトン
            containerRegistry.Register<ISnippetService, SnippetService>(); // 都度生成
            containerRegistry.Register<IDirectoryService, DirectoryService>();
            
            // オプション付きサービス
            containerRegistry.Register<IParameterService, ParameterService>();
            containerRegistry.RegisterInstance(typeof(IOptions<ParameterOptions>), Options.Create(new ParameterOptions { RegionName = RegionNames.ContentRegion }));
        }

        /// <summary>
        /// モジュールの登録
        /// </summary>
        /// <param name="moduleCatalog"></param>
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<CommonModule>();
        }
    }
}
