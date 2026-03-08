using CyberQuiz.BLL.DTOs;
using CyberQuiz.BLL.Interfaces;
using CyberQuiz.DAL.Data;
using CyberQuiz.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.BLL.Services
{
    public class QuizService : IQuizService
    {
        private readonly QuizDbContext _db;

        public QuizService(QuizDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<QuestionDto>> GetQuestionsForSubCategoryAsync(int subCategoryId)
        {
            return await _db.Questions
                .Include(q => q.AnswerOptions)
                .Where(q => q.SubCategoryId == subCategoryId)
                .Select(q => new QuestionDto
                {
                    Id = q.Id,
                    Text = q.Text,
                    Options = q.AnswerOptions.Select(a => new AnswerOptionDto
                    {
                        Id = a.Id,
                        Text = a.Text
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<SubmitResultDto> ValidateAndSaveAnswerAsync(string userId, int questionId, int answerOptionId)
        {
            var correctOption = await _db.AnswerOptions
                .FirstOrDefaultAsync(a => a.QuestionId == questionId && a.IsCorrect);

            var question = await _db.Questions.FindAsync(questionId);
            if (correctOption == null || question == null) return null!;

            bool isCorrect = correctOption.Id == answerOptionId;

            // Spara resultatet
            var result = new UserResult
            {
                UserId = userId,
                QuestionId = questionId,
                SubCategoryId = question.SubCategoryId,
                IsCorrect = isCorrect
            };

            _db.UserResults.Add(result);
            await _db.SaveChangesAsync();

            return new SubmitResultDto
            {
                IsCorrect = isCorrect,
                CorrectOptionId = correctOption.Id
            };
        }

        public async Task<UserStatsDto> GetUserStatsAsync(string userId)
        {
            var results = await _db.UserResults.Where(ur => ur.UserId == userId).ToListAsync();
            var totalSubCats = await _db.SubCategories.CountAsync();

            return new UserStatsDto
            {
                TotalAnsweredQuestions = results.Count,
                TotalCorrectAnswers = results.Count(r => r.IsCorrect),
                TotalSubCategoriesCount = totalSubCats
                // UnlockedSubCategoriesCount kan läggas till här med liknande logik som i CategoryService
            };
        }

        // Denna behövs för din Controller säkerhetscheck
        public async Task<bool> IsSubCategoryUnlockedAsync(string userId, int subCategoryId)
        {
            // Vi kan återanvända logiken från CategoryService här eller kalla på den
            // För enkelhetens skull implementerar vi en snabbkoll:
            var catService = new CategoryService(_db);
            return await catService.IsSubCategoryUnlockedAsync(userId, subCategoryId);
        }

        public Task<IEnumerable<CategoryDto>> GetCategoriesForUserAsync(string userId)
        {
            var catService = new CategoryService(_db);
            return catService.GetCategoriesWithProgressionAsync(userId);
        }
    }
}
