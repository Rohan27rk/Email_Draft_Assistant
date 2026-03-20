using Email_Assistant.Infrastructure;
using Email_Assistant.Models;
using Email_Assistant.Services;

namespace Email_Assistant
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();
            
            // Choose AI Service Implementation
            // Option 1: Azure OpenAI Function Calling (Default)
            builder.Services.AddScoped<AIService>();
            
            // Register EmailHistoryService as singleton (persists for application lifetime)
            builder.Services.AddSingleton<EmailHistoryService>();

            // Configure CORS for frontend communication
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazorUI", policy =>
                {
                    policy.WithOrigins("http://localhost:5010", "http://localhost:5219", "http://localhost:5000", "https://localhost:7022")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Enable CORS
            app.UseCors("AllowBlazorUI");

            app.UseAuthorization();


            app.MapPost("/generate-email",
async (
    EmailRequest request,
    AIService aiService,
    EmailHistoryService historyService,
    ILogger<Program> logger) =>
{
    try
    {
        logger.LogInformation("Received email generation request for recipient: {RecipientName}, KeyPoints: {KeyPoints}", 
            request.RecipientName, request.KeyPoints);
        
        var response = await aiService.GenerateEmailAsync(request);
        
        // Store in history
        historyService.AddEntry(request, response);

        logger.LogInformation("Successfully generated email with subject: {Subject}", response.Subject);
        return Results.Ok(response);   // return object, not string
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error generating email: {Message}", ex.Message);
        Console.WriteLine($"DEBUG: Backend exception - {ex.Message}");
        // Return plain text error message (not JSON)
        return Results.BadRequest(ex.Message);
    }
});

            app.MapGet("/email-history",
                (EmailHistoryService historyService) =>
                {
                    var history = historyService.GetAllEntries();
                    return Results.Ok(history);
                });



            app.Run();
        }
    }
}
