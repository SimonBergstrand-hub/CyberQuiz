namespace CyberQuiz.UI.ViewModels
{
    public class QuizQuestionViewModel
    {
        public int QuestionId { get; set; }

        public string QuestionText { get; set; } = "";

        public List<string> Answers { get; set; } = new();

        public int CorrectAnswerIndex { get; set; }

        public int? SelectedAnswerIndex { get; set; }

        public bool IsAnswered => SelectedAnswerIndex != null;

        public bool IsCorrect =>
            SelectedAnswerIndex == CorrectAnswerIndex;
    }
}