using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.BLL.DTOs
{
    public class SubCategoryStatusDto
    {

        public int SubCategoryId { get; set; }

        public string Name { get; set; }

        public bool IsLocked { get; set; }

        public double Percentage { get; set; }
    }
}
