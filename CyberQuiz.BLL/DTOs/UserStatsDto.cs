using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.BLL.DTOs
{
    public class UserStatsDto
    {
        public int TotalCorrectAnswers { get; set; }
        public int TotalAnsweredQuestions { get; set; }

        // Räknar ut procentandelen
        public double SuccessRate => TotalAnsweredQuestions > 0
            ? (double)TotalCorrectAnswers / TotalAnsweredQuestions
            : 0;

        public int UnlockedSubCategoriesCount { get; set; }
        public int TotalSubCategoriesCount { get; set; }
    }
}
