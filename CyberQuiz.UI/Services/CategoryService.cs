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

        public CategoryService(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<CategoryProgressViewModel>> GetUserCategoryProgressAsync()
        {
            var user = await _userManager.GetUserAsync(
                _httpContextAccessor.HttpContext!.User);

            if (user == null)
                return new List<CategoryProgressViewModel>();

            var categories = new List<CategoryProgressViewModel>
            {
                new() { CategoryId = 1, Name="Network Security", Description="Core fundamentals", CorrectAnswers=9, TotalQuestions=10 },
                new() { CategoryId = 2, Name="Cryptography", Description="Encryption & hashing", CorrectAnswers=8, TotalQuestions=10 },
                new() { CategoryId = 3, Name="Web Security", Description="OWASP & attacks", CorrectAnswers=3, TotalQuestions=10 },
                new() { CategoryId = 4, Name="Malware Analysis", Description="Reverse engineering basics", CorrectAnswers=0, TotalQuestions=10 }
            };

            for (int i = 0; i < categories.Count; i++)
            {
                categories[i].IsUnlocked =
                    i == 0 || categories[i - 1].Percentage >= 80;
            }

            return categories;
        }
    }
}