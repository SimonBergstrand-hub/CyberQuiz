using CyberQuiz.UI.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CyberQuiz.UI.Services;

public class SubCategoryService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHttpClientFactory _httpClientFactory;

    public SubCategoryService(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
    {
        _httpContextAccessor = httpContextAccessor;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<SubCategoryViewModel>> GetSubCategoriesAsync(int categoryId)
    {
        var client = _httpClientFactory.CreateClient("QuizApi");

        var req = new HttpRequestMessage(HttpMethod.Get, $"api/categories/{categoryId}/subcategories");

        // forward cookies so API receives authentication
        if (_httpContextAccessor.HttpContext?.Request.Headers.TryGetValue("Cookie", out var cookie) == true)
        {
            req.Headers.Add("Cookie", (string[])cookie);
        }

        var resp = await client.SendAsync(req);
        if (!resp.IsSuccessStatusCode)
        {
            return new List<SubCategoryViewModel>();
        }

        var stream = await resp.Content.ReadAsStreamAsync();

        List<ApiSubCategoryDto>? dtos = null;
        try
        {
            dtos = await JsonSerializer.DeserializeAsync<List<ApiSubCategoryDto>>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch
        {
            return new List<SubCategoryViewModel>();
        }

        var result = new List<SubCategoryViewModel>();
        if (dtos == null)
        {
            return result;
        }

        foreach (var d in dtos)
        {
            result.Add(new SubCategoryViewModel
            {
                Id = d.Id,
                Name = d.Name,
                Order = d.Order,
                QuestionCount = d.QuestionCount,
                IsLocked = d.IsLocked,
                DifficultyIndex = Math.Max(0, d.Order - 1),
                DifficultyName = d.Name,
                CorrectAnswers = 0 // API currently does not return per-user correct count here
            });
        }

        // Try to fetch per-user stats (if API supports it) and merge
        try
        {
            var statsReq = new HttpRequestMessage(HttpMethod.Get, $"api/profile/subcategories/{categoryId}");
            if (_httpContextAccessor.HttpContext?.Request.Headers.TryGetValue("Cookie", out var cookie2) == true)
            {
                statsReq.Headers.Add("Cookie", (string[])cookie2);
            }

            var statsResp = await client.SendAsync(statsReq);
            if (statsResp.IsSuccessStatusCode)
            {
                var statsStream = await statsResp.Content.ReadAsStreamAsync();
                var stats = await JsonSerializer.DeserializeAsync<List<ApiSubCategoryStatsDto>>(statsStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (stats != null)
                {
                    foreach (var s in stats)
                    {
                        var sub = result.FirstOrDefault(x => x.Id == s.SubCategoryId);
                        if (sub != null)
                        {
                            sub.CorrectAnswers = s.CorrectAnswers;
                            sub.TotalQuestions = s.TotalQuestions;
                        }
                    }
                }
            }
        }
        catch
        {
            // Silently ignore if stats endpoint not available yet
        }

        // Recalculate unlocks using the merged data
        for (int i = 0; i < result.Count; i++)
        {
            result[i].IsUnlocked = i == 0 || result[i - 1].Percentage >= 80;
        }

        return result;
    }
}