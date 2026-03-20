# Azure OpenAI Integration Setup Guide

## Overview
This guide will help you integrate Azure OpenAI into your Email Assistant application. All mock code has been removed, and the application is now ready to use real AI.

---

## ✅ What's Been Done

### 1. Removed Mock Code
- ✅ Deleted `Infrastructure/MockAIService.cs`
- ✅ Updated `Program.cs` to use `AIService` instead of `MockAIService`
- ✅ Kept only the real Azure OpenAI implementation

### 2. Updated Configuration
- ✅ `appsettings.json` has placeholders for your credentials
- ✅ `AIService.cs` is ready to use Azure OpenAI
- ✅ `Azure.AI.OpenAI` package (v2.1.0) is already installed

---

## 🔑 Step 1: Get Your Azure OpenAI Credentials

### A. Create Azure OpenAI Resource

1. **Go to Azure Portal**: https://portal.azure.com
2. **Create Resource**:
   - Click "Create a resource"
   - Search for "Azure OpenAI"
   - Click "Create"

3. **Fill in Details**:
   - **Subscription**: Select your subscription
   - **Resource Group**: Create new or use existing
   - **Region**: Choose a region (e.g., East US, West Europe)
   - **Name**: Give it a unique name (e.g., `email-assistant-openai`)
   - **Pricing Tier**: Standard S0

4. **Review + Create**: Click "Create" and wait for deployment

### B. Deploy a Model

1. **Go to Azure OpenAI Studio**: https://oai.azure.com
2. **Select Your Resource**: Choose the resource you just created
3. **Go to Deployments**:
   - Click "Deployments" in the left menu
   - Click "Create new deployment"

4. **Choose Model**:
   - **Model**: Select `gpt-4` or `gpt-35-turbo` (recommended: gpt-4)
   - **Deployment Name**: Give it a name (e.g., `email-generator`)
   - **Version**: Select latest version
   - Click "Create"

5. **Note Your Deployment Name**: You'll need this later (e.g., `email-generator`)

### C. Get Your Credentials

1. **Go to Azure Portal**: https://portal.azure.com
2. **Find Your Resource**: Navigate to your Azure OpenAI resource
3. **Go to "Keys and Endpoint"**: In the left menu under "Resource Management"
4. **Copy These Values**:
   - **Endpoint**: `https://YOUR-RESOURCE-NAME.openai.azure.com/`
   - **Key 1** or **Key 2**: Copy either key (they both work)

---

## 📝 Step 2: Add Credentials to Your Application

### Option A: Using appsettings.json (Development Only)

⚠️ **WARNING**: Never commit API keys to source control!

1. **Open**: `appsettings.json`
2. **Replace placeholders**:

```json
{
  "AzureOpenAI": {
    "Endpoint": "https://YOUR-RESOURCE-NAME.openai.azure.com/",
    "ApiKey": "YOUR-API-KEY-HERE",
    "DeploymentName": "email-generator"
  }
}
```

**Example**:
```json
{
  "AzureOpenAI": {
    "Endpoint": "https://email-assistant-openai.openai.azure.com/",
    "ApiKey": "abc123def456ghi789jkl012mno345pqr678stu901vwx234yz",
    "DeploymentName": "email-generator"
  }
}
```

3. **Add to .gitignore**:
```
appsettings.json
appsettings.*.json
```

### Option B: Using User Secrets (Recommended for Development)

1. **Right-click project** → "Manage User Secrets"
2. **Add to secrets.json**:

```json
{
  "AzureOpenAI": {
    "Endpoint": "https://YOUR-RESOURCE-NAME.openai.azure.com/",
    "ApiKey": "YOUR-API-KEY-HERE",
    "DeploymentName": "email-generator"
  }
}
```

### Option C: Using Environment Variables (Production)

Set these environment variables:

```bash
AzureOpenAI__Endpoint=https://YOUR-RESOURCE-NAME.openai.azure.com/
AzureOpenAI__ApiKey=YOUR-API-KEY-HERE
AzureOpenAI__DeploymentName=email-generator
```

**Windows (PowerShell)**:
```powershell
$env:AzureOpenAI__Endpoint="https://YOUR-RESOURCE-NAME.openai.azure.com/"
$env:AzureOpenAI__ApiKey="YOUR-API-KEY-HERE"
$env:AzureOpenAI__DeploymentName="email-generator"
```

**Linux/Mac**:
```bash
export AzureOpenAI__Endpoint="https://YOUR-RESOURCE-NAME.openai.azure.com/"
export AzureOpenAI__ApiKey="YOUR-API-KEY-HERE"
export AzureOpenAI__DeploymentName="email-generator"
```

---

## 🚀 Step 3: Test the Integration

### 1. Run the Application

**Backend**:
```bash
cd Email_Assistant
dotnet run
```

**Frontend**:
```bash
cd EmailAssistant.UI
dotnet run
```

### 2. Generate a Test Email

1. Open browser: http://localhost:5219
2. Click "Ask anything" input
3. Fill in the form:
   - **Recipient Name**: John Doe
   - **Tone**: Professional
   - **Email Type**: Meeting Request
   - **Key Points**: Discuss Q4 budget planning and resource allocation
4. Click "Generate Email"

### 3. Verify It Works

You should see:
- ✅ Loading spinner
- ✅ AI-generated email appears
- ✅ Email has proper subject, greeting, body, and closing
- ✅ Email is saved to history

---

## 🔧 How the Integration Works

### AIService.cs Breakdown

```csharp
public async Task<EmailResponse> GenerateEmailAsync(EmailRequest request)
{
    // 1. Get credentials from configuration
    var endpoint = _config["AzureOpenAI:Endpoint"];
    var apiKey = _config["AzureOpenAI:ApiKey"];
    var deployment = _config["AzureOpenAI:DeploymentName"];

    // 2. Create Azure OpenAI client
    var client = new AzureOpenAIClient(
        new Uri(endpoint),
        new AzureKeyCredential(apiKey));

    // 3. Get chat client for your deployment
    var chatClient = client.GetChatClient(deployment);

    // 4. Prepare messages (System + User)
    var messages = new List<ChatMessage>
    {
        new SystemChatMessage("You are a professional email assistant..."),
        new UserChatMessage($"Write a {tone} {type} email...")
    };

    // 5. Call Azure OpenAI
    var response = await chatClient.CompleteChatAsync(messages);

    // 6. Parse JSON response
    var content = response.Value.Content[0].Text;
    var email = JsonSerializer.Deserialize<EmailResponse>(content);

    return email;
}
```

### Request Flow

```
User Input (Form)
    ↓
Frontend (Blazor)
    ↓
HTTP POST /generate-email
    ↓
Backend API
    ↓
ValidationService (validates input)
    ↓
AIService (calls Azure OpenAI)
    ↓
Azure OpenAI API
    ↓
AI generates email
    ↓
Response (JSON)
    ↓
EmailHistoryService (saves to history)
    ↓
Return to Frontend
    ↓
Display in Chat
```

---

## 💰 Cost Estimation

### Pricing (Approximate)

**GPT-4**:
- Input: $0.03 per 1K tokens
- Output: $0.06 per 1K tokens

**GPT-3.5-Turbo**:
- Input: $0.0015 per 1K tokens
- Output: $0.002 per 1K tokens

### Typical Email Generation

**Tokens per request**:
- System prompt: ~50 tokens
- User prompt: ~50 tokens
- Response: ~200 tokens
- **Total**: ~300 tokens per email

**Cost per email**:
- GPT-4: ~$0.015 per email
- GPT-3.5-Turbo: ~$0.0006 per email

**Monthly estimates** (GPT-4):
- 100 emails/day = $45/month
- 500 emails/day = $225/month
- 1000 emails/day = $450/month

**Recommendation**: Start with GPT-3.5-Turbo for testing, then upgrade to GPT-4 for production.

---

## 🎯 Prompt Engineering Tips

### Current System Prompt

```
You are a professional email drafting assistant. 
Return ONLY valid JSON with these properties: subject, greeting, body, closing. 
Do NOT include explanations. Do NOT include markdown. Do NOT include extra text.
```

### Improving the Prompt

**For Better Quality**:
```csharp
new SystemChatMessage(
    "You are a professional email drafting assistant with expertise in business communication. " +
    "Generate emails that are clear, concise, and appropriate for the specified tone. " +
    "Return ONLY valid JSON with these exact properties: subject, greeting, body, closing. " +
    "Do NOT include markdown, code blocks, or explanations. " +
    "Ensure the body is well-structured with proper paragraphs.")
```

**For Specific Formatting**:
```csharp
new SystemChatMessage(
    "You are a professional email assistant. " +
    "Return ONLY valid JSON in this exact format: " +
    "{\"subject\": \"...\", \"greeting\": \"...\", \"body\": \"...\", \"closing\": \"...\"} " +
    "The body should be 2-3 paragraphs. Do NOT use markdown or special formatting.")
```

### Adjusting Temperature

Add temperature control for creativity:

```csharp
var options = new ChatCompletionOptions
{
    Temperature = 0.7f  // 0.0 = deterministic, 1.0 = creative
};

var response = await chatClient.CompleteChatAsync(messages, options);
```

**Recommended temperatures**:
- Formal emails: 0.3-0.5
- Professional emails: 0.5-0.7
- Friendly emails: 0.7-0.9

---

## 🐛 Troubleshooting

### Error: "Endpoint not configured"

**Problem**: Missing endpoint in configuration

**Solution**:
1. Check `appsettings.json` has correct endpoint
2. Verify format: `https://YOUR-RESOURCE-NAME.openai.azure.com/`
3. Include trailing slash

### Error: "401 Unauthorized"

**Problem**: Invalid API key

**Solution**:
1. Go to Azure Portal → Your Resource → Keys and Endpoint
2. Copy a fresh API key
3. Update configuration
4. Restart application

### Error: "404 Not Found"

**Problem**: Deployment name doesn't exist

**Solution**:
1. Go to Azure OpenAI Studio → Deployments
2. Verify deployment name (case-sensitive)
3. Update `DeploymentName` in configuration

### Error: "Failed to parse AI response"

**Problem**: AI returned invalid JSON

**Solution**:
1. Check system prompt is clear about JSON format
2. Add JSON mode (see below)
3. Increase temperature if response is too rigid

**Enable JSON Mode**:
```csharp
var options = new ChatCompletionOptions
{
    ResponseFormat = ChatResponseFormat.JsonObject
};

var response = await chatClient.CompleteChatAsync(messages, options);
```

### Error: "Rate limit exceeded"

**Problem**: Too many requests

**Solution**:
1. Check Azure Portal → Your Resource → Quotas
2. Increase quota or wait
3. Implement retry logic with exponential backoff

---

## 🔒 Security Best Practices

### 1. Never Commit Secrets

Add to `.gitignore`:
```
appsettings.json
appsettings.*.json
*.user
secrets.json
```

### 2. Use Azure Key Vault (Production)

```csharp
builder.Configuration.AddAzureKeyVault(
    new Uri($"https://{keyVaultName}.vault.azure.net/"),
    new DefaultAzureCredential());
```

### 3. Use Managed Identity (Azure Deployment)

```csharp
var client = new AzureOpenAIClient(
    new Uri(endpoint),
    new DefaultAzureCredential());  // No API key needed!
```

### 4. Rotate Keys Regularly

- Azure Portal → Your Resource → Keys and Endpoint
- Regenerate keys every 90 days
- Use Key 1, regenerate Key 2, switch to Key 2, regenerate Key 1

---

## 📊 Monitoring and Logging

### Add Logging to AIService

```csharp
public class AIService : IAIService
{
    private readonly IConfiguration _config;
    private readonly ILogger<AIService> _logger;

    public AIService(IConfiguration config, ILogger<AIService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<EmailResponse> GenerateEmailAsync(EmailRequest request)
    {
        _logger.LogInformation("Generating email for {Recipient}", request.RecipientName);
        
        try
        {
            // ... existing code ...
            
            _logger.LogInformation("Email generated successfully");
            return email;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate email");
            throw;
        }
    }
}
```

### Monitor in Azure Portal

1. Go to your Azure OpenAI resource
2. Click "Metrics" in left menu
3. Monitor:
   - Total Calls
   - Total Tokens
   - Errors
   - Latency

---

## 🚀 Next Steps

### 1. Add Function Calling (Advanced)

Function calling allows AI to call your C# functions:

```csharp
var tools = new List<ChatTool>
{
    ChatTool.CreateFunctionTool(
        functionName: "get_email_template",
        functionDescription: "Gets a pre-defined email template",
        functionParameters: BinaryData.FromString("""
        {
            "type": "object",
            "properties": {
                "template_name": {
                    "type": "string",
                    "description": "Name of the template"
                }
            }
        }
        """))
};

var options = new ChatCompletionOptions();
foreach (var tool in tools)
{
    options.Tools.Add(tool);
}

var response = await chatClient.CompleteChatAsync(messages, options);

// Check if AI wants to call a function
if (response.Value.FinishReason == ChatFinishReason.ToolCalls)
{
    var toolCall = response.Value.ToolCalls[0];
    // Execute your C# function
    // Add result back to conversation
}
```

### 2. Add Streaming Responses

Show email being generated in real-time:

```csharp
await foreach (var update in chatClient.CompleteChatStreamingAsync(messages))
{
    if (update.ContentUpdate.Count > 0)
    {
        var text = update.ContentUpdate[0].Text;
        // Send to frontend via SignalR
    }
}
```

### 3. Add Conversation History

Allow multi-turn conversations:

```csharp
var conversation = new List<ChatMessage>
{
    new SystemChatMessage("..."),
    new UserChatMessage("Generate email about meeting"),
    new AssistantChatMessage(previousResponse),
    new UserChatMessage("Make it more formal")
};
```

---

## ✅ Checklist

Before going live, verify:

- [ ] Azure OpenAI resource created
- [ ] Model deployed (gpt-4 or gpt-35-turbo)
- [ ] Credentials added to configuration
- [ ] API keys not in source control
- [ ] Application tested with real AI
- [ ] Error handling in place
- [ ] Logging configured
- [ ] Monitoring set up in Azure
- [ ] Cost alerts configured
- [ ] Security best practices followed

---

## 📞 Support

### Azure OpenAI Documentation
- https://learn.microsoft.com/en-us/azure/ai-services/openai/

### Azure OpenAI Studio
- https://oai.azure.com

### Pricing Calculator
- https://azure.microsoft.com/en-us/pricing/calculator/

### Azure Support
- https://portal.azure.com → Support + troubleshooting

---

**Version**: 1.0  
**Last Updated**: 2026  
**Status**: Production Ready ✅
