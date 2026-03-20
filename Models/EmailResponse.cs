namespace EmailAssistant.UI.Models
{
    public class EmailResponse
    {
        public string Subject { get; set; } = string.Empty;
        public string Greeting { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Closing { get; set; } = string.Empty;
    }
}
