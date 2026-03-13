using CyberQuiz.BLL.DTOs;
using CyberQuiz.BLL.Interfaces;
using CyberQuiz.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace CyberQuiz.BLL.Services
{
    public class AiService : IAiService
    {
        private readonly QuizDbContext _db;
        private readonly HttpClient _httpClient;

        public AiService(QuizDbContext db, HttpClient httpClient)
        {
            _db = db;
            _httpClient = httpClient;
        }

        public async Task<AiResponseDto> GetPersonalFeedbackAsync(string userId)
        {
            // Hämtar användarens resultat 
            var results = await _db.UserResults
                .Where(ur => ur.UserId == userId)
                .ToListAsync();

            // Fallback
            if (results == null || !results.Any())
            {
                return new AiResponseDto
                {
                    Feedback = "I don't have enough data to coach you yet. Please complete a " +
                    "few quiz categories so I can analyze your strengths and weaknesses!",
                    GeneratedAt = DateTime.Now
                };
            }

            // Begränsa till unika frågor (ta bästa försöket per fråga) och max 50 senaste
            var uniqueResults = results
                .GroupBy(r => r.QuestionId)
                .Select(g => g.OrderByDescending(r => r.IsCorrect).ThenByDescending(r => r.Id).First())
                .OrderByDescending(r => r.Id) // Senaste först
                .Take(50) // Max 50 frågor för att hålla prompten kort
                .ToList();

            // Hämtar alla frågor som användaren har svarat på.
            var questionIds = uniqueResults.Select(r => r.QuestionId).ToList();
            var questions = await _db.Questions
                .Where(q => questionIds.Contains(q.Id))
                .ToDictionaryAsync(q => q.Id, q => q.Text);

            // Skapa en textbeskrivning av resultaten (Parsing)
            var summary = new StringBuilder();
            foreach (var res in uniqueResults)
            {
                // Hämtar texten från vår dictionary med QuestionId
                if (questions.TryGetValue(res.QuestionId, out var questionText))
                {
                    summary.AppendLine($"- Question: {questionText}, Correct: {res.IsCorrect}");
                }
            }

            // Prompten till Phi-3
            var prompt =
                $"Do NOT include greetings or introductions.\n" +
                $"Do NOT mention 'cybersecurity student' or any other label.\n" +
                $"ALWAYS address the user as 'you'.\n" +
                $"Start directly with the feedback.\n" +
                $"Analyze these quiz results for a cybersecurity student and give a brief, " +
                $"encouraging summary of their strengths and what they need to improve: \n{summary}";



            var requestBody = new
            {
                model = "phi3",
                prompt = prompt,
                stream = false
            };

            // Skicka till Ollama
            var response = await _httpClient.PostAsJsonAsync("http://localhost:11434/api/generate", requestBody);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Ollama returnerar ett JSON-objekt med fältet "response"
            using var doc = JsonDocument.Parse(jsonResponse);
            var aiText = doc.RootElement.GetProperty("response").GetString();

            return new AiResponseDto
            {
                Feedback = aiText ?? "Could not generate feedback at this time.",
                GeneratedAt = DateTime.Now
            };
        }
    }
}
