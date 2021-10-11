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

        /// <summary>読み込みボタンのコマンド</summary>
        public DelegateCommand LoadSnippetCommand { get; private set; }

        /// <summary>削除ボタンのコマンド</summary>
        public DelegateCommand DeleteSnippetCommand { get; private set; }

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
            SnippetService = snippetService;

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
            //MessageBox.Show($"{Language}を選択", "だめじゃん", MessageBoxButton.OK, MessageBoxImage.Error);
            // 読み込んでおいたスニペットリストから、ファイルリストを表示
            SnippetList = SnippetListDictionary[Language.Value];
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
            // クリックしたら読み込んで編集画面に遷移する。
            MessageBox.Show($"読み込む", $"読み込む", MessageBoxButton.OK, MessageBoxImage.Error);

            //var snippetDocument = SnippetService.ReadSnippet(Snippet.FullPath);
            // イベントで次の画面に渡す

            // TODO:イベント作る LoadSnippetEvent
            // ・編集画面にこういう受信を書く
            // eventAggregator.GetEvent<LoadSnippetEvent>().Subscribe(SetLoadedSnippet);

            // ・イベント発行処理を書く
            //EventAggregator.GetEvent<GinpayModeEvent>()
            //    .Publish(new GinpayMode { IsGinpayMode = IsGinpayMode });
            // TODO:でもちょっと待って！OnNavigatedToを使えばイベント要らないのでは？読み込み処理もここでやる必要なくない？

            // 遷移処理を書く
            // IRegionManager regionManagerをDIする。
            var param = new NavigationParameters();
            param.Add("SnippetFullPath", Snippet.FullPath);
            //_regionManager.RequestNavigate(RegionNames.ContentRegion, "Edit", "ここに読み込み用のフルパスを書いて次の画面に渡す！！");

            // 遷移先での受け取り方
            // navigationContext.Parameters["SnippetFullPath"] as string;
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
                MessageBox.Show("削除しました。", "結果", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // TODO:これってVS側しか削除できないから、Common側もList取っておいて同時に削除するようにしたい。
                // Languageとファイル名を使って、Common側のファイル情報を探す
            }
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

            // 初期値
            IsEnableButton = false;
            Language = null;
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
