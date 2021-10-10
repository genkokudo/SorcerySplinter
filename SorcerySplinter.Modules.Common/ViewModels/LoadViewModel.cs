using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using SnippetGenerator;
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

        public LoadViewModel(ISnippetService snippetService)
        {
            SnippetService = snippetService;
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

            // 読み込む
            var list = SnippetService.GetSnippetList(SnippetDirectoryVs);
            Console.WriteLine();
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
