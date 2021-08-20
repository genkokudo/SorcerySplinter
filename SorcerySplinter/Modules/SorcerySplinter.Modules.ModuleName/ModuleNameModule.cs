using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using SorcerySplinter.Core;
using SorcerySplinter.Modules.ModuleName.Views;

namespace SorcerySplinter.Modules.ModuleName
{
    public class ModuleNameModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ModuleNameModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            //_regionManager.RequestNavigate(RegionNames.ContentRegion, "ViewA");  // これを書いてしまうと起動時に初期画面として表示されてしまう。初期画面の指定方法として正しくない。
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewA>();
        }
    }
}