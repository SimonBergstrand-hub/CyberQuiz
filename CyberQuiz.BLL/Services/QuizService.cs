using CyberQuiz.BLL.DTOs;
using CyberQuiz.BLL.Interfaces;
using CyberQuiz.DAL.Quiz;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.BLL.Services
{
    internal class QuizService : IQuizService
    {
        private readonly QuizDbContext _context;
        private readonly IProgressService _progressService;

        public QuizService(QuizDbContext context, IProgressService progressService)
        {
            _context = context;
            _progressService = progressService;
        }

        public async Task<List<QuestionDTO>> GetQuestionsForSubCategory(int subCategoryId)
        {
            var questions = await _context.Questions
                .Where(q => q.SubCategoryId == subCategoryId)
                .Include(q => q.AnswerOptions)
                .ToListAsync();

            return questions.Select(q => new QuestionDTO
            {
                Id = q.Id,
                Text = q.Text,
                optionlist = q.AnswerOptions.Select(a => new AnswerOptionDTO
                {
                    Id = a.Id,
                    Text = a.Text,

                }).ToList()

            }).ToList();
        }

        public async Task<bool> SubmitAnswer(SubmitAnswerDto dto, string userId)
        {

            //Frågan sparas i var question
            var question = await _context.Questions
                .Include(q => q.AnswerOptions)
                .FirstOrDefaultAsync(q => q.Id == dto.QuestinId);

            if (question == null)
                return false;

            //kollar om svaret på frågan är korrekt

            var isCorrect = question.AnswerOptions
                .Any(a => a.Id == dto.AnswerOptionId && a.IsCorrect);

            var result = new UserResult
            {
                UserId = userId,
                IsCorrect = isCorrect,
                QuestionId = question.Id,
                SubCategoryId = question.SubCategoryId
            };

            _context.UserResults.Add(result);
            await _context.SaveChangesAsync();

            if(await _progressService.HasPassedSubCategory(userId, question.SubCategoryId))
            {
                await _progressService.UnlockNextSubCategory(userId, question.SubCategoryId);
            }

            return isCorrect;

        }
    }
}
