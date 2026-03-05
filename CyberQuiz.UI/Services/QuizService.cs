using CyberQuiz.UI.ViewModels;

namespace CyberQuiz.Services
{
    public class QuizService
    {
        public Task<List<QuizQuestionViewModel>> GetQuizQuestions(
            int categoryId,
            int difficultyIndex)
        {
            var questions = new List<QuizQuestionViewModel>();

            for (int i = 1; i <= 10; i++)
            {
                questions.Add(new QuizQuestionViewModel
                {
                    QuestionId = i,
                    QuestionText = $"Question {i} for category {categoryId}",
                    Answers = new List<string>
                    {
                        "Answer A",
                        "Answer B",
                        "Answer C",
                        "Answer D"
                    },
                    //CorrectAnswerIndex = Random.Shared.Next(0, 4)
                    CorrectAnswerIndex = 0
                });
            }

            return Task.FromResult(questions);
        }
    }
}