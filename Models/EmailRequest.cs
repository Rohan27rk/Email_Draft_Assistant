using System.ComponentModel.DataAnnotations;

namespace Email_Assistant.Models
{
    public class EmailRequest
    {
        [Required(ErrorMessage = "Please enter recipient name")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Recipient name can only contain letters and spaces")]
        public string RecipientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a tone")]
        public string Tone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select an email type")]
        public string EmailType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter key points")]
        [MaxLength(500, ErrorMessage = "Key points cannot exceed 500 characters")]
        public string KeyPoints { get; set; } = string.Empty;
    }
}
