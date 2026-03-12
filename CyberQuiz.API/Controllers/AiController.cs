using CyberQuiz.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CyberQuiz.API.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AiController : ControllerBase 
    {

        private readonly IAiService _aiService;

        public AiController(IAiService aiService)
        {
            _aiService = aiService;
        }

        [HttpGet("coach-feedback")]
        public async Task<IActionResult> GetFeedback()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var feedback = await _aiService.GetPersonalFeedbackAsync(userId);
            return Ok(feedback);
        }

    }
}
