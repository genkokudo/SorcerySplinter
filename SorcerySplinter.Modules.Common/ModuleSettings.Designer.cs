﻿//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.42000
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SorcerySplinter.Modules.Common {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.8.1.0")]
    internal sealed partial class ModuleSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static ModuleSettings defaultInstance = ((ModuleSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new ModuleSettings())));
        
        public static ModuleSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string Author {
            get {
                return ((string)(this["Author"]));
            }
            set {
                this["Author"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string SnippetDirectory {
            get {
                return ((string)(this["SnippetDirectory"]));
            }
            set {
                this["SnippetDirectory"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string SnippetDirectoryVs {
            get {
                return ((string)(this["SnippetDirectoryVs"]));
            }
            set {
                this["SnippetDirectoryVs"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool GinpayMode {
            get {
                return ((bool)(this["GinpayMode"]));
            }
            set {
                this["GinpayMode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string GinpayModeFile {
            get {
                return ((string)(this["GinpayModeFile"]));
            }
            set {
                this["GinpayModeFile"] = value;
            }
        }
    }
}
