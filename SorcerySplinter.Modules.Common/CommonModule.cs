using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SorcerySplinter.Modules.Common.Views;

namespace SorcerySplinter.Modules.Common
{
    public class CommonModule : IModule
    {
        /// <summary>
        /// このモジュールが読み込まれたときの初期処理（＝プログラム起動時にモジュールが読まれるので、プログラム起動時の処理。）
        /// </summary>
        /// <param name="containerProvider">これが何かわからない。要調査。</param>
        public void OnInitialized(IContainerProvider containerProvider)
        {
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
            containerRegistry.RegisterForNavigation<Buffer>();
        }
    }

    /// <summary>
    /// このモジュールに含まれるView名を定数にしておく
    /// </summary>
    public static class ViewNames
    {
        public const string ViewHome = "Home";
        public const string ViewLoad = "Load";
        public const string ViewEdit = "Edit";
        public const string ViewConfig = "Config";
        public const string ViewBuffer = "Buffer";
    }
}