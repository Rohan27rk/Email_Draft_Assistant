namespace EmailAssistant.UI.Models
{
    public class EmailHistoryEntry
    {
        public DateTime Timestamp { get; set; }
        public EmailRequest Request { get; set; } = new();
        public EmailResponse Response { get; set; } = new();
    }
}
