using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using SorcerySplinter.Core;
using SorcerySplinter.Modules.Common.Events;
using SorcerySplinter.Modules.Common.Views;
using System;

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

        // 自分用モードのオンオフ
        private bool _isGinpayMode;
        public bool IsGinpayMode
        {
            get { return _isGinpayMode; }
            set { SetProperty(ref _isGinpayMode, value); }
        }

        /// <summary>他のモジュールからの通知を購読する</summary>
        public IEventAggregator _eventAggregator { get; set; }

        private string _title = "Sorcery Splinter";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(Home));// 初期画面指定

            // コマンド設定
            NavigateCommand = new DelegateCommand<string>(Navigate);

            // モジュールからの通知内容を設定
            eventAggregator.GetEvent<GinpayModeEvent>()
                .Subscribe(CalculateAnswer);

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

        /// <summary>
        /// 自分用モードかどうかを受信
        /// </summary>
        /// <param name="isGinpayMode"></param>
        private void CalculateAnswer(GinpayMode obj)
        {
            IsGinpayMode = obj.IsGinpayMode;
        }
    }
}
