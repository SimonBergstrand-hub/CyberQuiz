using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.DAL.Quiz
{
    public class SubCategory
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public int Order { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        public ICollection<Question> Questions { get; set; }
            = new List<Question>();
    }
}
