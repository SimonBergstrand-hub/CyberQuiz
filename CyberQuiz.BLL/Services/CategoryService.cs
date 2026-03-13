using CyberQuiz.BLL.DTOs;
using CyberQuiz.BLL.Interfaces;
using CyberQuiz.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace CyberQuiz.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly QuizDbContext _db;

        public CategoryService(QuizDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesWithProgressionAsync(string userId)
        {
            var categories = await _db.Categories
                .Include(c => c.SubCategories)
                .ThenInclude(sc => sc.Questions)
                .ToListAsync();

            var userResults = await _db.UserResults
                .Where(ur => ur.UserId == userId)
                .ToListAsync();

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                TotalSubCategories = c.SubCategories.Count,
                // Räknar hur många subkategorier som har >= 80% rätt
                CompletedSubCategories = c.SubCategories.Count(sc => IsLevelCleared(sc, userResults))
            });
        }

        public async Task<IEnumerable<SubCategoryDto>> GetSubCategoriesWithLockStatusAsync(int categoryId, string userId)
        {
            var category = await _db.Categories
                .Include(c => c.SubCategories.OrderBy(sc => sc.Order))
                .ThenInclude(sc => sc.Questions)
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null) return null!;

            var userResults = await _db.UserResults.Where(ur => ur.UserId == userId).ToListAsync();
            var dtos = new List<SubCategoryDto>();
            bool previousLevelCleared = true; // Första nivån (Order 1) är alltid upplåst

            foreach (var sc in category.SubCategories)
            {
                dtos.Add(new SubCategoryDto
                {
                    Id = sc.Id,
                    Name = sc.Name,
                    Order = sc.Order,
                    QuestionCount = sc.Questions.Count,
                    IsLocked = !previousLevelCleared
                });

                // Kolla om användaren klarat denna nivå för att låsa upp nästa i nästa loop-varv
                previousLevelCleared = IsLevelCleared(sc, userResults);
            }

            return dtos;
        }

        public async Task<bool> IsSubCategoryUnlockedAsync(string userId, int subCategoryId)
        {
            var subCat = await _db.SubCategories.FindAsync(subCategoryId);
            if (subCat == null) return false;
            if (subCat.Order == 1) return true; // Easy är alltid öppen

            // Hitta nivån precis innan i samma kategori
            var previousSubCat = await _db.SubCategories
                .Include(sc => sc.Questions)
                .Where(sc => sc.CategoryId == subCat.CategoryId && sc.Order == subCat.Order - 1)
                .FirstOrDefaultAsync();

            if (previousSubCat == null) return false;

            var results = await _db.UserResults
                .Where(ur => ur.UserId == userId && ur.SubCategoryId == previousSubCat.Id)
                .ToListAsync();

            return IsLevelCleared(previousSubCat, results);
        }

        public async Task<IEnumerable<SubCategoryStatsDto>> GetSubCategoryStatsAsync(int categoryId, string userId)
        {
            var category = await _db.Categories
                .Include(c => c.SubCategories.OrderBy(sc => sc.Order))
                .ThenInclude(sc => sc.Questions)
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null)
                return Enumerable.Empty<SubCategoryStatsDto>();

            var userResults = await _db.UserResults
                .Where(ur => ur.UserId == userId)
                .ToListAsync();

            var stats = new List<SubCategoryStatsDto>();

            foreach (var sc in category.SubCategories)
            {
                int totalQuestions = sc.Questions.Count;

                // Count unique questions that were answered correctly (not total attempts)
                int correctAnswers = userResults
                    .Where(r => r.SubCategoryId == sc.Id && r.IsCorrect)
                    .Select(r => r.QuestionId)
                    .Distinct()
                    .Count();

                double percentage = totalQuestions == 0
                    ? 0
                    : (double)correctAnswers / totalQuestions * 100;

                stats.Add(new SubCategoryStatsDto
                {
                    SubCategoryId = sc.Id,
                    CorrectAnswers = correctAnswers,
                    TotalQuestions = totalQuestions,
                    Percentage = percentage
                });
            }

            return stats;
        }

        // Hjälpmetod för att räkna ut 80%-spärren
        private bool IsLevelCleared(DAL.Models.SubCategory sc, List<DAL.Models.UserResult> results)
        {
            int totalQuestions = sc.Questions.Count;
            if (totalQuestions == 0) return false;

            int correctAnswers = results
                .Where(r => r.SubCategoryId == sc.Id && r.IsCorrect)
                .Select(r => r.QuestionId)
                .Distinct()
                .Count();

            return (double)correctAnswers / totalQuestions >= 0.6;
        }
    }
}
