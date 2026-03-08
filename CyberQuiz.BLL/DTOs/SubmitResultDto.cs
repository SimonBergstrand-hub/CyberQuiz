using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.BLL.DTOs
{
    //krävs för att ValidateAndSaveAnswerAsync ska kunna returnera feedback
    public class SubmitResultDto
    {
        public bool IsCorrect { get; set; }
        public int CorrectOptionId { get; set; }
    }
}
