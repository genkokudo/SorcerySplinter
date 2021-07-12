
namespace SorcerySplinter.Services.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// 最初の画面に表示するメッセージを取得します。
        /// </summary>
        /// <returns>画面に表示するメッセージ</returns>
        string GetTopMessage();
    }
}
