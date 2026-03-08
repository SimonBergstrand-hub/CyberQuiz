using CyberQuiz.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CyberQuiz.BLL.Interfaces
{
    public interface IProgressService
    {
        Task<double> CalculateScorePercentage(string userId, int subCategoryId);

        Task<bool> HasPassedSubCategory(string userId, int subCategoryId);

        Task<List<SubCategoryDto>> GetSubCategoriesWithStatusAsync(int categoryId, string userId);
    }
}
