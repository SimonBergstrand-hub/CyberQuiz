using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.BLL.DTOs
{
    internal class UserProgressDto
    {
        public int SubCategoryId { get; set; }
        public double Percentage { get; set; }

        public bool IsUnlocked { get; set; }
    }
}
