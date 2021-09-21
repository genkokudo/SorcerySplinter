using Prism.Commands;
using Prism.Mvvm;
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

        // 入力内容
        private string _textInput;
        public string TextInput
        {
            get { return _textInput; }
            set { SetProperty(ref _textInput, value); }
        }

        public BufferViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
            ActivateCommand = new DelegateCommand(Init);
        }

        /// <summary>
        /// 画面表示時にテキストを読み込む
        /// </summary>
        private void Init()
        {
            MessageBox.Show($"読み込み処理");
        }

        /// <summary>
        /// 保存する
        /// </summary>
        private void Save()
        {
            MessageBox.Show($"保存しました。");
        }
    }
}
