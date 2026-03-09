using CyberQuiz.UI.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text.Json;

namespace CyberQuiz.UI.Services;

public class QuizService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHttpClientFactory _httpClientFactory;

    public QuizService(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<QuizQuestionViewModel>> GetQuestionsAsync(int subCategoryId)
    {
        var client = _httpClientFactory.CreateClient("QuizApi");
        var req = new HttpRequestMessage(HttpMethod.Get, $"api/quiz/subcategories/{subCategoryId}/questions");
        if (_httpContextAccessor.HttpContext?.Request.Headers.TryGetValue("Cookie", out var cookie) == true)
        {
            req.Headers.Add("Cookie", (string[])cookie);
        }
        var resp = await client.SendAsync(req);

        if (!resp.IsSuccessStatusCode)
        {
            try { Console.WriteLine($"[QuizService] API request failed: {req.RequestUri} -> {(int)resp.StatusCode} {resp.ReasonPhrase}"); } catch { }
            try
            {
                var err = await resp.Content.ReadAsStringAsync();
                Console.WriteLine($"[QuizService] Response body: {err}");
            }
            catch { }

            return new List<QuizQuestionViewModel>();
        }

        var stream = await resp.Content.ReadAsStreamAsync();

        List<ApiQuestionDto>? dtos = null;
        try
        {
            dtos = await JsonSerializer.DeserializeAsync<List<ApiQuestionDto>>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception ex)
        {
            try { Console.WriteLine($"[QuizService] Deserialization failed: {ex.Message}"); } catch { }
            return new List<QuizQuestionViewModel>();
        }

        var result = new List<QuizQuestionViewModel>();
        if (dtos == null)
        {
            try { Console.WriteLine($"[QuizService] No questions returned for subcategory {subCategoryId}"); } catch { }
            return result;
        }

        try { Console.WriteLine($"[QuizService] Received {dtos.Count} questions for subcategory {subCategoryId}"); } catch { }

        foreach (var q in dtos)
        {
            var vm = new QuizQuestionViewModel
            {
                Id = q.Id,
                Text = q.Text,
                Options = q.Options.Select(o => new CyberQuiz.UI.ViewModels.AnswerOptionViewModel { Id = o.Id, Text = o.Text }).ToList()
            };

            result.Add(vm);
        }

        return result;
    }
}
