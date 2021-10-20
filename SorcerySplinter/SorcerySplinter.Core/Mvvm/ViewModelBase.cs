using Prism.Mvvm;
using Prism.Navigation;

namespace SorcerySplinter.Core.Mvvm
{
    /// <summary>
    /// RegionViewModelBaseよりも更にBaseな部分
    /// </summary>
    public abstract class ViewModelBase : BindableBase, IDestructible
    {
        protected ViewModelBase()
        {

        }

        public virtual void Destroy()
        {

        }
    }

    // IActiveAware：タブページの選択を検知、タブへの遷移時にある処理を行いたい
}
