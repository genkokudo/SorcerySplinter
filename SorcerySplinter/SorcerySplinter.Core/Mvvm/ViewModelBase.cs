using Prism.Mvvm;
using Prism.Navigation;

namespace SorcerySplinter.Core.Mvvm
{
    /// <summary>
    /// このアプリケーションの各ViewModelの抽象クラス
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
}
