using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.BLL.DTOs
{
    public class SubCategoryStatsDto
    {
        public int SubCategoryId { get; set; }

        public int CorrectAnswers { get; set; }

        public int TotalQuestions { get; set; }

        public double Percentage { get; set; }
    }
}
