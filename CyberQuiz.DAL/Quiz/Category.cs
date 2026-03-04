using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.DAL.Quiz
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }
            = new List<SubCategory>();
    }
}
