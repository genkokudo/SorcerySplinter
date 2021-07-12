using Prism.Regions;
using System;

namespace SorcerySplinter.Core.Mvvm
{
    // INavigationAwareはPrismのRegionを使えるようにするもの
    /// <summary>
    /// このアプリケーションの各ViewModelのBaseクラス
    /// </summary>
    public class RegionViewModelBase : ViewModelBase, INavigationAware, IConfirmNavigationRequest
    {
        protected IRegionManager RegionManager { get; private set; }

        public RegionViewModelBase(IRegionManager regionManager)
        {
            RegionManager = regionManager;
        }

        /// <summary>
        /// バリデーションに引っ掛かった時などに
        /// 画面遷移をキャンセルできるらしい
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <param name="continuationCallback"></param>
        public virtual void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            continuationCallback(true);
        }

        /// <summary>
        /// 画面のインスタンスを使いまわすかどうか制御するためのものらしい
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <returns></returns>
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <summary>
        /// この画面から他の画面に遷移するときの処理
        /// </summary>
        /// <param name="navigationContext"></param>
        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        /// <summary>
        /// 他の画面からこの画面に遷移したときの処理。ここで遷移元からのパラメータを受け取れる
        /// </summary>
        /// <param name="navigationContext"></param>
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {

        }
    }
}
