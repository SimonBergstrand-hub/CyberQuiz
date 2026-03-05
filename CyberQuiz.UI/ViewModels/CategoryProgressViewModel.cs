namespace CyberQuiz.UI.ViewModels
{

    // DUMMY VIEWMODEL UNTIL WE FIX THE DATABASE
    public class CategoryProgressViewModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }

        // Per-category subcategories so UI can show per-subcategory progress bars
        public List<SubCategoryViewModel> SubCategories { get; set; } = new();

        public double Percentage =>
            TotalQuestions == 0 ? 0 :
            (double)CorrectAnswers / TotalQuestions * 100;
    }
}
