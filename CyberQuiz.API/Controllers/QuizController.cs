using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CyberQuiz.BLL.Interfaces;
using CyberQuiz.BLL.DTOs;

namespace CyberQuiz.API.Controllers
{
    // Kräver inloggning för alla anrop
    [Authorize] 
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        // Hämta kategorier med progression
        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized(); // Säkerhet 1

            var categories = await _quizService.GetCategoriesForUserAsync(userId);
            return Ok(categories);
        }

        // Hämta subkategorier 
        [HttpGet("subcategories/{subCategoryId}/questions")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestions(int subCategoryId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fråga BLL om denna subkategori faktiskt är upplåst för användaren
            bool isUnlocked = await _quizService.IsSubCategoryUnlockedAsync(userId, subCategoryId);

            if (!isUnlocked)
            {
                // 403 Forbidden - Användaren försöker fuska
                return Forbid(); 
            }

            var questions = await _quizService.GetQuestionsForSubCategoryAsync(subCategoryId);
            return Ok(questions);
        }

        // Skicka in ett svar och få direkt feedback
        [HttpPost("submit")]
        public async Task<IActionResult> SubmitAnswer([FromBody] SubmitAnswerDto submission)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            // Kollar om modellen/objektet inte är trasig
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _quizService.ValidateAndSaveAnswerAsync(userId, submission.QuestionId, submission.AnswerOptionId);

            // Om BLL returnerar null eller indikerar att frågan inte tillhör en upplåst nivå
            if (result == null) return BadRequest("Ogiltig fråga eller nivå låst.");

            return Ok(new
            {
                IsCorrect = result.IsCorrect,
                CorrectOptionId = result.CorrectOptionId,
                Feedback = result.IsCorrect ? "Rätt svar!" : "Tyvärr, det var fel."
            });
        }
    }
}
