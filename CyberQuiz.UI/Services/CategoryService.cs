using CyberQuiz.UI.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text.Json;

namespace CyberQuiz.UI.Services;

public class CategoryService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly SubCategoryService _subCategoryService;

    public CategoryService(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory, SubCategoryService subCategoryService)
    {
        _httpContextAccessor = httpContextAccessor;
        _httpClientFactory = httpClientFactory;
        _subCategoryService = subCategoryService;
    }

    public async Task<List<CategoryProgressViewModel>> GetCategoriesAsync()
    {
        var client = _httpClientFactory.CreateClient("QuizApi");

        var req = new HttpRequestMessage(HttpMethod.Get, "api/quiz/categories");
        if (_httpContextAccessor.HttpContext?.Request.Headers.TryGetValue("Cookie", out var cookie) == true)
        {
            req.Headers.Add("Cookie", (string[])cookie);
        }

        var resp = await client.SendAsync(req);
        if (!resp.IsSuccessStatusCode) return new();

        var stream = await resp.Content.ReadAsStreamAsync();
        var dtos = await JsonSerializer.DeserializeAsync<List<ApiCategoryDto>>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var result = new List<CategoryProgressViewModel>();
        if (dtos == null) return result;

        foreach (var dto in dtos)
        {
            var vm = new CategoryProgressViewModel
            {
                CategoryId = dto.Id,
                Name = dto.Name,
                Description = dto.Description ?? string.Empty
            };

            // populate subcategories via SubCategoryService
            vm.SubCategories = await _subCategoryService.GetSubCategoriesAsync(vm.CategoryId);
            vm.TotalQuestions = vm.SubCategories.Sum(s => s.TotalQuestions);
            vm.CorrectAnswers = vm.SubCategories.Sum(s => s.CorrectAnswers);

            result.Add(vm);
        }

        return result;
    }
}
