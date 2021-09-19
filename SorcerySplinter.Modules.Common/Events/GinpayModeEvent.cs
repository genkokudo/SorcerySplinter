using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace SorcerySplinter.Modules.Common.Events
{
    /// <summary>
    /// 自分用モードかどうか通知する
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
