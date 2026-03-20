using EmailAssistant.UI.Models;
using System.Net.Http.Json;

namespace EmailAssistant.UI.Services
{
    /// <summary>
    /// Implementation of API service for backend communication.
    /// </summary>
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;

        public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<EmailResponse> GenerateEmailAsync(EmailRequest request)
        {
            try
            {
                Console.WriteLine($"DEBUG: Sending request to backend - RecipientName: {request.RecipientName}, KeyPoints: {request.KeyPoints}");
                
                var response = await _httpClient.PostAsJsonAsync("/generate-email", request);
                
                Console.WriteLine($"DEBUG: Backend response status: {response.StatusCode}");
                
                if (!response.IsSuccessStatusCode)
                {
                    // Read error message as plain text
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"DEBUG: Backend error content: '{errorContent}'");
                    _logger.LogWarning("Backend returned error: {StatusCode} - {ErrorContent}", response.StatusCode, errorContent);
                    
                    if (!string.IsNullOrEmpty(errorContent))
                    {
                        throw new Exception(errorContent.Trim());
                    }
                    
                    // Fallback error message
                    throw new Exception("Unable to generate email. Please try again.");
                }
                
                var emailResponse = await response.Content.ReadFromJsonAsync<EmailResponse>();
                Console.WriteLine($"DEBUG: Successfully received email response with subject: {emailResponse?.Subject}");
                return emailResponse ?? throw new Exception("Failed to deserialize response");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed: {Message}", ex.Message);
                throw new Exception("Unable to connect to server. Please check your connection.");
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Request timed out");
                throw new Exception("Request timed out. Please try again.");
            }
        }

        public async Task<List<EmailHistoryEntry>> GetEmailHistoryAsync()
        {
            try
            {
                var history = await _httpClient.GetFromJsonAsync<List<EmailHistoryEntry>>("/email-history");
                return history ?? new List<EmailHistoryEntry>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error retrieving email history");
                throw new Exception($"Unable to connect to server: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error retrieving history");
                throw new Exception($"Error retrieving history: {ex.Message}");
            }
        }
    }
}
