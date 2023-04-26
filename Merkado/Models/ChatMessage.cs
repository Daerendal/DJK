namespace DJK.Models
{
    public class ChatMessage
    {
        public long Id { get; set; }
        public string FromUserId { get; set; } = String.Empty;
        public string ToUserId { get; set; } = String.Empty;
        public string Message { get; set; } = String.Empty;
        public DateTime CreatedDate { get; set; }
        public virtual User FromUser { get; set; }
        public virtual User ToUser { get; set; }
    }
}
