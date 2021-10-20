using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SorcerySplinter.Modules.Common.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SorcerySplinter.Modules.Common.ViewModels
{
    public class BufferViewModel : RegionViewModelBase
    {
        /// <summary>保存するコマンド</summary>
        public DelegateCommand SaveCommand { get; private set; }

        /// <summary>画面表示時コマンド</summary>
        public DelegateCommand ActivateCommand { get; private set; }

        /// <summary>テンプレート更新コマンド</summary>
        public DelegateCommand<string> SetTextCommand { get; private set; }

        /// <summary>他のモジュールに通知する</summary>
        public IEventAggregator EventAggregator { get; set; }

        // 書き溜めファイル存在確認
        private bool _isExistsFile;
        public bool IsExistsFile
        {
            get { return _isExistsFile; }
            set { SetProperty(ref _isExistsFile, value); }
        }

        // 書き溜めファイル内容
        private string _bufferInput;
        public string BufferInput
        {
            get { return _bufferInput; }
            set { SetProperty(ref _bufferInput, value); }
        }

        // テンプレート入力内容
        private string _templateInput;
        public string TemplateInput
        {
            get { return _templateInput; }
            set { SetProperty(ref _templateInput, value); }
        }

        public BufferViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(Save);
            ActivateCommand = new DelegateCommand(Init);
            SetTextCommand = new DelegateCommand<string>(SetText);

            // 通知イベントを設定
            eventAggregator.GetEvent<InputTemplateEvent>().Subscribe(SetTextInput);

            // 初期化したことを伝えて、もしスニペット入力があれば送ってもらう
            EventAggregator.GetEvent<InputTemplateEvent>()
                .Publish(new InputTemplate { InputText = string.Empty, SendFromViewModelName = "Initialize" });
        }

        /// <summary>
        /// 画面表示時にテキストを読み込む
        /// </summary>
        private void Init()
        {
            var path = ModuleSettings.Default.GinpayModeFile;
            IsExistsFile = File.Exists(path);
            if (IsExistsFile)
            {
                BufferInput = File.ReadAllText(path);
            }
            else
            {
                MessageBox.Show($"指定したファイルが見つかりません。");
                BufferInput = string.Empty;
            }
        }

        /// <summary>
        /// 保存する
        /// </summary>
        private void Save()
        {
            var path = ModuleSettings.Default.GinpayModeFile;
            IsExistsFile = File.Exists(path);
            if (IsExistsFile)
            {
                File.WriteAllText(path, BufferInput);
                MessageBox.Show($"保存しました。");
            }
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
                TemplateInput = obj.InputText;
            }
        }
    }
}
