using System;
using CyberQuiz.BLL.Interfaces;
using CyberQuiz.DAL;

using System.Collections.Generic;
using System.Text;
using CyberQuiz.DAL.Quiz;
using Microsoft.EntityFrameworkCore;
using CyberQuiz.BLL.DTOs;

namespace CyberQuiz.BLL.Services
{
    internal class ProgressService : IProgressService
    {
        private readonly QuizDbContext _context;

        public ProgressService(QuizDbContext context)
        {
            _context = context;
        }

        
        

        //räknar ut procent
        
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

        //kollar om procent >80

        public async Task<bool> HasPassedSubCategory(string userId, int subCategoryId)
        {
            var percentage = await CalculateScorePercentage(userId, subCategoryId);
            return percentage >= 80;
        }

        // låser upp nästa subkategori(om det finns)
        public async Task<List<SubCategoryStatusDto>> GetSubCategoriesWithStatusAsync(int categoryId, string userId)
        {
            var subCategories = await _context.SubCategories
                .Where(sc => sc.CategoryId == categoryId)
                .OrderBy(sc => sc.Order)
                .ToListAsync();

            var result = new List<SubCategoryStatusDto>();

            bool previousLevelCleared = true;

            foreach (var sc in subCategories)
            {
                var percentage = await CalculateScorePercentage(userId, sc.Id);

                var status = new SubCategoryStatusDto
                {
                    SubCategoryId = sc.Id,
                    Name = sc.Name,
                    Percentage = percentage,
                    IsLocked = !previousLevelCleared
                };

                previousLevelCleared = percentage >= 80;

                result.Add(status);
            }

            return result;
        }
    }

   
}
