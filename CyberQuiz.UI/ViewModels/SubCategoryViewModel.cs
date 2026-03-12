namespace CyberQuiz.UI.ViewModels
{
    public class SubCategoryViewModel
    {
        // API fields
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Order { get; set; }
        public int QuestionCount { get; set; }
        public bool IsLocked { get; set; }

        // UI-friendly fields (kept for compatibility with existing components)
        public int DifficultyIndex { get; set; }
        public string DifficultyName { get; set; } = string.Empty;
        public int TotalQuestions
        {
            get => QuestionCount;
            set => QuestionCount = value;
        }

        public int CorrectAnswers { get; set; }

        public bool IsUnlocked
        {
            get => !IsLocked;
            set => IsLocked = !value;
        }

        public double Percentage =>
            TotalQuestions == 0 ? 0 : Math.Min(100.0, (double)CorrectAnswers / TotalQuestions * 100);
    }
}