using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using SnippetGenerator;
using SnippetGenerator.Common;
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
    public class EditViewModel : BindableBase
    {
        /// <summary>変数リスト</summary>
        public List<TemplateVariable> Variables { get; set; }

        ///// <summary>インポートリスト（未実装）</summary>
        //public List<TemplateVariable> Imports { get; set; }

        /// <summary>テンプレート更新コマンド</summary>
        public DelegateCommand<string> SetTextCommand { get; private set; }

        /// <summary>出力コマンド</summary>
        public DelegateCommand OutputCommand { get; private set; }

        /// <summary>VSフォルダを開くコマンド</summary>
        public DelegateCommand OpenVsFolderCommand { get; private set; }

        /// <summary>任意フォルダを開くコマンド</summary>
        public DelegateCommand OpenCommonFolderCommand { get; private set; }

        /// <summary>他のモジュールに通知する</summary>
        public IEventAggregator EventAggregator { get; set; }

        /// <summary>スニペット出力サービス</summary>
        public ISnippetService SnippetService { get; set; }

        // ファイル名＆ショートカットフレーズ
        private string _shortcut;
        public string Shortcut
        {
            get { return _shortcut; }
            set { SetProperty(ref _shortcut, value); }
        }

        // 言語
        private string _language;
        public string Language
        {
            get { return _language; }
            set { SetProperty(ref _language, value); }
        }

        // 特殊文字
        private string _delimiter;
        public string Delimiter
        {
            get { return _delimiter; }
            set { SetProperty(ref _delimiter, value); }
        }

        // 説明
        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        // テンプレート入力内容
        private string _templateInput;
        public string TemplateInput
        {
            get { return _templateInput; }
            set { SetProperty(ref _templateInput, value); }
        }

        // フォルダ存在確認
        private bool _isExistsCommonFolder;
        public bool IsExistsCommonFolder
        {
            get { return _isExistsCommonFolder; }
            set { SetProperty(ref _isExistsCommonFolder, value); }
        }
        // フォルダ存在確認VS
        private bool _isExistsVsFolder;
        public bool IsExistsVsFolder
        {
            get { return _isExistsVsFolder; }
            set { SetProperty(ref _isExistsVsFolder, value); }
        }

        public EditViewModel(IEventAggregator eventAggregator, ISnippetService snippetService)
        {
            EventAggregator = eventAggregator;
            SnippetService = snippetService;

            Variables = new List<TemplateVariable>
            {
                new TemplateVariable{Name = "1郎", Description = "説明1", DefValue = "aaaa", IsClassName = true },
                new TemplateVariable{Name = "2郎", Description = "説明2", DefValue = "bbbb", IsClassName = false }
            };

            // コマンドを設定
            SetTextCommand = new DelegateCommand<string>(SetText);
            OutputCommand = new DelegateCommand(Output);
            OpenVsFolderCommand = new DelegateCommand(OpenVsFolder);
            OpenCommonFolderCommand = new DelegateCommand(OpenCommonFolder);

            // 通知イベントを設定
            eventAggregator.GetEvent<InputTemplateEvent>().Subscribe(SetTextInput);

            // モジュールからの通知内容を設定
            eventAggregator.GetEvent<GinpayModeEvent>().Subscribe(SetIsGinpayMode);

            // 初期化処理の1つ
            SetIsGinpayMode(new GinpayMode());

            // 初期値
            Shortcut = string.Empty;
            Delimiter = "$";
            Description = string.Empty;
            TemplateInput = string.Empty;
            Language = "CSharp";
        }

        /// <summary>
        /// コンフィグの更新を受信
        /// </summary>
        /// <param name="isGinpayMode"></param>
        private void SetIsGinpayMode(GinpayMode obj)
        {
            // フォルダ存在確認
            IsExistsCommonFolder = Directory.Exists(ModuleSettings.Default.SnippetDirectory);
            IsExistsVsFolder = Directory.Exists(ModuleSettings.Default.SnippetDirectoryVs);
        }

        private void Output()
        {
            // TODO:ファイルの存在確認ぐらいはしてあげよう。
            MessageBox.Show($"Output");

            // スニペットXML
            var language = SnippetGenerator.Common.Language.CSharp;
            switch (Language)
            {
                case "XAML":
                    language = SnippetGenerator.Common.Language.XAML;
                    break;
                case "JavaScript":
                    language = SnippetGenerator.Common.Language.JavaScript;
                    break;
                case "TypeScript":
                    language = SnippetGenerator.Common.Language.TypeScript;
                    break;
                case "HTML":
                    language = SnippetGenerator.Common.Language.HTML;
                    break;
                default:
                    language = SnippetGenerator.Common.Language.CSharp;
                    break;
            }

            var input = new Snippet(Shortcut, ModuleSettings.Default.Author, Description, Shortcut, TemplateInput, language, Delimiter, Kind.Any);

            var declarations = new List<Literal>();
            foreach (var variable in Variables)
            {
                declarations.Add(new Literal
                {
                    Default = variable.DefValue,
                    Id = variable.Name,
                    ToolTip = variable.Description,
                    FunctionValue = null,
                    Function = variable.IsClassName ? Function.ClassName : Function.None
                });
            }
            input.Declarations = declarations;
            // TODO:ライブラリとクライアントの両方にnullチェックを付けること。
            var xml = SnippetService.MakeSnippetXml(input).ToString();
            //writer.Write
            MessageBox.Show(xml);
        }

        private void OpenVsFolder()
        {
            System.Diagnostics.Process.Start("explorer.exe", ModuleSettings.Default.SnippetDirectoryVs);
        }

        private void OpenCommonFolder()
        {
            System.Diagnostics.Process.Start("explorer.exe", ModuleSettings.Default.SnippetDirectoryVs);
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

    /// <summary>
    /// テンプレートで使用する変数
    /// </summary>
    public class TemplateVariable
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string DefValue { get; set; }
        public bool IsClassName { get; set; }
    }
}
