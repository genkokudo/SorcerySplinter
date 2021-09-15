
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SorcerySplinter.Modules.Common.ViewModels
{
    public class ConfigViewModel : BindableBase
    {
        /// <summary>画面遷移するコマンド</summary>
        public DelegateCommand SaveCommand { get; private set; }

        /// <summary>VisualStudioフォルダ選択するコマンド</summary>
        public DelegateCommand<string> VsFolderCommand { get; private set; }
        /// <summary>任意フォルダ選択するコマンド</summary>
        public DelegateCommand<string> CommonFolderCommand { get; private set; }

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
            // 設定の読み込み
            Author = ModuleSettings.Default.Author;
            SnippetDirectory = ModuleSettings.Default.SnippetDirectory;
            SnippetDirectoryVs = ModuleSettings.Default.SnippetDirectoryVs;

            // コマンド設定
            SaveCommand = new DelegateCommand(SaveConfig);
            CommonFolderCommand = new DelegateCommand<string>(ChooseFolder);
            VsFolderCommand = new DelegateCommand<string>(ChooseFolder);
        }

        /// <summary>
        /// フォルダ選択ボタン
        /// </summary>
        /// <param name="defaultPath">CommandParameter</param>
        private void ChooseFolder(string mode)
        {
            var path = string.Empty;
            var defaultPath = string.Empty;

            switch (mode)
            {
                case "VS":
                    // VSのスニペットフォルダをデフォルトの場所として設定
                    path = SnippetDirectoryVs;
                    defaultPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\Visual Studio 2019\\Code Snippets";
                    break;
                case "Common":
                    // Gitのフォルダをデフォルトの場所として設定
                    path = SnippetDirectory;
                    defaultPath = $"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\source\\repos";
                    break;
                default:
                    // その他のパターンは現在の所なし
                    System.Windows.MessageBox.Show($"おかしい：ConfigViewModel");
                    break;
            }

            // 入力がない場合または、入力フォルダが存在しない場合、デフォルトの場所を指定する
            if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
            {
                path = defaultPath;
            }

            var result = OpenFolderDialog(path);
            if (!string.IsNullOrWhiteSpace(result))
            {
                switch (mode)
                {
                    case "VS":
                        SnippetDirectoryVs = result;
                        break;
                    case "Common":
                        SnippetDirectory = result;
                        break;
                    default:
                        // その他のパターンは現在の所なし
                        System.Windows.MessageBox.Show($"おかしい：ConfigViewModel");
                        break;
                }
            }
        }

        /// <summary>
        /// 設定を保存する
        /// </summary>
        private void SaveConfig()
        {
            ModuleSettings.Default.Author = Author;
            ModuleSettings.Default.SnippetDirectory = SnippetDirectory;
            ModuleSettings.Default.SnippetDirectoryVs = SnippetDirectoryVs;

            ModuleSettings.Default.Save();
        }

        /// <summary>
        /// フォルダ選択ダイアログを表示
        /// </summary>
        /// <returns>選択しない場合は空文字</returns>
        private string OpenFolderDialog(string defaultPath = "")
        {
            using var cofd = new CommonOpenFileDialog()
            {
                Title = "フォルダを選択してください",
                InitialDirectory = defaultPath,
                // フォルダ選択モードにする
                IsFolderPicker = true,
            };
            if (cofd.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return string.Empty;
            }

            // FileNameで選択されたフォルダを取得する
            return cofd.FileName;
        }
    }
}
