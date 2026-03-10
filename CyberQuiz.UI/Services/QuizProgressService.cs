using System.Net.Http.Json;

namespace CyberQuiz.UI.Services;

public class QuizProgressService
{
    private readonly HttpClient _http;

    public QuizProgressService(HttpClient http)
    {
        _http = http;
    }

    public async Task<SubmitAnswerResponse?> SubmitAnswerAsync(
        int questionId,
        int answerOptionId)
    {
        var response = await _http.PostAsJsonAsync("api/quiz/submit", new
        {
            questionId,
            answerOptionId
        });

        return await response.Content.ReadFromJsonAsync<SubmitAnswerResponse>();
    }
}

public class SubmitAnswerResponse
{
    public bool IsCorrect { get; set; }

    public int CorrectOptionId { get; set; }

    public string? Feedback { get; set; }
}