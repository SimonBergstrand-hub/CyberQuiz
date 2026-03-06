using CyberQuiz.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.BLL.Interfaces
{
    public interface IQuizService
    {

        Task<bool> SubmitAnswer(SubmitAnswerDto dto, string userId);

        Task<List<QuestionDTO>> GetQuestionsForSubCategory(int subcategoryId);

    }
}
