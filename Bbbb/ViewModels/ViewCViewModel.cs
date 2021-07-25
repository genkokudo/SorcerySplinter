﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SorcerySplinter.Core.Mvvm;
using SorcerySplinter.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bbbb.ViewModels
{
    public class ViewCViewModel : RegionViewModelBase
    {
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
        }

        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            //do something
        }
    }
}
