
using Microsoft.WindowsAPICodePack.Dialogs;
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

namespace SorcerySplinter.Modules.Common.ViewModels
{
    public class ConfigViewModel : BindableBase
    {
        /// <summary>画面遷移するコマンド</summary>
        public DelegateCommand SaveCommand { get; private set; }

        /// <summary>フォルダ選択するコマンド</summary>
        public DelegateCommand<string> FolderCommand { get; private set; }

        /// <summary>ファイル選択するコマンド</summary>
        public DelegateCommand FileCommand { get; private set; }

        /// <summary>自分用モードであることを他のモジュールに通知する</summary>
        public IEventAggregator EventAggregator { get; set; }

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

        // スニペット作成用メモファイル（自分用モード）
        private string _ginpayModeFile;
        public string GinpayModeFile
        {
            get { return _ginpayModeFile; }
            set { SetProperty(ref _ginpayModeFile, value); }
        }

        // 自分用モードのオンオフ
        private bool _isGinpayMode;
        public bool IsGinpayMode
        {
            get { return _isGinpayMode; }
            set { SetProperty(ref _isGinpayMode, value); }
        }

        public ConfigViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;

            // 設定の読み込み
            Author = ModuleSettings.Default.Author;
            SnippetDirectory = ModuleSettings.Default.SnippetDirectory;
            SnippetDirectoryVs = ModuleSettings.Default.SnippetDirectoryVs;
            GinpayModeFile = ModuleSettings.Default.GinpayModeFile;
            IsGinpayMode = ModuleSettings.Default.IsGinpayMode;

            // コマンド設定
            SaveCommand = new DelegateCommand(SaveConfig);
            FolderCommand = new DelegateCommand<string>(ChooseFolder);
            FileCommand = new DelegateCommand(ChooseFile);
        }

        /// <summary>
        /// 設定を保存する
        /// </summary>
        private void SaveConfig()
        {
            ModuleSettings.Default.Author = Author;
            ModuleSettings.Default.SnippetDirectory = SnippetDirectory;
            ModuleSettings.Default.SnippetDirectoryVs = SnippetDirectoryVs;
            ModuleSettings.Default.GinpayModeFile = GinpayModeFile;
            ModuleSettings.Default.IsGinpayMode = IsGinpayMode;

            ModuleSettings.Default.Save();

            // 設定内容を他のモジュールに通知
            EventAggregator.GetEvent<GinpayModeEvent>()
                .Publish(new GinpayMode { IsGinpayMode = IsGinpayMode });
        }

        /// <summary>
        /// ファイル選択ボタン
        /// 今の所モードとか無し
        /// </summary>
        private void ChooseFile()
        {
            var result = OpenFileDialog();
            if (!string.IsNullOrWhiteSpace(result))
            {
                GinpayModeFile = result;
            }
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
                IsFolderPicker = true
            };
            if (cofd.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return string.Empty;
            }

            return cofd.FileName;
        }

        /// <summary>
        /// ファイル選択ダイアログを表示
        /// </summary>
        /// <returns>選択しない場合は空文字</returns>
        private string OpenFileDialog(string defaultPath = "")
        {
            using var cofd = new CommonOpenFileDialog()
            {
                Title = "ファイルを選択してください",
                InitialDirectory = defaultPath
            };
            if (cofd.ShowDialog() != CommonFileDialogResult.Ok)
            {
                return string.Empty;
            }

            return cofd.FileName;
        }
    }
}
