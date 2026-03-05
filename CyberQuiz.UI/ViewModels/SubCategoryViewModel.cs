namespace CyberQuiz.UI.ViewModels
{
    public class SubCategoryViewModel
    {
        public int DifficultyIndex { get; set; }
        public string DifficultyName { get; set; } = string.Empty;

        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }

        public bool IsUnlocked { get; set; }

        public double Percentage =>
            TotalQuestions == 0 ? 0 :
            (double)CorrectAnswers / TotalQuestions * 100;
    }
}