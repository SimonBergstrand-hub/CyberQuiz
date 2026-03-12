using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.BLL.DTOs
{
    public class AiResponseDto
    {
        public string Feedback { get; set; } = null!;
        public DateTime GeneratedAt { get; set; }
    }
}
