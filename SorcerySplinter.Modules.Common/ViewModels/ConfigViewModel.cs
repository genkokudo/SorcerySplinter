using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SorcerySplinter.Modules.Common.ViewModels
{
    public class ConfigViewModel : BindableBase
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        public ConfigViewModel()
        {
            Message = "Config from your Prism Module";
        }
    }
}
