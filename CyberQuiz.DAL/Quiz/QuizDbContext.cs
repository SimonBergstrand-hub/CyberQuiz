using Microsoft.EntityFrameworkCore;

namespace CyberQuiz.DAL.Quiz
{
    public class QuizDbContext : DbContext
    {
        public QuizDbContext(DbContextOptions<QuizDbContext> options) : base(options) { }

        public DbSet<Category> Categories => Set<Category>();
        public DbSet<SubCategory> SubCategories => Set<SubCategory>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<AnswerOption> AnswerOptions => Set<AnswerOption>();
        public DbSet<UserResult> UserResults => Set<UserResult>();


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Category>()
                .HasMany(c => c.SubCategories)
                .WithOne(sc => sc.Category)
                .HasForeignKey(sc => sc.CategoryId);

            builder.Entity<SubCategory>()
                .HasMany(sc => sc.Questions)
                .WithOne(q => q.SubCategory)
                .HasForeignKey(q => q.SubCategoryId);

            builder.Entity<Question>()
                .HasMany(q => q.AnswerOptions)
                .WithOne(a => a.Question)
                .HasForeignKey(a => a.QuestionId);
        }

    }
}
