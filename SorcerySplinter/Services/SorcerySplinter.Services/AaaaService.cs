﻿namespace SorcerySplinter.Services
{
    /// <summary>
    /// 取り敢えず適当なサービス
    /// </summary>
    public interface IAaaaService
    {
        /// <summary>
        /// 適当にメッセージを返す
        /// </summary>
        /// <returns></returns>
        string GetMessage();
    }

    public class AaaaService : IAaaaService
    {
        public string GetMessage()
        {
            return "新しいサービスだよ！";
        }
    }
}
