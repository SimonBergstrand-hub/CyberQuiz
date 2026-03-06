using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.BLL.DTOs
{
    public class SubmitAnswerDto
    {
        public int QuestionId { get; set; }
        public int AnswerOptionId { get; set; }
    }
}
