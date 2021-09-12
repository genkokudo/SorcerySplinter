using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SorcerySplinter.Core;
using SorcerySplinter.Core.Mvvm;
using SorcerySplinter.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bbbb.ViewModels
{
    public class ViewCViewModel : RegionViewModelBase
    {
        // ボタンに紐づけたコマンド
        public DelegateCommand<string> NavigateCommand { get; private set; }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public ViewCViewModel(IRegionManager regionManager, IAaaaService messageService) :
            base(regionManager)
        {
            Message = messageService.GetMessage();
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //do something
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                RegionManager.RequestNavigate(RegionNames.ContentRegion, navigatePath);
        }
    }
}
