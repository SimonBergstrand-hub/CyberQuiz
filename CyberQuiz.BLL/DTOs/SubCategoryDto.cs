using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.BLL.DTOs
{
    public class SubCategoryDto
    {

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Order { get; set; }
        public int QuestionCount { get; set; }
        public bool IsLocked { get; set; }
    }
}


// Gammal Kod
//public int SubCategoryId { get; set; }
//public string Name { get; set; }
//public bool IsLocked { get; set; }
//public double Percentage { get; set; }