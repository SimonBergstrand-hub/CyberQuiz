namespace CyberQuiz.UI.ViewModels
{
    public class QuizQuestionViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; } = "";

        public List<AnswerOptionViewModel> Options { get; set; } = new();

        public int? SelectedOptionId { get; set; }

        public int? CorrectOptionId { get; set; }

        public bool IsAnswered => SelectedOptionId != null;
    }
}