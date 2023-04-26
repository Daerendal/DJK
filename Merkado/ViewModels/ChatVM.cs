using DJK.Models;

namespace DJK.ViewModels
{
    public class ChatVM
    {
        public string NewMessege { get; set; } = string.Empty;
        public string ToUser { get; set; }
        public List<ChatMessage> Messages { get; set; }= new List<ChatMessage>();
    }
}
