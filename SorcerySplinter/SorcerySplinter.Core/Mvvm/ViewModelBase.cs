//using Prism.Mvvm;
//using Prism.Navigation;

//namespace SorcerySplinter.Core.Mvvm
//{
//    /// <summary>
//    /// RegionViewModelBaseよりも更にBaseな部分
//    /// </summary>
//    public abstract class ViewModelBase : BindableBase, IDestructible
//    {
//        protected ViewModelBase()
//        {

//        }

//        public virtual void Destroy()
//        {
//            // NotifyCollectionChangedAction.Removeのときに呼ぶ
//            // 要するにコレクションから削除されたとき

//            // private CompositeDisposable disposables = new CompositeDisposable();
//            // これをViewModelに持たせて、IDisposableを実装したオブジェクトを一括管理。
//            // disposables.Dispose()で一発Disposeできる。

//            // disposables.Dispose();
//        }
//    }

//}
