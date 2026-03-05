using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.BLL.DTOs
{
    public class QuestionDTO
    {
        public int Id {  get; set; }
        public string Text { get; set; }

        public List<AnswerOptionDTO> optionlist { get; set; }
    }
}
