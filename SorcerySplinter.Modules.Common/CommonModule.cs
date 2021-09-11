using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SorcerySplinter.Modules.Common.Views;

namespace SorcerySplinter.Modules.Common
{
    public class CommonModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public CommonModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        /// <summary>
        /// このモジュールが読み込まれたときの初期処理（＝プログラム起動時にモジュールが読まれるので、プログラム起動時の処理。）
        /// </summary>
        /// <param name="containerProvider">これが何かわからない。要調査。</param>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            //_regionManager.RequestNavigate(RegionNames.ContentRegion, ViewName);  // これを書いてしまうと起動時に初期画面として表示されてしまう。初期画面の指定方法として正しくない。
        }

        /// <summary>
        /// このモジュール内の作成したViewを登録する。
        /// </summary>
        /// <param name="containerRegistry"></param>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Home>();
            containerRegistry.RegisterForNavigation<Load>();
            containerRegistry.RegisterForNavigation<Edit>();
            containerRegistry.RegisterForNavigation<Config>();
        }
    }
}