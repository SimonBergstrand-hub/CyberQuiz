using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.BLL.DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public List<AnswerOptionDto> Options { get; set; } = new();
    }
}
