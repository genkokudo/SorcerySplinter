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
    public class BufferViewModel : BindableBase
    {
        /// <summary>保存するコマンド</summary>
        public DelegateCommand SaveCommand { get; private set; }

        /// <summary>画面表示時コマンド</summary>
        public DelegateCommand ActivateCommand { get; private set; }

        /// <summary>テンプレート更新コマンド</summary>
        public DelegateCommand<string> SetTextCommand { get; private set; }

        /// <summary>他のモジュールに通知する</summary>
        public IEventAggregator EventAggregator { get; set; }

        // 書き溜めファイル内容
        private string _bufferInput;
        public string BufferInput
        {
            get { return _bufferInput; }
            set { SetProperty(ref _bufferInput, value); }
        }

        // テンプレート入力内容
        private string _textInput;
        public string TextInput
        {
            get { return _textInput; }
            set { SetProperty(ref _textInput, value); }
        }

        public BufferViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(Save);
            ActivateCommand = new DelegateCommand(Init);
            SetTextCommand = new DelegateCommand<string>(SetText);

            // 通知イベントを設定
            eventAggregator.GetEvent<InputTemplateEvent>().Subscribe(SetTextInput);
        }

        /// <summary>
        /// 画面表示時にテキストを読み込む
        /// </summary>
        private void Init()
        {
            //MessageBox.Show($"読み込み処理");
        }

        /// <summary>
        /// 保存する
        /// </summary>
        private void Save()
        {
            MessageBox.Show($"保存しました。");
        }

        /// <summary>
        /// イベントを使って更新した内容を他のVMに伝える
        /// </summary>
        /// <param name="text"></param>
        private void SetText(string text)
        {
            // 内容を他のモジュールに通知
            EventAggregator.GetEvent<InputTemplateEvent>()
                .Publish(new InputTemplate { InputText = text, SendFromViewModelName = "BufferViewModel" });
        }

        /// <summary>
        /// イベントから受信したテンプレート内容をテキストボックスに反映させる
        /// </summary>
        /// <param name="text"></param>
        private void SetTextInput(InputTemplate obj)
        {
            if (obj.SendFromViewModelName != "BufferViewModel")
            {
                TextInput = obj.InputText;
            }
        }
    }
}
