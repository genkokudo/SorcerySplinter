using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SorcerySplinter.Core;
using SorcerySplinter.Modules.Common.Views;

namespace SorcerySplinter.ViewModels
{
    /// <summary>
    /// ここのフィールドをMainWindowにバインドしたりする
    /// </summary>
    public class MainWindowViewModel : BindableBase
    {
        /// <summary>画面遷移するコマンド</summary>
        public DelegateCommand<string> NavigateCommand { get; private set; }
        private readonly IRegionManager _regionManager;

        private string _title = "Sorcery Splinter";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(Home));// 初期画面指定

            // コマンド設定
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        /// <summary>
        /// 画面遷移する
        /// </summary>
        /// <param name="navigatePath">View名</param>
        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate(RegionNames.ContentRegion, navigatePath);
        }
    }
}
