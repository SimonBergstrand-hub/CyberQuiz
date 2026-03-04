using CyberQuiz.UI.ViewModels;

namespace CyberQuiz.Services
{
    public class CategoryService
    {
        public Task<List<CategoryProgressViewModel>> GetUserCategoryProgressAsync()
        {
            // For now, dummy data
            return Task.FromResult(new List<CategoryProgressViewModel>
            {
                new() { CategoryId = 1, Name="Basics", CorrectAnswers=9, TotalQuestions=10 },
                new() { CategoryId = 2, Name="Advanced", CorrectAnswers=8, TotalQuestions=10 },
                new() { CategoryId = 3, Name="Expert", CorrectAnswers=3, TotalQuestions=10 },
                new() { CategoryId = 4, Name="Master", CorrectAnswers=0, TotalQuestions=10 }
            });
        }
    }
}