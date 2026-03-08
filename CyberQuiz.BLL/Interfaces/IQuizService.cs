using CyberQuiz.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.BLL.Interfaces
{
    public interface IQuizService
    {
        // För GetCategories i controllern
        Task<IEnumerable<CategoryDto>> GetCategoriesForUserAsync(string userId);

        // För GetQuestions i controllern
        Task<IEnumerable<QuestionDto>> GetQuestionsForSubCategoryAsync(int subCategoryId);

        // För säkerhetskontrollen i controllern
        Task<bool> IsSubCategoryUnlockedAsync(string userId, int subCategoryId);

        // För SubmitAnswer i controllern (returnerar ett objekt med IsCorrect och CorrectOptionId)
        Task<SubmitResultDto> ValidateAndSaveAnswerAsync(string userId, int questionId, int answerOptionId);

        Task<UserStatsDto> GetUserStatsAsync(string userId);
    }
}
