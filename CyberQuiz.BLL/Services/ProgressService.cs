using System;
using CyberQuiz.BLL.Interfaces;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.BLL.Services
{
    internal class ProgressService : IProgressService
    {
        //private readonly CyberQuizDbContext _context;

        //räknar ut procent

        public async Task<double> CalculateScorePercentage(string userId, int subCategoryId)
        {
            var totalQuestions = await _context.Questions
                .CountAsync(q => q.SubCategoryId == subCategoryId);

            var correctAnswers = await _context.UserResults
                .Where(r => r.UserId == userId && r.Quetion.SubCategoryId == subCategoryId && r.IsCorrect)
                .CountAsync();

            if (totalQuestions == 0)
                return 0;

            return (double)correctAnswers / totalQuestions * 100;
        }

        //kollar om procent >80

        public async Task<bool> HasPassedSubCategory(string userId, int subCategoryId)
        {
            var percentage = await CalculateScorePercentage(userId, subCategoryId);
            return percentage >= 80;
        }

        // låser upp nästa subkategori(om det finns)
        public async Task UnlockNextSubCategory(string userId, int currentSubCategoryId)
        {
            var current = await _context.SubCategories
                .FirstOrDefaultAsync(s => s.Id == currentSubCategoryId);

            if (current == null) return;

            var next = await _context.SubCategories
                .Where(s => s.CategoryId == current.CategoryId && s.Order > current.Order)
                .OrderBy(s => s.Order)
                .FirstOrDefaultAsync();

            if (next == Null) return;



           
        }
    }

   
}
