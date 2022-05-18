using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SnippetGenerator;
using SnippetGenerator.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SorcerySplinter.Modules.Common.ViewModels
{
    // 取り敢えず、VSフォルダに対して、ファイルの一覧を取る。
    public class LoadViewModel : RegionViewModelBase
    {
        /// <summary>スニペット出力</summary>
        private ISnippetService _snippetService;

        /// <summary>画面遷移（DIするのではなく、遷移時に受け取る）</summary>
        private IRegionNavigationService _regionNavigationService;

        /// <summary>言語選択時のコマンド</summary>
        public DelegateCommand SelectLanguageCommand { get; private set; }

        /// <summary>スニペット選択時のコマンド</summary>
        public DelegateCommand SelectSnippetCommand { get; private set; }

        /// <summary>読み込みボタンのコマンド</summary>
        public DelegateCommand LoadSnippetCommand { get; private set; }

        /// <summary>削除ボタンのコマンド</summary>
        public DelegateCommand DeleteSnippetCommand { get; private set; }

        /// <summary>画面アクセス時に読み込む全ての言語のスニペットリスト</summary>
        public Dictionary<Language, List<SnippetInfo>> SnippetListDictionary;

        /// <summary>画面アクセス時に読み込む全ての言語のスニペットリスト（削除用にもう一方のパスのリストを持つ）</summary>
        public Dictionary<Language, List<SnippetInfo>> SnippetListDictionarySecondary;

        // この画面を使用できるか
        private bool _isEnabledList;
        public bool IsEnabledList
        {
            get { return _isEnabledList; }
            set { SetProperty(ref _isEnabledList, value); }
        }

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
        private Language? _language;
        public Language? Language
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

        // ボタンの有効化
        private bool _isEnableButton;
        public bool IsEnableButton
        {
            get { return _isEnableButton; }
            set { SetProperty(ref _isEnableButton, value); }
        }

        public LoadViewModel(ISnippetService snippetService)
        {
            // DI
            _snippetService = snippetService;

            // コマンド設定
            SelectLanguageCommand = new DelegateCommand(SelectLanguage);
            SelectSnippetCommand = new DelegateCommand(SelectSnippet);
            LoadSnippetCommand = new DelegateCommand(LoadSnippet);
            DeleteSnippetCommand = new DelegateCommand(DeleteSnippet);

            // 初期値
            Language = null;
        }

        /// <summary>
        /// 言語選択時
        /// スニペットのリストを表示する
        /// 選択状態を解除する
        /// </summary>
        private void SelectLanguage()
        {
            // 読み込んでおいたスニペットリストから、ファイルリストを表示
            if (Language != null)
            {
                SnippetList = SnippetListDictionary[Language.Value];
            }
        }

        /// <summary>
        /// スニペット選択時
        /// ※選択解除されたときも呼ばれるので注意
        /// </summary>
        private void SelectSnippet()
        {
            IsEnableButton = Snippet != null;   // ボタンの有効化
        }

        /// <summary>
        /// 読み込むボタン
        /// </summary>
        private void LoadSnippet()
        {
            // 遷移処理
            var param = new NavigationParameters
            {
                { "SnippetFullPath", Snippet.FullPath }
            };
            _regionNavigationService.RequestNavigate(ViewNames.ViewEdit, param);

        }

        /// <summary>
        /// 削除ボタン
        /// </summary>
        private void DeleteSnippet()
        {
            var res = MessageBox.Show(
                "本当にこのスニペットを削除してしまいますが\nよろしいですか？",
                "確認メッセージ",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question, MessageBoxResult.Cancel
                );
            if (res != MessageBoxResult.Cancel)
            {
                if (SnippetListDictionarySecondary != null)
                {
                    // もう一方も削除する
                    if (SnippetListDictionarySecondary.Keys.Contains(Language.Value))
                    {
                        var another = SnippetListDictionarySecondary[Language.Value].FirstOrDefault(x => x.Title == Snippet.Title);
                        if (another != null)
                        {
                            File.Delete(another.FullPath);
                            SnippetListDictionary[Language.Value].Remove(another);
                        }
                    }
                }
                File.Delete(Snippet.FullPath);
                SnippetListDictionary[Language.Value].Remove(Snippet);
                Snippet = null;

                MessageBox.Show("削除しました。", "結果", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // スニペットを保存するディレクトリ（VSが優先）
        private string _snippetDirectoryVs;
        public string SnippetDirectoryVs
        {
            get { return _snippetDirectoryVs; }
            set { SetProperty(ref _snippetDirectoryVs, value); }
        }

        // 表示した時の処理
        public override void OnNavigatedTo(NavigationContext navigationContext)
        {
            base.OnNavigatedTo(navigationContext);

            _regionNavigationService = navigationContext.NavigationService;

            // 初期値
            IsEnableButton = false;
            Language = null;

            // 設定している保存ディレクトリを取得、VSフォルダを設定していない場合は任意フォルダを使用する
            SnippetDirectoryVs = ModuleSettings.Default.SnippetDirectoryVs;
            if (string.IsNullOrWhiteSpace(SnippetDirectoryVs))
            {
                SnippetDirectoryVs = ModuleSettings.Default.SnippetDirectory;
                if (string.IsNullOrWhiteSpace(SnippetDirectoryVs))
                {
                    // 両方設定していなければ画面（最初のリスト）を無効化する
                    IsEnabledList = false;
                    MessageBox.Show($"先にスニペットを保存するディレクトリを設定してください。");
                    return;
                }
            }
            else
            {
                // VSが設定されていれば、もう一方も読み込む（ない場合もある）
                var snippetDirectoryCommon = ModuleSettings.Default.SnippetDirectory;
                if (!string.IsNullOrWhiteSpace(snippetDirectoryCommon))
                {
                    SnippetListDictionarySecondary = _snippetService.GetSnippetList(snippetDirectoryCommon);
                }
            }

            IsEnabledList = true;
            // 全ての言語のスニペットのリストを読み込む
            SnippetListDictionary = _snippetService.GetSnippetList(SnippetDirectoryVs);

            // 言語選択肢を作成する
            LanguageDictionary = SnippetListDictionary.Keys.ToDictionary(t => t.ToString() == "CSharp" ? "C#" : t.ToString(), t => t);
        }
    }
}
