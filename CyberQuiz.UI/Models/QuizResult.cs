namespace CyberQuiz.Models
{
    public class QuizResult
    {
        public int CategoryId { get; set; }

        public int DifficultyIndex { get; set; }

        public int Score { get; set; }

        public int TotalQuestions { get; set; }

        public DateTime CompletedAt { get; set; }
    }
}