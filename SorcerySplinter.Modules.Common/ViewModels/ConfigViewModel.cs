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
        /// <summary>画面遷移するコマンド</summary>
        public DelegateCommand SaveCommand { get; private set; }

        // 作者名
        private string _author;
        public string Author
        {
            get { return _author; }
            set { SetProperty(ref _author, value); }
        }

        // スニペットを保存するディレクトリ（リポジトリ用）
        private string _snippetDirectory;
        public string SnippetDirectory
        {
            get { return _snippetDirectory; }
            set { SetProperty(ref _snippetDirectory, value); }
        }

        // スニペットを保存するディレクトリ（VisualStudio用）
        private string _snippetDirectoryVs;
        public string SnippetDirectoryVs
        {
            get { return _snippetDirectoryVs; }
            set { SetProperty(ref _snippetDirectoryVs, value); }
        }

        public ConfigViewModel()
        {
            Author = "aaaa";
            SnippetDirectory = "bbbb";
            SnippetDirectoryVs = "cccc";

            // 設定の読み込み

            // コマンド設定
            SaveCommand = new DelegateCommand(SaveConfig);
        }

        /// <summary>
        /// 設定を保存する
        /// </summary>
        private void SaveConfig()
        {
        }
    }
}
