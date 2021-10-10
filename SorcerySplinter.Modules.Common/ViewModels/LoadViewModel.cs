using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SnippetGenerator;
using SnippetGenerator.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SorcerySplinter.Modules.Common.ViewModels
{
    // 取り敢えず、VSフォルダに対して、ファイルの一覧を取る。
    public class LoadViewModel : BindableBase, IConfirmNavigationRequest
    {
        /// <summary>スニペット出力サービス</summary>
        public ISnippetService SnippetService { get; set; }

        /// <summary>言語選択時のコマンド</summary>
        public DelegateCommand SelectLanguageCommand { get; private set; }

        /// <summary>スニペット選択時のコマンド</summary>
        public DelegateCommand SelectSnippetCommand { get; private set; }

        /// <summary>画面アクセス時に読み込む全ての言語のスニペットリスト</summary>
        public Dictionary<Language, List<SnippetInfo>> SnippetListDictionary;

        // SnippetListDictionaryから選んだ言語のスニペット選択肢
        private List<SnippetInfo> _snippetList;
        public List<SnippetInfo> SnippetList
        {
            get { return _snippetList; }
            set { SetProperty(ref _snippetList, value); }
        }

        // 言語選択肢
        private Dictionary<string, Language> _languageDictionary;
        public Dictionary<string, Language> LanguageDictionary
        {
            get { return _languageDictionary; }
            set { SetProperty(ref _languageDictionary, value); }
        }

        // 選択した言語
        private Language _language;
        public Language Language
        {
            get { return _language; }
            set { SetProperty(ref _language, value); }
        }

        // 選択したスニペット
        private SnippetInfo _snippet;
        public SnippetInfo Snippet
        {
            get { return _snippet; }
            set { SetProperty(ref _snippet, value); }
        }

        public LoadViewModel(ISnippetService snippetService)
        {
            SnippetService = snippetService;

            // コマンド設定
            SelectLanguageCommand = new DelegateCommand(SelectLanguage);
            SelectSnippetCommand = new DelegateCommand(SelectSnippet);

            // 初期値
            Language = Language.CSharp;
        }

        /// <summary>
        /// 言語選択時
        /// スニペットのリストを表示する
        /// 選択状態を解除する
        /// </summary>
        private void SelectLanguage()
        {
            //MessageBox.Show($"{Language}を選択", "だめじゃん", MessageBoxButton.OK, MessageBoxImage.Error);
            // 読み込んでおいたスニペットリストから、ファイルリストを表示
            SnippetList = SnippetListDictionary[Language];
        }

        /// <summary>
        /// スニペット選択時
        /// </summary>
        private void SelectSnippet()
        {
            // TODO:ListViewは、選択すると「選択状態」にそのデータを置く
            MessageBox.Show($"{Snippet.Description}\n{Snippet.FullPath}", $"{Snippet.Title}を選択", MessageBoxButton.OK, MessageBoxImage.Error);

            // TODO:読み込みボタンを作成
            // 「選択状態」のデータがあれば有効化。
            // クリックしたら読み込んで編集画面に遷移する。
        }


        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            continuationCallback(true);
        }

        // スニペットを保存するディレクトリ（VSが優先）
        private string _snippetDirectoryVs;
        public string SnippetDirectoryVs
        {
            get { return _snippetDirectoryVs; }
            set { SetProperty(ref _snippetDirectoryVs, value); }
        }

        // 表示した時の処理
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // 設定している保存ディレクトリを取得、VSフォルダを設定していない場合は任意フォルダを使用する
            SnippetDirectoryVs = ModuleSettings.Default.SnippetDirectoryVs;
            if (string.IsNullOrWhiteSpace(SnippetDirectoryVs))
            {
                SnippetDirectoryVs = ModuleSettings.Default.SnippetDirectory;
                if (string.IsNullOrWhiteSpace(SnippetDirectoryVs))
                {
                    // TODO:両方設定していなければ無効化する
                    MessageBox.Show($"先にスニペットを保存するディレクトリを設定してください。");
                }
            }

            // 全ての言語のスニペットのリストを読み込む
            SnippetListDictionary = SnippetService.GetSnippetList(SnippetDirectoryVs);

            // 言語選択肢を作成する
            LanguageDictionary = SnippetListDictionary.Keys.ToDictionary(t => t.ToString() == "CSharp" ? "C#" : t.ToString(), t => t);
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
