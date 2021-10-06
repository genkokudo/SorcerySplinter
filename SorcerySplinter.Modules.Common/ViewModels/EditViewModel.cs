﻿using Prism.Commands;
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
    public class EditViewModel : BindableBase, IConfirmNavigationRequest
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

        /// <summary>ディレクトリ作成サービス</summary>
        public IDirectoryService DirectoryService { get; set; }

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

        // 出力ボタンが有効であるか
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
            // "CSharp" は "C#" と表示する
            return Enum.GetValues(typeof(EnumType)).Cast<EnumType>().ToDictionary(t => t.ToString() == "CSharp" ? "C#" : t.ToString(), t => t);
        }

        public EditViewModel(IEventAggregator eventAggregator, ISnippetService snippetService, IDirectoryService directoryService)
        {
            EventAggregator = eventAggregator;
            SnippetService = snippetService;
            DirectoryService = directoryService;

            // 変数リスト
            Variables = new List<TemplateVariable>();
            //Variables = new List<TemplateVariable>
            //{
            //    new TemplateVariable{Name = "1郎", Description = "説明1", DefValue = "aaaa", IsClassName = true },
            //    new TemplateVariable{Name = "2郎", Description = "説明2", DefValue = "bbbb", IsClassName = false }
            //};

            LanguageDictionary = CreateEnumDictionary<Language>();

            // コマンドを設定
            SetTextCommand = new DelegateCommand<string>(SetText);
            OutputCommand = new DelegateCommand(Output);
            OpenVsFolderCommand = new DelegateCommand(OpenVsFolder);
            OpenCommonFolderCommand = new DelegateCommand(OpenCommonFolder);
            SetIsEnableOutputCommand = new DelegateCommand(SetIsEnableOutput);

            // 通知イベントを設定
            eventAggregator.GetEvent<InputTemplateEvent>().Subscribe(SetTextInput);

            // 初期値
            Shortcut = string.Empty;
            Delimiter = "$";
            Description = string.Empty;
            TemplateInput = string.Empty;
            Language = Language.CSharp;
            IsEnableOutput = false;
        }

        /// <summary>
        /// チェックする
        /// ファイル名が入力されていること
        /// 有効なフォルダが少なくとも1つ設定されていること
        /// </summary>
        private void SetIsEnableOutput()
        {
            var vs = ModuleSettings.Default.SnippetDirectoryVs;
            var common = ModuleSettings.Default.SnippetDirectory;

            // フォルダ存在確認
            var isEnableVs = !string.IsNullOrWhiteSpace(vs) && Directory.Exists(vs);
            var isEnableCommon = !string.IsNullOrWhiteSpace(common) && Directory.Exists(common);

            // 許可条件
            IsEnableOutput = !string.IsNullOrWhiteSpace(Shortcut) && (isEnableVs || isEnableCommon);
        }
        
        /// <summary>
        /// 現在入力している情報でXMLを生成し、ファイルを出力する
        /// フォルダがなかったら作る
        /// 設定でフォルダ指定していない場合は何もしない
        /// ファイル存在確認をする
        /// </summary>
        private void Output()
        {
            // TODO:面倒なのでフォルダ設定していない場合の動作を確認していない。
            // フォルダが設定されていて、そのディレクトリがあることを確認する
            var isUseVs = !string.IsNullOrWhiteSpace(ModuleSettings.Default.SnippetDirectoryVs) && Directory.Exists(ModuleSettings.Default.SnippetDirectoryVs);
            var isUseCommon = !string.IsNullOrWhiteSpace(ModuleSettings.Default.SnippetDirectory) && Directory.Exists(ModuleSettings.Default.SnippetDirectory);
            if (!isUseVs && !isUseCommon)
            {
                MessageBox.Show($"設定されている保存場所が存在しないので保存できません。", "だめじゃん", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // VSのフォルダと任意指定のフォルダをそれぞれ確認する
            var vs = Path.Combine(ModuleSettings.Default.SnippetDirectoryVs, SnippetService.GetLanguagePath(Language));
            var common = Path.Combine(ModuleSettings.Default.SnippetDirectory, SnippetService.GetLanguagePath(Language));

            // ディレクトリがなければ作る
            if (isUseVs)
            {
                DirectoryService.SafeCreateDirectory(vs);
            }
            if (isUseCommon)
            {
                DirectoryService.SafeCreateDirectory(common);
            }

            // ファイルの存在確認
            var vsFilePath = Path.Combine(vs, Shortcut + ".snippet");
            var commonFilePath = Path.Combine(common, Shortcut + ".snippet");
            if (File.Exists(vsFilePath) || File.Exists(commonFilePath))
            {
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
            }

            // テンプレート内変数リスト
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

            // テンプレート生成パラメータの作成
            var input = new Snippet(Shortcut, ModuleSettings.Default.Author, Description, Shortcut, TemplateInput, Language, Delimiter, Kind.Any, declarations, null);

            try
            {
                // ファイル出力処理
                var output = SnippetService.MakeSnippetXml(input);
                var xml = output.ToString();

                var res = MessageBox.Show(xml, "これでいいですか？",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Question, MessageBoxResult.Cancel);
                if (res == MessageBoxResult.Cancel)
                {
                    return;
                }
                if (isUseVs)
                {
                    File.WriteAllText(vsFilePath, xml);
                }
                if (isUseCommon)
                {
                    File.WriteAllText(commonFilePath, xml);
                }
                // TODO:動作確認が終わったら、自分がVSに設定しているスニペットフォルダを全部変更しよう。
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
            System.Diagnostics.Process.Start("explorer.exe", ModuleSettings.Default.SnippetDirectory);
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

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            // 確認すべきだけど面倒なので実装しない
            continuationCallback(true);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            // 何もしない
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            SetIsEnableOutput();
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
