
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using SnippetGenerator;
using SnippetGenerator.Common;
using SorcerySplinter.Modules.Common.Events;
using SorcerySplinter.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SorcerySplinter.Modules.Common.ViewModels
{
    public class ConfigViewModel : BindableBase, IConfirmNavigationRequest
    {
        /// <summary>保存するコマンド</summary>
        public DelegateCommand SaveCommand { get; private set; }

        /// <summary>フォルダ選択するコマンド</summary>
        public DelegateCommand<string> FolderCommand { get; private set; }

        /// <summary>ファイル選択するコマンド</summary>
        public DelegateCommand FileCommand { get; private set; }

        /// <summary>自分用モード：同期するコマンド</summary>
        public DelegateCommand SynchronizeCommand { get; private set; }

        /// <summary>自分用モードであることを他のモジュールに通知する</summary>
        public IEventAggregator EventAggregator { get; set; }

        /// <summary>スニペット出力サービス</summary>
        public ISnippetService SnippetService { get; set; }

        /// <summary>ディレクトリ作成サービス</summary>
        public IDirectoryService DirectoryService { get; set; }

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

        public ConfigViewModel(IEventAggregator eventAggregator, ISnippetService snippetService, IDirectoryService directoryService)
        {
            EventAggregator = eventAggregator;
            SnippetService = snippetService;
            DirectoryService = directoryService;

            // コマンド設定
            SaveCommand = new DelegateCommand(SaveConfig);
            FolderCommand = new DelegateCommand<string>(ChooseFolder);
            FileCommand = new DelegateCommand(ChooseFile);
            SynchronizeCommand = new DelegateCommand(Synchronize);
        }

        // 同期ボタンの許可
        private bool _isEnableSynchronize;
        public bool IsEnableSynchronize
        {
            get { return _isEnableSynchronize; }
            set { SetProperty(ref _isEnableSynchronize, value); }
        }

        /// <summary>
        /// 設定保存時に呼ぶ
        /// 同期ボタンが押せるかチェックする
        /// 保存場所が2つとも設定されており、存在すること
        /// </summary>
        private void SetIsEnableSynchronize()
        {
            // フォルダ存在確認
            var vs = ModuleSettings.Default.SnippetDirectoryVs;
            var common = ModuleSettings.Default.SnippetDirectory;
            var isEnableVs = !string.IsNullOrWhiteSpace(vs) && Directory.Exists(vs);
            var isEnableCommon = !string.IsNullOrWhiteSpace(common) && Directory.Exists(common);

            IsEnableSynchronize = isEnableVs && isEnableCommon;
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
            IsEnableSynchronize = false;

            // 保存
            ModuleSettings.Default.Save();

            // 設定内容を他のモジュールに通知
            EventAggregator.GetEvent<GinpayModeEvent>()
                .Publish(new GinpayMode { IsGinpayMode = IsGinpayMode });

            // 同期ボタンの許可
            SetIsEnableSynchronize();

            MessageBox.Show($"保存しました");
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
                    MessageBox.Show($"おかしい：ConfigViewModel");
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
                        MessageBox.Show($"おかしい：ConfigViewModel");
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

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            // 変更があるかチェック
            var isModified =
                Author != ModuleSettings.Default.Author ||
                SnippetDirectory != ModuleSettings.Default.SnippetDirectory ||
                SnippetDirectoryVs != ModuleSettings.Default.SnippetDirectoryVs ||
                GinpayModeFile != ModuleSettings.Default.GinpayModeFile ||
                IsGinpayMode != ModuleSettings.Default.IsGinpayMode;

            if (isModified)
            {
                var res = MessageBox.Show(
                    "設定を保存していませんが\n破棄してよろしいですか？",
                    "確認メッセージ",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Question, MessageBoxResult.Cancel
                    );
                continuationCallback(res != MessageBoxResult.Cancel);
            }
            else
            {
                continuationCallback(true);
            }
        }

        /// <summary>
        /// 自分用モード
        /// 任意の保存場所からVS保存場所にファイルを同期する
        /// 同名のファイルがある場合は上書きする
        /// このソフトで使用可能な言語で、存在しているフォルダしか同期しない
        /// </summary>
        private void Synchronize()
        {
            var res = MessageBox.Show(
                "同じファイル名のスニペットなどのチェックは行いません。\n同期してよろしいですか？",
                "確認",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question, MessageBoxResult.Cancel
            );
            if (res == MessageBoxResult.Cancel)
            {
                return ;
            }

            // 設定フォルダ
            var common = ModuleSettings.Default.SnippetDirectory.Replace('/','\\').TrimEnd('\\');
            var vs = ModuleSettings.Default.SnippetDirectoryVs.Replace('/', '\\').TrimEnd('\\');    // ここを揃えておけばおかしなことにならないはず

            // 各言語のフォルダを取得
            var langs = (Language[])Enum.GetValues(typeof(Language));
            foreach (var lang in langs)
            {
                // 各言語について、ディレクトリが取得できたものだけ上書きコピーする
                var langdir = SnippetService.GetLanguagePath(lang);
                var sourceDir = Path.Combine(common, langdir);

                if (Directory.Exists(sourceDir))
                {
                    MessageBox.Show(lang.ToString() + "のスニペットをコピーします。");
                    
                    // コピー元の.snippetファイルを全て挙げる
                    var fileList = new List<string>();
                    DirectoryService.FolderInsiteSearch(sourceDir, fileList, new string[] { ".snippet" });

                    // コピーするが、ディレクトリが無ければ作る
                    foreach (var snippetFilePath in fileList)
                    {
                        var relativePath = Path.GetDirectoryName(snippetFilePath).Replace(common, string.Empty);
                        var filename = Path.GetFileName(snippetFilePath);
                        var destDirectory = vs + relativePath;
                        
                        // フォルダが無ければ作成
                        DirectoryService.SafeCreateDirectory(destDirectory);
                        var destFilePath = Path.Combine(destDirectory, filename);
                        
                        // ファイルを上書きコピー
                        File.Copy(snippetFilePath, destFilePath, true);
                    }

                }
            }
            MessageBox.Show("処理が終わりました。");

            // 各言語について、ディレクトリが取得できたものだけ上書きコピーする
            // TODO:フォルダを再帰的に探索するコードを見つけること。
        }

        // 表示した時の処理
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // 設定の読み込み
            Author = ModuleSettings.Default.Author;
            SnippetDirectory = ModuleSettings.Default.SnippetDirectory;
            SnippetDirectoryVs = ModuleSettings.Default.SnippetDirectoryVs;
            GinpayModeFile = ModuleSettings.Default.GinpayModeFile;
            IsGinpayMode = ModuleSettings.Default.IsGinpayMode;

            // 同期ボタンの許可
            SetIsEnableSynchronize();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            // Viewは使い回さない
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // 画面から離れる時何もしない
        }
    }
}
