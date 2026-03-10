using CyberQuiz.BLL.Interfaces;
using CyberQuiz.BLL.DTOs;
using CyberQuiz.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace CyberQuiz.BLL.Services
{
    public class ProgressService : IProgressService
    {
        private readonly QuizDbContext _context;

        public ProgressService(QuizDbContext context)
        {
            _context = context;
        }

        // Räknar ut procent
        public async Task<double> CalculateScorePercentage(string userId, int subCategoryId)
        {
            var totalQuestions = await _context.Questions
                .CountAsync(q => q.SubCategoryId == subCategoryId);

            var correctAnswers = await (from ur in _context.UserResults
                                        join q in _context.Questions
                                            on ur.QuestionId equals q.Id
                                        where ur.UserId == userId &&
                                              q.SubCategoryId == subCategoryId &&
                                              ur.IsCorrect
                                        select ur)
                                        .CountAsync();

            if (totalQuestions == 0)
                return 0;

            return (double)correctAnswers / totalQuestions * 100;
        }

        // Kollar om procent >80
        public async Task<bool> HasPassedSubCategory(string userId, int subCategoryId)
        {
            var percentage = await CalculateScorePercentage(userId, subCategoryId);
            return percentage >= 80;
        }

        // Kollar status på subkategorier
        public async Task<List<SubCategoryDto>> GetSubCategoriesWithStatusAsync(int categoryId, string userId)
        {
            var subCategories = await _context.SubCategories
                .Include(sc => sc.Questions)
                .Where(sc => sc.CategoryId == categoryId)
                .OrderBy(sc => sc.Order)
                .ToListAsync();

            var result = new List<SubCategoryDto>();
            bool previousLevelCleared = true;

            foreach (var sc in subCategories)
            {
                var percentage = await CalculateScorePercentage(userId, sc.Id);

                var status = new SubCategoryDto
                {
                    Id = sc.Id,
                    Name = sc.Name,
                    Order = sc.Order,
                    QuestionCount = sc.Questions.Count,
                    IsLocked = !previousLevelCleared
                };

                previousLevelCleared = percentage >= 80;

                result.Add(status);
            }

            return result;
        }
    }
}