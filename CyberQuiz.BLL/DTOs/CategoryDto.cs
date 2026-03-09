using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.BLL.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int TotalSubCategories { get; set; }
        // Hur många har > 80%
        public int CompletedSubCategories { get; set; } 
    }
}
