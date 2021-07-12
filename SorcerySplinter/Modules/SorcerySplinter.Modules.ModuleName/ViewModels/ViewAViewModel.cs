using Prism.Regions;
using SorcerySplinter.Core.Mvvm;
using SorcerySplinter.Services.Interfaces;

namespace SorcerySplinter.Modules.ModuleName.ViewModels
{
    public class ViewAViewModel : RegionViewModelBase
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public ViewAViewModel(IRegionManager regionManager, IMessageService messageService) :
            base(regionManager)
        {
            Message = messageService.GetTopMessage();
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //do something
        }
    }

    // 画面遷移の方法は、こうらしい
    // this.RegionManager.RequestNavigate("MainRegion", nameof(UserControl2), new NavigationParameters($"id=1"));
}
