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
}
