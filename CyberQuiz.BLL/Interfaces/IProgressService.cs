using CyberQuiz.BLL.DTOs;

namespace CyberQuiz.BLL.Interfaces
{
    public interface IProgressService
    {
        Task<double> CalculateScorePercentage(string userId, int subCategoryId);
        Task<bool> HasPassedSubCategory(string userId, int subCategoryId);
        Task<List<SubCategoryDto>> GetSubCategoriesWithStatusAsync(int categoryId, string userId);
    }
}