using Bbbb.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SorcerySplinter.Core;

namespace Bbbb
{
    // ・Module作成者：どんなRegionが必要でどんな機能を提供するか文書に書いて周知すること。Module内にどんな名前のViewがあるかを周知すること。
    // ・Module使用者：Moduleを使用するためにどんなRegion名を準備する必要があるか知っておく、Window内にRegionを作成しておくこと。

    /// <summary>
    /// 適当なモジュール
    /// </summary>
    public class BbbbModule : IModule
    {
        private const string ViewName = "ViewC";            // TODO:これどこに書くのが適切？
        private readonly IRegionManager _regionManager;

        public BbbbModule(IRegionManager regionManager)
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
            containerRegistry.RegisterForNavigation<ViewC>();
        }
    }
}