using CyberQuiz.UI.ViewModels;
using System.Text.Json;

namespace CyberQuiz.UI.Services;

public class AiCoachService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHttpClientFactory _httpClientFactory;

    public AiCoachService(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<AiCoachViewModel> GetPersonalFeedbackAsync()
    {
        var client = _httpClientFactory.CreateClient("QuizApi");

        var req = new HttpRequestMessage(HttpMethod.Get, "api/ai/coach-feedback");
        if (_httpContextAccessor.HttpContext?.Request.Headers.TryGetValue("Cookie", out var cookie) == true)
        {
            req.Headers.Add("Cookie", (string[])cookie);
        }

        var resp = await client.SendAsync(req);
        if (!resp.IsSuccessStatusCode)
        {
            return new AiCoachViewModel
            {
                Feedback = "Unable to load AI feedback at this time. Please try again later.",
                GeneratedAt = DateTime.Now,
                IsError = true
            };
        }

        var stream = await resp.Content.ReadAsStreamAsync();
        var dto = await JsonSerializer.DeserializeAsync<AiCoachDto>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return new AiCoachViewModel
        {
            Feedback = dto?.Feedback ?? "No feedback available.",
            GeneratedAt = dto?.GeneratedAt ?? DateTime.Now,
            IsError = false
        };
    }
}

public class AiCoachDto
{
    public string Feedback { get; set; } = string.Empty;
    public DateTime GeneratedAt { get; set; }
}