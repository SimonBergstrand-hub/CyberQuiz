using CyberQuiz.UI.ViewModels;
using CyberQuiz.UI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace CyberQuiz.Services
{
    public class SubCategoryService
    {
        // Event raised when progress changes (for example when a quiz result is saved)
        public event Action? ResultSaved;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly QuizProgressService _progressService;

        private readonly string[] _difficulties =
            { "Beginner", "Intermediate", "Advanced", "Expert" };

        public SubCategoryService(
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            QuizProgressService progressService)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _progressService = progressService;

            // Forward progress service notifications so consumers of SubCategoryService
            // don't need to know about the underlying progress storage implementation.
            _progressService.ResultSaved += () => ResultSaved?.Invoke();
        }

        public async Task<List<SubCategoryViewModel>> GetSubCategoriesAsync(int categoryId)
        {
            var user = await _userManager.GetUserAsync(
                _httpContextAccessor.HttpContext!.User);

            if (user == null)
                return new List<SubCategoryViewModel>();

            // Deterministic seed per user + category
            var seed = HashCode.Combine(user.Id, categoryId);
            var random = new Random(seed);

            var result = new List<SubCategoryViewModel>();

            int totalQuestions = 10;

            int beginnerScore = random.Next(0, 11);
            int previousScore = beginnerScore;

            for (int i = 0; i < _difficulties.Length; i++)
            {
                int correctAnswers;

                if (i == 0)
                {
                    correctAnswers = beginnerScore;
                }
                else
                {
                    if (previousScore >= 8)
                    {
                        correctAnswers = Math.Max(
                            0,
                            previousScore - random.Next(1, 4)
                        );
                    }
                    else
                    {
                        correctAnswers = 0;
                    }
                }

                result.Add(new SubCategoryViewModel
                {
                    DifficultyIndex = i,
                    DifficultyName = _difficulties[i],
                    TotalQuestions = totalQuestions,
                    CorrectAnswers = correctAnswers
                });

                previousScore = correctAnswers;
            }

            // If we have saved results in the (dummy) database, use them to override
            // the generated deterministic values so progress is preserved across quizzes.
            for (int i = 0; i < result.Count; i++)
            {
                var saved = await _progressService.GetResultAsync(categoryId, i);
                if (saved != null)
                {
                    // Apply saved result
                    result[i].CorrectAnswers = saved.Score;
                    result[i].TotalQuestions = saved.TotalQuestions;

                    try
                    {
                        Console.WriteLine($"[SubCategoryService] Applied saved result for Category={categoryId} Difficulty={i} Score={saved.Score} CompletedAt={saved.CompletedAt}");
                    }
                    catch { }
                }
            }

            // Unlock logic
            for (int i = 0; i < result.Count; i++)
            {
                result[i].IsUnlocked =
                    i == 0 || result[i - 1].Percentage >= 80;
            }

            return result;
        }
    }
}