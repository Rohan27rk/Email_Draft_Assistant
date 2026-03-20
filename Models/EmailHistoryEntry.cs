namespace Email_Assistant.Models
{
    /// <summary>
    /// Represents a single entry in the email history.
    /// Contains both the request parameters and the generated response.
    /// </summary>
    public class EmailHistoryEntry
    {
        public DateTime Timestamp { get; set; }
        public EmailRequest Request { get; set; } = new();
        public EmailResponse Response { get; set; } = new();
    }
}
