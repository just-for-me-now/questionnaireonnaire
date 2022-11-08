using Microsoft.EntityFrameworkCore;

namespace Questionnaireonnaire.Models
{
    public class QuestionnaireContext : DbContext
    {
        public QuestionnaireContext (DbContextOptions<QuestionnaireContext> options) : base(options)
        {

        }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<UserData> Users { get; set; }
        public DbSet<Response> Responses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Question>().ToTable("Question");
            modelBuilder.Entity<Answer>().ToTable("Answer");
            modelBuilder.Entity<UserData>().ToTable("UserData");
            modelBuilder.Entity<Response>().ToTable("Response");
        }
    }
}
