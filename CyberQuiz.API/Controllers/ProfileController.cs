using CyberQuiz.BLL.DTOs;
using CyberQuiz.BLL.Interfaces;
using CyberQuiz.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CyberQuiz.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IQuizService _quizService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<IdentityUser> _userManager;

        public ProfileController(IQuizService quizService, ICategoryService categoryService, UserManager<IdentityUser> userManager)
        {
            _categoryService = categoryService;
            _quizService = quizService;
            _userManager = userManager;
        }

        // GET: api/profile/progression
        // Visar total statistik för användaren (t.ex. totalt antal rätta svar)
        [HttpGet("progression")]
        public async Task<IActionResult> GetOverallProgression()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // Här anropar vi en metod i BLL som räknar ut totalen
            var stats = await _quizService.GetUserStatsAsync(userId);
            return Ok(stats);
        }

        [HttpGet("subcategories/{categoryId}")]
        public async Task<ActionResult<IEnumerable<SubCategoryStatsDto>>> GetSubCategoryStats(int categoryId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var stats = await _categoryService.GetSubCategoryStatsAsync(categoryId, userId);

            return Ok(stats);
        }
    }
}