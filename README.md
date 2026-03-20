# Email Assistant

A professional email drafting application built with ASP.NET Core and Blazor Server, powered by Azure OpenAI with true function calling capabilities.

## Features

- **AI-Powered Email Generation**: Uses Azure OpenAI with function calling for structured email creation
- **ChatGPT-Style Interface**: Modern dark theme UI with sidebar history
- **Real-time Validation**: Client-side validation with immediate feedback
- **Email History**: Persistent storage of generated emails during session
- **Responsive Design**: Works on desktop and mobile devices
- **Strict AI Boundaries**: AI only assists with email-related tasks

## Architecture

### Backend (Email_Assistant)
- **ASP.NET Core 8.0** Web API
- **Azure OpenAI Integration** with true function calling
- **Minimal APIs** for clean, lightweight endpoints
- **External Prompt Management** for easy customization

### Frontend (EmailAssistant.UI)
- **Blazor Server** for interactive UI
- **Bootstrap 5** with custom dark theme
- **Real-time form validation**
- **Responsive sidebar with history**

## Quick Start

### Prerequisites
- .NET 8.0 SDK
- Azure OpenAI resource with GPT-4 deployment

### Configuration

1. Update `appsettings.json` with your Azure OpenAI credentials:
```json
{
  "AzureOpenAI": {
    "Endpoint": "https://your-resource.openai.azure.com/",
    "ApiKey": "your-api-key-here",
    "DeploymentName": "your-gpt-4-deployment"
  }
}
```

### Running the Application

1. **Start the Backend**:
```bash
dotnet run --project Email_Assistant.csproj
```
Backend will run on `http://localhost:5045`

2. **Start the Frontend**:
```bash
dotnet run --project EmailAssistant.UI/EmailAssistant.UI.csproj
```
Frontend will run on `http://localhost:5010`

3. **Access the Application**:
Open your browser to `http://localhost:5010`

## Troubleshooting

### "Unable to connect to server" Error

If you see this error, check the following:

1. **Backend Running**: Ensure the backend is running on `http://localhost:5045`
   ```bash
   # Check if backend is running by visiting Swagger UI
   # Open http://localhost:5045/swagger in your browser
   ```

2. **Port Conflicts**: If port 5045 is in use, update both:
   - `Properties/launchSettings.json` (backend)
   - `EmailAssistant.UI/Program.cs` (frontend HttpClient configuration)

3. **Firewall/Antivirus**: Temporarily disable to test connectivity

4. **HTTPS Issues**: The app is configured for HTTP-only communication. If you see HTTPS errors:
   - Ensure both apps run with `http` profile: `dotnet run --launch-profile http`
   - Check that HTTPS redirection is disabled in development

5. **CORS Issues**: Check browser console for CORS errors. The backend allows:
   - `http://localhost:5010` (frontend)
   - `http://localhost:5219` (alternative)
   - `http://localhost:5000` (alternative)
   - `https://localhost:7022` (HTTPS frontend)

### Testing Backend Connectivity

1. **Swagger UI**: Visit `http://localhost:5045/swagger`
2. **Test API**: Use curl or Postman:
   ```bash
   curl -X POST http://localhost:5045/generate-email \
     -H "Content-Type: application/json" \
     -d '{"recipientName":"Test","tone":"Professional","emailType":"FollowUp","keyPoints":"Test message"}'
   ```

### Common Solutions

- **Restart both applications** in the correct order (backend first, then frontend)
- **Clear browser cache** and cookies
- **Check Windows Defender/Firewall** settings
- **Run as Administrator** if permission issues occur
- **Use different ports** if conflicts exist

## Project Structure

```
Email_Assistant/
├── Infrastructure/          # AI services and core logic
├── Models/                 # Data models
├── Services/               # Business services
├── Prompts/               # AI prompt templates
├── EmailAssistant.UI/     # Blazor frontend
│   ├── Components/        # Razor components
│   ├── Models/           # Frontend models
│   ├── Services/         # API communication
│   └── wwwroot/          # Static assets
└── README.md
```

## Key Components

### AI Service (`Infrastructure/AIService.cs`)
- Implements true Azure OpenAI function calling
- Structured email generation with proper components
- Error handling and fallback mechanisms
- Non-email request rejection

### Email Generator (`EmailAssistant.UI/Components/Pages/EmailGenerator.razor`)
- ChatGPT-style interface
- Real-time validation and character counting
- Email history sidebar with toggle
- Edit/regenerate functionality

### Prompt Management (`Prompts/`)
- `SystemPrompt.txt`: AI behavior and restrictions
- `UserPromptTemplate.txt`: User request formatting
- Easy editing without code changes

## Email Types Supported

- **Follow-up emails**
- **Leave requests**
- **Meeting requests**
- **Custom email types** (easily extensible)

## Tone Options

- **Professional**: Standard business communication
- **Friendly**: Warm but professional
- **Formal**: Traditional formal business style
- **Informal**: Casual but appropriate

## Development

### Building
```bash
# Backend
dotnet build Email_Assistant.csproj

# Frontend
dotnet build EmailAssistant.UI/EmailAssistant.UI.csproj
```

### Testing
The application includes comprehensive error handling and validation at multiple levels:
- Client-side form validation
- Server-side model validation
- AI response validation
- Azure OpenAI error handling

## Customization

### Modifying AI Behavior
Edit `Prompts/SystemPrompt.txt` to change AI behavior and restrictions.

### Adding Email Types
Update the enum options in the frontend form and corresponding logic in `AIService.cs`.

### Styling
Modify `EmailAssistant.UI/wwwroot/css/custom.css` for UI customization.

## License

This project is for educational and demonstration purposes.