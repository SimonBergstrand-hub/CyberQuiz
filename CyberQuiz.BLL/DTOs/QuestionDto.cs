using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.BLL.DTOs
{
    internal class QuestionDTO
    {
        int id {  get; set; }
        string text { get; set; }

        list<AnswerOptionDTO> optionlist { get; set; }
    }
}
