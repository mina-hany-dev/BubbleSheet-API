using BubleSheet.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class EmailService : IEmailService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _senderEmail;
    private readonly string _senderName;

    public EmailService(IConfiguration configuration, HttpClient httpClient)
    {
        _httpClient = httpClient;

        _httpClient.BaseAddress = new Uri("https://api.brevo.com/v3/");

        _apiKey = configuration["BrevoSettings:ApiKey"]
                  ?? throw new ArgumentNullException("Brevo ApiKey missing");

        _senderEmail = configuration["BrevoSettings:SenderEmail"]
                       ?? throw new ArgumentNullException("SenderEmail missing");

        _senderName = configuration["BrevoSettings:SenderName"]
                      ?? "Ahmed Platform";

        _httpClient.DefaultRequestHeaders.Add("api-key", _apiKey);

        _httpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var emailData = new
        {
            sender = new
            {
                name = _senderName,
                email = _senderEmail
            },
            to = new[]
            {
                new { email = toEmail }
            },
            subject = subject,
            htmlContent = body
        };

        var json = JsonSerializer.Serialize(emailData);

        var response = await _httpClient.PostAsync(
            "smtp/email",
            new StringContent(json, Encoding.UTF8, "application/json")
        );

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new ApplicationException($"Brevo API Error: {error}");
        }
    }
}