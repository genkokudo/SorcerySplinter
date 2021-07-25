﻿using Prism.Regions;
using System;

namespace SorcerySplinter.Core.Mvvm
{
    public class RegionViewModelBase : ViewModelBase, INavigationAware, IConfirmNavigationRequest
    {
        protected IRegionManager RegionManager { get; private set; }

        public RegionViewModelBase(IRegionManager regionManager)
        {
            RegionManager = regionManager;
        }

        /// <summary>
        /// 状況に応じて遷移をキャンセルする
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <param name="continuationCallback">falseを送ると遷移をキャンセルする</param>
        public virtual void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            continuationCallback(true);
        }

        /// <summary>
        /// 同じインスタンスを使いまわすかどうか
        /// 例えば、TabControlをRegionにしているときに、同じViewだけど違うコンテンツを表示したいといったケースはfalseを返す
        /// 画面Aから画面Bに遷移して、画面Aに戻ってきたときに、前の画面Aがそのまま使われる場合はtrueを返す
        /// </summary>
        /// <param name="navigationContext"></param>
        /// <returns>Viewを再利用するか</returns>
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        /// <summary>
        /// 画面から離れるときに呼び出される
        /// </summary>
        /// <param name="navigationContext"></param>
        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        /// <summary>
        /// 画面に遷移してきたときに呼び出される（表示後）
        /// ※遷移先表示前に実行したい処理がある場合はPrism.Navigation.IInitializeを使用する
        /// </summary>
        /// <param name="navigationContext"></param>
        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {

        }
    }
}
