using CyberQuiz.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.BLL.Interfaces
{
    internal class IProgressService
    {
        Task<double> CalculateScorePercentage(string userId, int subCategoryId);

        Task<bool> HasPassedSubCategory(string userId, int subCategoryId);

        Task UnlockNextSubCategory(string userId, int currentSubCategoryId);
    }
}
