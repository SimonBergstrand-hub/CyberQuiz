namespace CyberQuiz.DAL.Models
{
    public class UserResult
    {
        public int Id { get; set; }

        public string UserId { get; set; } = null!;

        public int SubCategoryId { get; set; }
        public int QuestionId { get; set; }

        public bool IsCorrect { get; set; }
    }
}
