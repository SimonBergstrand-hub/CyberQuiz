using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.DAL.Quiz
{
    public class AnswerOption
    {
        public int Id { get; set; }

        public string Text { get; set; } = null!;
        public bool IsCorrect { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
    }
}
