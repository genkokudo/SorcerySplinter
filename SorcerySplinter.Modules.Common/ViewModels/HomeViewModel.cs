using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SorcerySplinter.Modules.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SorcerySplinter.Modules.Common.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        /// <summary>自分用モードであることを他のモジュールに通知する</summary>
        public IEventAggregator EventAggregator { get; set; }

        /// <summary>起動時処理のコマンド</summary>
        public DelegateCommand LoadedCommand { get; private set; }

        public HomeViewModel(IEventAggregator eventAggregator)
        {
            Message = "これは、コードスニペットを作成するソフトです。 \n";

            LoadedCommand = new DelegateCommand(OnLoaded);
            EventAggregator = eventAggregator;
        }

        /// <summary>起動時処理</summary>
        public void OnLoaded()
        {
            // 設定内容を他のモジュールに通知
            EventAggregator.GetEvent<GinpayModeEvent>()
                .Publish(new GinpayMode { IsGinpayMode = ModuleSettings.Default.IsGinpayMode });
        }
    }
}
