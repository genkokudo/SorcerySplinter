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

        /// <summary>出力ボタンの有効化を判断するコマンド</summary>
        public DelegateCommand SetIsEnableOutputCommand { get; private set; }

        /// <summary>他のモジュールに通知する</summary>
        public IEventAggregator EventAggregator { get; set; }

        /// <summary>スニペット出力サービス</summary>
        public ISnippetService SnippetService { get; set; }

        /// <summary>言語選択肢</summary>
        public Dictionary<string, Language> LanguageDictionary { get; set; }

        // ファイル名＆ショートカットフレーズ
        private string _shortcut;
        public string Shortcut
        {
            get { return _shortcut; }
            set { SetProperty(ref _shortcut, value); }
        }

        // 言語
        private Language _language;
        public Language Language
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

        // フォルダ存在確認VS
        private bool _isEnableOutput;
        public bool IsEnableOutput
        {
            get { return _isEnableOutput; }
            set { SetProperty(ref _isEnableOutput, value); }
        }

        /// <summary>
        /// EnumをDictionaryにする
        /// 便利なメソッドなのでとっておきたい
        /// </summary>
        /// <typeparam name="EnumType"></typeparam>
        /// <returns></returns>
        static public Dictionary<string, EnumType> CreateEnumDictionary<EnumType>()
        {
            return Enum.GetValues(typeof(EnumType)).Cast<EnumType>().ToDictionary(t => t.ToString(), t => t);
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

            LanguageDictionary = CreateEnumDictionary<Language>();

            // コマンドを設定
            SetTextCommand = new DelegateCommand<string>(SetText);
            OutputCommand = new DelegateCommand(Output);
            OpenVsFolderCommand = new DelegateCommand(OpenVsFolder);
            OpenCommonFolderCommand = new DelegateCommand(OpenCommonFolder);
            SetIsEnableOutputCommand = new DelegateCommand(SetIsEnableOutput);

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
            Language = Language.CSharp;
            IsEnableOutput = false;
        }

        /// <summary>
        /// ファイル名と特殊文字が入力されていることをチェックする
        /// </summary>
        private void SetIsEnableOutput()
        {
            IsEnableOutput = !string.IsNullOrWhiteSpace(Shortcut) && !string.IsNullOrWhiteSpace(Delimiter);
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
            // ファイルの存在確認
            var res = MessageBox.Show(
                "同じファイル名のスニペットがあります。上書きしますか？",
                "確認",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question, MessageBoxResult.Cancel
            );
            if (res == MessageBoxResult.Cancel)
            {
                return;
            }

            var input = new Snippet(Shortcut, ModuleSettings.Default.Author, Description, Shortcut, TemplateInput, Language, Delimiter, Kind.Any);

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

            try
            {
                var output = SnippetService.MakeSnippetXml(input);

                var xml = output.ToString();
                MessageBox.Show(xml);       // TODO:ここを完成させよう

                MessageBox.Show("保存しました。");
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}", "だめじゃん",MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
