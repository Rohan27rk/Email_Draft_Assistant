using EmailAssistant.UI.Models;

namespace EmailAssistant.UI.Services
{
    /// <summary>
    /// Service for communicating with the Email_Assistant backend API.
    /// </summary>
    public interface IApiService
    {
        /// <summary>
        /// Sends an email generation request to the backend.
        /// </summary>
        Task<EmailResponse> GenerateEmailAsync(EmailRequest request);

        /// <summary>
        /// Retrieves all email history entries from the backend.
        /// </summary>
        Task<List<EmailHistoryEntry>> GetEmailHistoryAsync();
    }
}
