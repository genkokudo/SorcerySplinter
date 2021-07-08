using SorcerySplinter.Services.Interfaces;

namespace SorcerySplinter.Services
{
    public class MessageService : IMessageService
    {
        public string GetMessage()
        {
            return "Hello from the Message Service";
        }
    }
}
