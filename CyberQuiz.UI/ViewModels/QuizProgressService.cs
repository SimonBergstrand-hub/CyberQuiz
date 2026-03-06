using CyberQuiz.Models;

namespace CyberQuiz.Services
{
    public class QuizProgressService
    {
        // Simulated database
        private readonly List<QuizResult> _results = new();

        // Event raised when a new result is saved so UI can refresh
        public event Action? ResultSaved;

        public Task SaveResultAsync(QuizResult result)
        {
            _results.Add(result);

            // Debug logging
            try
            {
                Console.WriteLine($"[QuizProgressService] Saved result: Category={result.CategoryId} Difficulty={result.DifficultyIndex} Score={result.Score} CompletedAt={result.CompletedAt}");
            }
            catch { }

            // Notify subscribers that a new result has been saved
            ResultSaved?.Invoke();

            return Task.CompletedTask;
        }

        public Task<List<QuizResult>> GetResultsAsync()
        {
            return Task.FromResult(_results);
        }

        public Task<QuizResult?> GetResultAsync(int categoryId, int difficultyIndex)
        {
            var result = _results
                .Where(r =>
                    r.CategoryId == categoryId &&
                    r.DifficultyIndex == difficultyIndex)
                .OrderByDescending(r => r.CompletedAt)
                .FirstOrDefault();

            return Task.FromResult(result);
        }
    }
}