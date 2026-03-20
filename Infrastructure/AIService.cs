using Azure;
using Azure.AI.OpenAI;
using Email_Assistant.Models;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using System.Text.Json;

namespace Email_Assistant.Infrastructure
{
    public class AIService
    {
        private readonly IConfiguration _config;

        public AIService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<EmailResponse> GenerateEmailAsync(EmailRequest request)
        {

            var endpoint = _config["AzureOpenAI:Endpoint"]
                ?? throw new Exception("Endpoint not configured");

            var apiKey = _config["AzureOpenAI:ApiKey"]
                ?? throw new Exception("ApiKey not configured");

            var deployment = _config["AzureOpenAI:DeploymentName"]
                ?? throw new Exception("Deployment not configured");

            var client = new AzureOpenAIClient(
                new Uri(endpoint),
                new AzureKeyCredential(apiKey));

            var chatClient = client.GetChatClient(deployment);

            // Load prompts from external files for easy editing
            var systemPrompt = PromptLoader.GetSystemPrompt();
            var userPrompt = PromptLoader.GetUserPrompt(request);

            Console.WriteLine($"DEBUG: System Prompt Length: {systemPrompt.Length}");
            Console.WriteLine($"DEBUG: User Prompt: '{userPrompt}'");

            var messages = new List<ChatMessage>
            {
                new SystemChatMessage(systemPrompt),
                new UserChatMessage(userPrompt)
            };

            // Define the function that AI can call - AI will generate the content
            var generateEmailTool = ChatTool.CreateFunctionTool(
                functionName: "generate_email",
                functionDescription: "Generates a professional email with proper structure. Only call this function if the request is about drafting an email.",
                functionParameters: BinaryData.FromString("""
                {
                    "type": "object",
                    "properties": {
                        "subject": {
                            "type": "string",
                            "description": "The email subject line - should be contextually appropriate for the email type and content"
                        },
                        "greeting": {
                            "type": "string", 
                            "description": "The email greeting - should match the requested tone (e.g., 'Dear John,' for formal, 'Hi John,' for friendly)"
                        },
                        "body": {
                            "type": "string",
                            "description": "The main email content - should incorporate the key points naturally and maintain the requested tone throughout"
                        },
                        "closing": {
                            "type": "string",
                            "description": "The email closing - should be appropriate for the tone (e.g., 'Sincerely,' for formal, 'Best regards,' for professional)"
                        }
                    },
                    "required": ["subject", "greeting", "body", "closing"]
                }
                """));

            // Configure chat completion with function calling
            var chatCompletionOptions = new ChatCompletionOptions
            {
                Tools = { generateEmailTool }
            };

            try
            {
                var response = await chatClient.CompleteChatAsync(messages, chatCompletionOptions);
                var chatCompletion = response.Value;

                // Get the AI's response content
                var content = chatCompletion.Content.FirstOrDefault()?.Text ?? "";
                
                // Debug logging to see what AI is responding with
                Console.WriteLine($"DEBUG: AI Response Content: '{content}'");
                Console.WriteLine($"DEBUG: Finish Reason: {chatCompletion.FinishReason}");
                Console.WriteLine($"DEBUG: Tool Calls Count: {chatCompletion.ToolCalls?.Count ?? 0}");

                // Check if AI refused the request with plain text error (more comprehensive detection)
                if (content.Contains("Sorry, I can't help you answer this question") || 
                    content.Contains("I can only assist with email-related drafting tasks") ||
                    content.Contains("can't help") ||
                    content.Contains("only assist with email") ||
                    content.Contains("email-related drafting") ||
                    content.Contains("not about email"))
                {
                    Console.WriteLine("DEBUG: Detected refusal in AI response - throwing exception");
                    throw new Exception("Sorry, I can't help you answer this question. I can only assist with email-related drafting tasks.");
                }

                // Check if AI refused with JSON error format
                if (content.Contains("\"error\"") && content.Contains("Sorry, I can't help you answer this question"))
                {
                    Console.WriteLine("DEBUG: Detected JSON error format in AI response - throwing exception");
                    throw new Exception("Sorry, I can't help you answer this question. I can only assist with email-related drafting tasks.");
                }

                // Check if AI wants to call the function
                if (chatCompletion.FinishReason == ChatFinishReason.ToolCalls)
                {
                    var toolCall = chatCompletion.ToolCalls.FirstOrDefault();
                    if (toolCall != null && toolCall.FunctionName == "generate_email")
                    {
                        // Parse the AI-generated email content from function arguments
                        var emailResponse = JsonSerializer.Deserialize<EmailResponse>(
                            toolCall.FunctionArguments,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        if (emailResponse == null)
                        {
                            throw new Exception("Invalid email content received from AI.");
                        }

                        // Validate the AI-generated email
                        ValidateEmail(emailResponse);
                        
                        return emailResponse;
                    }
                    else
                    {
                        // Tool call exists but not the expected function
                        throw new Exception("Unexpected function call received from AI.");
                    }
                }

                // If no function call, try to parse direct JSON response
                var directEmail = TryParseEmailFromContent(content);
                if (directEmail != null)
                {
                    ValidateEmail(directEmail);
                    return directEmail;
                }

                // If we get here, the AI didn't provide a valid response
                throw new Exception("Failed to generate email. Please try again.");
            }
            catch (JsonException)
            {
                throw new Exception("Failed to parse AI response. Please try again.");
            }
            catch (RequestFailedException ex)
            {
                throw new Exception($"Azure OpenAI error: {ex.Message}");
            }
            catch (Exception ex) when (ex.Message.Contains("email-related drafting tasks"))
            {
                // Re-throw our custom error message
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating email: {ex.Message}");
            }
        }

        // Try to parse EmailResponse from AI content
        private static EmailResponse? TryParseEmailFromContent(string content)
        {
            try
            {
                // Try direct JSON parsing first
                return JsonSerializer.Deserialize<EmailResponse>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                // If direct parsing fails, try to extract JSON from text
                var jsonStart = content.IndexOf('{');
                var jsonEnd = content.LastIndexOf('}');
                
                if (jsonStart >= 0 && jsonEnd > jsonStart)
                {
                    var jsonContent = content.Substring(jsonStart, jsonEnd - jsonStart + 1);
                    try
                    {
                        return JsonSerializer.Deserialize<EmailResponse>(
                            jsonContent,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                    catch
                    {
                        return null;
                    }
                }
                
                return null;
            }
        }

        // Validate email has all required fields
        private static void ValidateEmail(EmailResponse email)
        {
            if (email == null)
            {
                throw new Exception("Failed to generate email. Please try again.");
            }

            if (string.IsNullOrWhiteSpace(email.Subject) ||
                string.IsNullOrWhiteSpace(email.Greeting) ||
                string.IsNullOrWhiteSpace(email.Body) ||
                string.IsNullOrWhiteSpace(email.Closing))
            {
                throw new Exception("AI response missing required email fields. Please try again.");
            }
        }

    }
}