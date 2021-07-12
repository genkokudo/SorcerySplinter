using SorcerySplinter.Services.Interfaces;
using System.Text;

namespace SorcerySplinter.Services
{
    public class MessageService : IMessageService
    {
        public string GetTopMessage()
        {
            var sb = new StringBuilder();
            sb.Append("VisualStudioのコードスニペットファイルを作成します。\n");
            sb.Append("作ったXMLファイルはここに保存しましょう。\n");
            sb.Append(@"C:\Users\(username)\Documents\Visual Studio 2019\Code Snippets\Visual C#\My Code Snippets");
            return sb.ToString();
        }
    }
}
