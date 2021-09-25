using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SorcerySplinter.Modules.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SorcerySplinter.Modules.Common.ViewModels
{
    public class EditViewModel : BindableBase
    {
        public List<SampleData> Variables { get; set; }

        /// <summary>テンプレート更新コマンド</summary>
        public DelegateCommand<string> SetTextCommand { get; private set; }

        /// <summary>他のモジュールに通知する</summary>
        public IEventAggregator EventAggregator { get; set; }

        // テンプレート入力内容
        private string _textInput;
        public string TextInput
        {
            get { return _textInput; }
            set { SetProperty(ref _textInput, value); }
        }

        public EditViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;

            Variables = new List<SampleData>
            {
                new SampleData{SubName = "1郎", SubDescription = "説明1", SubDefValue = "aaaa", SubIsClassName = true },
                new SampleData{SubName = "2郎", SubDescription = "説明2", SubDefValue = "bbbb", SubIsClassName = false },
                new SampleData{SubName = "3郎", SubDescription = "説明3", SubDefValue = "cccc", SubIsClassName = true }
            };

            // コマンドを設定
            SetTextCommand = new DelegateCommand<string>(SetText);

            // 通知イベントを設定
            eventAggregator.GetEvent<InputTemplateEvent>().Subscribe(SetTextInput);
        }

        /// <summary>
        /// イベントを使って更新した内容を他のVMに伝える
        /// </summary>
        /// <param name="text"></param>
        private void SetText(string text)
        {
            // 内容を他のモジュールに通知
            EventAggregator.GetEvent<InputTemplateEvent>()
                .Publish(new InputTemplate { InputText = text, SendFromViewModelName = GetType().Name });
        }

        /// <summary>
        /// イベントから受信したテンプレート内容をテキストボックスに反映させる
        /// </summary>
        /// <param name="text"></param>
        private void SetTextInput(InputTemplate obj)
        {
            if (obj.SendFromViewModelName != GetType().Name)
            {
                TextInput = obj.InputText;
            }
        }
    }

    public class SampleData
    {
        public string SubName { get; set; }
        public string SubDescription { get; set; }
        public string SubDefValue { get; set; }
        public bool SubIsClassName { get; set; }
    }
}
