using System.Collections.Generic;

namespace CyberQuiz.UI.Services
{
    public class ApiCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int TotalSubCategories { get; set; }
        public int CompletedSubCategories { get; set; }
    }

    public class ApiSubCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Order { get; set; }
        public int QuestionCount { get; set; }
        public bool IsLocked { get; set; }
    }

    public class ApiQuestionDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public List<ApiAnswerOptionDto> Options { get; set; } = new();
    }

    public class ApiAnswerOptionDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
    }

    // Per-user stats for a subcategory (returned by profile/progression endpoint)
    public class ApiSubCategoryStatsDto
    {
        public int SubCategoryId { get; set; }
        public int CorrectAnswers { get; set; }
        public int TotalQuestions { get; set; }
        public double Percentage { get; set; }
    }
}
