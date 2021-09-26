using Prism.Events;

namespace SorcerySplinter.Modules.Common.Events
{
    /// <summary>
    /// 自分用モードかどうか通知する
    /// コンフィグの更新があったら通知されるので、その検出にも使用できる
    /// </summary>
    public class GinpayModeEvent : PubSubEvent<GinpayMode>
    {

    }

    public class GinpayMode {
        /// <summary>
        /// 自分用モードならばtrue
        /// </summary>
        public bool IsGinpayMode { get; set; }
    }
}
