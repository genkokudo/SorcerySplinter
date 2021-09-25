using Prism.Events;

namespace SorcerySplinter.Modules.Common.Events
{
    /// <summary>
    /// テンプレート入力画面と書き溜め画面で入力内容を共有するため
    /// 互いのViewに通知する
    /// </summary>
    public class InputTemplateEvent : PubSubEvent<InputTemplate>
    {
    }

    public class InputTemplate
    {
        /// <summary>
        /// 入力内容
        /// </summary>
        public string InputText { get; set; }

        /// <summary>
        /// 送信元ViewModel名
        /// </summary>
        public string SendFromViewModelName { get; set; }
    }
}
