using Bbbb;
using Bbbb.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SorcerySplinter.Core;
using SorcerySplinter.Modules.ModuleName;
using SorcerySplinter.Modules.ModuleName.Views;

namespace SorcerySplinter.ViewModels
{
    /// <summary>
    /// ここのフィールドをMainWindowにバインドしたりする
    /// </summary>
    public class MainWindowViewModel : BindableBase
    {
        //private readonly IRegionManager _regionManager;

        private string _title = "Sorcery Splinter";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            //_regionManager = regionManager;
            regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(ViewC));// 初期画面指定
            //_regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(ViewA));
        }
    }
}
