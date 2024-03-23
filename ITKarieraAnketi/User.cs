using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace ITKarieraAnketi
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }

    public class Survey
    {
        public int SurveyId { get; set; }
        public string SurveyName { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public List<SurveyQuestion> SurveyQuestions { get; set; }
    }
    public class SurveyQuestion
    {
        [Key]
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int SurveyId { get; set; }
        public Survey Survey { get; set; }
        public List<Answer> Answers { get; set; }
    }

    public class Answer
    {
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }
        public int QuestionId { get; set; }
        public SurveyQuestion SurveyQuestion { get; set; }
    }

    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConfigurationManager.ConnectionStrings["UserDatabase"].ConnectionString, new MySqlServerVersion(new Version(5, 5, 62)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SurveyQuestion>().ToTable("Questions");
            modelBuilder.Entity<Answer>().ToTable("Answers");

            modelBuilder.Entity<SurveyQuestion>()
                        .HasOne(q => q.Survey)
                        .WithMany(s => s.SurveyQuestions)
                        .HasForeignKey(q => q.SurveyId);

            modelBuilder.Entity<Answer>()
                        .HasOne(a => a.SurveyQuestion)
                        .WithMany(q => q.Answers)
                        .HasForeignKey(a => a.QuestionId);

            modelBuilder.Entity<Survey>()
                        .HasOne(s => s.User)
                        .WithMany()
                        .HasForeignKey(s => s.UserId)
                        .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
