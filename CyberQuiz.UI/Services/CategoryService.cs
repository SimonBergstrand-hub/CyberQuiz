using CyberQuiz.UI.ViewModels;
using CyberQuiz.UI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace CyberQuiz.Services
{
    public class CategoryService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SubCategoryService _subCategoryService;

        public CategoryService(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            SubCategoryService subCategoryService)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _subCategoryService = subCategoryService;
        }

        public async Task<List<CategoryProgressViewModel>> GetUserCategoryProgressAsync()
        {
            var user = await _userManager.GetUserAsync(
                _httpContextAccessor.HttpContext!.User);

            if (user == null)
                return new List<CategoryProgressViewModel>();

            var categories = new List<CategoryProgressViewModel>
            {
                new() { CategoryId = 1, Name="Network Security", Description="Core fundamentals" },
                new() { CategoryId = 2, Name="Cryptography", Description="Encryption & hashing" },
                new() { CategoryId = 3, Name="Web Security", Description="OWASP & attacks" },
                new() { CategoryId = 4, Name="Malware Analysis", Description="Reverse engineering basics" }
            };

            foreach (var category in categories)
            {
                var subcategories =
                    await _subCategoryService.GetSubCategoriesAsync(category.CategoryId);

                category.TotalQuestions = subcategories.Sum(s => s.TotalQuestions);
                category.CorrectAnswers = subcategories.Sum(s => s.CorrectAnswers);
                category.SubCategories = subcategories;
            }

            return categories;
        }
    }
}