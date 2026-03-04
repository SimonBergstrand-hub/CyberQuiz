namespace CyberQuiz.DAL.Quiz
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public int SubCategoryId { get; set; }
        public SubCategory SubCategory { get; set; } = null!;

        public ICollection<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();
    }
}
