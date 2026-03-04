using CyberQuiz.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.BLL.Interfaces
{
    public interface IQuizService
    {

        Task<bool> SubmitAnswer(int userId, int questionId, int answerOptionId);

        //Task<List<QuestionDTO>> GetQuestionsForSubcategory(int subcategoryId);

    }
}
