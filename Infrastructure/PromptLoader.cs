using Email_Assistant.Models;

namespace Email_Assistant.Infrastructure
{
    public static class PromptLoader
    {
        private static string? _systemPrompt;
        private static string? _userPromptTemplate;

        /// <summary>
        /// Loads the system prompt from the Prompts/SystemPrompt.txt file.
        /// The prompt is cached after the first load for performance.
        /// </summary>
        public static string GetSystemPrompt()
        {
            if (_systemPrompt == null)
            {
                var promptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Prompts", "SystemPrompt.txt");
                
                Console.WriteLine($"DEBUG: Looking for system prompt at: {promptPath}");
                
                if (!File.Exists(promptPath))
                {
                    Console.WriteLine($"DEBUG: System prompt file not found, checking alternative paths...");
                    
                    // Try alternative paths
                    var altPath1 = Path.Combine(Directory.GetCurrentDirectory(), "Prompts", "SystemPrompt.txt");
                    var altPath2 = Path.Combine(AppContext.BaseDirectory, "Prompts", "SystemPrompt.txt");
                    
                    Console.WriteLine($"DEBUG: Trying path 1: {altPath1}");
                    Console.WriteLine($"DEBUG: Trying path 2: {altPath2}");
                    
                    if (File.Exists(altPath1))
                    {
                        promptPath = altPath1;
                    }
                    else if (File.Exists(altPath2))
                    {
                        promptPath = altPath2;
                    }
                    else
                    {
                        throw new FileNotFoundException($"System prompt file not found at: {promptPath}");
                    }
                }

                _systemPrompt = File.ReadAllText(promptPath);
                Console.WriteLine($"DEBUG: Loaded system prompt, length: {_systemPrompt.Length}");
            }

            return _systemPrompt;
        }

        /// <summary>
        /// Loads the user prompt template from the Prompts/UserPromptTemplate.txt file
        /// and replaces placeholders with actual values from the EmailRequest.
        /// </summary>
        public static string GetUserPrompt(EmailRequest request)
        {
            if (_userPromptTemplate == null)
            {
                var promptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Prompts", "UserPromptTemplate.txt");
                
                if (!File.Exists(promptPath))
                {
                    throw new FileNotFoundException($"User prompt template file not found at: {promptPath}");
                }

                _userPromptTemplate = File.ReadAllText(promptPath);
            }

            // Replace placeholders with actual values
            return _userPromptTemplate
                .Replace("{Tone}", request.Tone)
                .Replace("{EmailType}", request.EmailType)
                .Replace("{RecipientName}", request.RecipientName)
                .Replace("{KeyPoints}", request.KeyPoints);
        }

        /// <summary>
        /// Reloads the prompts from disk. Useful for development when prompts are being edited.
        /// </summary>
        public static void ReloadPrompts()
        {
            _systemPrompt = null;
            _userPromptTemplate = null;
        }
    }
}
