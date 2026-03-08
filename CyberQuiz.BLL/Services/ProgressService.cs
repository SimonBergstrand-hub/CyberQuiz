using CyberQuiz.BLL.DTOs;
using CyberQuiz.BLL.Interfaces;
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

        // Räknar ut procent för en specifik subkategori
        public async Task<double> CalculateScorePercentage(string userId, int subCategoryId)
        {
            var totalQuestions = await _context.Questions
                .CountAsync(q => q.SubCategoryId == subCategoryId);

            if (totalQuestions == 0) return 0;

            // Enklare sätt att räkna rätta svar utan manuell join om relationer finns
            var correctAnswers = await _context.UserResults
                .CountAsync(ur => ur.UserId == userId &&
                                  ur.SubCategoryId == subCategoryId &&
                                  ur.IsCorrect);

            return (double)correctAnswers / totalQuestions * 100;
        }

        // Kollar om användaren har nått 80%
        public async Task<bool> HasPassedSubCategory(string userId, int subCategoryId)
        {
            var percentage = await CalculateScorePercentage(userId, subCategoryId);
            return percentage >= 80;
        }

        // Hämtar alla subkategorier med status
        public async Task<List<SubCategoryDto>> GetSubCategoriesWithStatusAsync(int categoryId, string userId)
        {
            // Hämta allt i ett anrop för att undvika "N+1"-problem i databasen
            var subCategories = await _context.SubCategories
                .Where(sc => sc.CategoryId == categoryId)
                .OrderBy(sc => sc.Order)
                .Include(sc => sc.Questions)
                .ToListAsync();

            // Hämta användarens alla resultat för denna kategori en gång
            var userResults = await _context.UserResults
                .Where(ur => ur.UserId == userId)
                .ToListAsync();

            var result = new List<SubCategoryDto>();
            bool previousLevelCleared = true; // Första nivån är alltid upplåst

            foreach (var sc in subCategories)
            {
                int totalQ = sc.Questions.Count;
                int correctQ = userResults.Count(ur => ur.SubCategoryId == sc.Id && ur.IsCorrect);

                double percentage = totalQ > 0 ? (double)correctQ / totalQ * 100 : 0;

                // Skapa DTO baserat på din tidigare struktur
                var status = new SubCategoryDto
                {
                    Id = sc.Id,           // Ändrat från SubCategoryId till Id för att matcha DTO
                    Name = sc.Name,
                    Order = sc.Order,
                    QuestionCount = totalQ,
                    IsLocked = !previousLevelCleared
                };

                // Uppdatera status inför nästa nivå i loopen
                previousLevelCleared = percentage >= 80;

                result.Add(status);
            }

            return result;
        }
    }
}
