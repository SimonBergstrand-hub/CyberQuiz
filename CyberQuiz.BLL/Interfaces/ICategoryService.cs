using System;
using System.Collections.Generic;
using System.Text;
using CyberQuiz.BLL.DTOs;

namespace CyberQuiz.BLL.Interfaces
{
    public interface ICategoryService
    {
        // För GetAll: Hämtar alla kategorier med info om progression
        Task<IEnumerable<CategoryDto>> GetCategoriesWithProgressionAsync(string userId);

        // För GetSubCategories: Hämtar subkategorier för en viss kategori med IsLocked-status
        Task<IEnumerable<SubCategoryDto>> GetSubCategoriesWithLockStatusAsync(int categoryId, string userId);

        // För IsUnlocked (om du vill ha den separata kontrollen)
        Task<bool> IsSubCategoryUnlockedAsync(string userId, int subCategoryId);

        Task<IEnumerable<SubCategoryStatsDto>> GetSubCategoryStatsAsync(int categoryId, string userId);
    }
}
