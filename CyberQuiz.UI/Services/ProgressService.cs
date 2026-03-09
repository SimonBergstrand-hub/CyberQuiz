using System.Net.Http;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using CyberQuiz.BLL.DTOs;

namespace CyberQuiz.UI.Services
{
    public class ProgressService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProgressService(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<SubmitResultDto?> SubmitAnswerAsync(int questionId, int answerOptionId)
        {
            var client = _httpClientFactory.CreateClient("QuizApi");

            var dto = new SubmitAnswerDto { QuestionId = questionId, AnswerOptionId = answerOptionId };
            var json = JsonSerializer.Serialize(dto);

            var req = new HttpRequestMessage(HttpMethod.Post, "api/quiz/submit")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            if (_httpContextAccessor.HttpContext?.Request.Headers.TryGetValue("Cookie", out var cookie) == true)
            {
                req.Headers.Add("Cookie", (string[])cookie);
            }

            var resp = await client.SendAsync(req);
            if (!resp.IsSuccessStatusCode) return null;

            var stream = await resp.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<SubmitResultDto>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return result;
        }
    }
}
