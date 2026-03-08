using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CyberQuiz.BLL.Interfaces;
using CyberQuiz.BLL.DTOs;


namespace CyberQuiz.API.Controllers
{
    // Kräver inloggning för att se kategorier
    [Authorize] 
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/categories
        // Visar alla kategorier och hur många subkategorier som är klara (80% regeln)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
        {
            // Hämta inloggad användares ID från JWT/Identity-token
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var categories = await _categoryService.GetCategoriesWithProgressionAsync(userId);
            return Ok(categories);
        }

        // GET: api/categories/{id}/subcategories
        // Visar alla subkategorier för en specifik kategori och om de är låsta/olåsta
        [HttpGet("{id}/subcategories")]
        public async Task<ActionResult<IEnumerable<SubCategoryDto>>> GetSubCategories(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var subCategories = await _categoryService.GetSubCategoriesWithLockStatusAsync(id, userId);

            if (subCategories == null) return NotFound("Kategorin hittades inte.");

            return Ok(subCategories);
        }
    }
}
