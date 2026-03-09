using CyberQuiz.BLL.Interfaces;
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
        private readonly UserManager<IdentityUser> _userManager;

        public ProfileController(IQuizService quizService, UserManager<IdentityUser> userManager)
        {
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
    }
}