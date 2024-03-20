using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.Windows.Shapes;
using System.ComponentModel.DataAnnotations;

namespace ITKarieraAnketi.UIWindows
{
    /// <summary>
    /// Interaction logic for SurveyCreationWindow.xaml
    /// </summary>
    public partial class SurveyCreationWindow : Window
    {
        private List<TextBox> answerBoxes = new List<TextBox>();
        private List<SurveyQuestion> questions = new List<SurveyQuestion>();
        private int currentQuestionIndex = -1;

        public SurveyCreationWindow(string surveyName)
        {
            InitializeComponent();

            // Set the survey name


            // Add two initial answer boxes
            answerBoxes.Add(answer1TextBox);
            answerBoxes.Add(answer2TextBox);

            GoToNextQuestion();
        }
        public class Answers
        {
            [Key]
            public int Id { get; set; }
            public string? Text { get; set; }
            public int SurveyQuestionId { get; set; }
            public SurveyQuestion? SurveyQuestion { get; set; }
        }
        public class SurveyQuestion
        {
            [Key]
            public int Id { get; set; }
            public string Question { get; set; }
            public string Answers { get; set; } 
            public int UserId { get; set; }

            public User User { get; set; }
        }
        public class MyDbContext : DbContext
        {
            public DbSet<User>? Users { get; set; }
            public DbSet<SurveyQuestion>? Surveys { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseMySql("server=sql11.freesqldatabase.com;database=sql11692795;user=sql11692795;password=j9ky13rSlb;port=3306;", new MySqlServerVersion(new Version(5, 5, 62)));
            }
        }

        private void AddAnswerBox(object sender, RoutedEventArgs e)
        {
            if (answerBoxes.Count >= 4)
            {
                // Maximum number of answers reached
                return;
            }

            // Add a new answer box, with the same properties as the first two
            TextBox newAnswerBox = new TextBox
            {
                Margin = new Thickness(10),
                BorderThickness = new Thickness(2)
            };
            answerBoxes.Add(newAnswerBox);
            answersPanel.Children.Add(newAnswerBox);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            SaveCurrentQuestion();
            GoToNextQuestion();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            SaveCurrentQuestion();
            GoToPreviousQuestion();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveCurrentQuestion();

            // Save the survey to the database
            using (var context = new MyDbContext())
            {
                foreach (var question in questions)
                {
                    // Check if the user exists
                    var user = context.Users.Find(question.UserId);
                    if (user == null)
                    {
                        // If the user doesn't exist, create a new user
                        user = new User { Id = question.UserId, Name = Session.LoggedInUser?.Name ?? string.Empty };
                        context.Users.Add(user);
                    }
                    else
                    {
                        // If the user exists, update the name
                        user.Name = Session.LoggedInUser?.Name ?? string.Empty;
                    }

                    // Set the user of the question
                    question.User = user;

                    context.Surveys.Add(question);
                }
                context.SaveChanges();
            }

            // Close the window and open the landing page
            LandingPageWindow landingPageWindow = new LandingPageWindow();
            landingPageWindow.Show();
            this.Close();
        }

        private void SaveCurrentQuestion()
        {
            if (currentQuestionIndex >= 0 && currentQuestionIndex < questions.Count)
            {
                // Update existing question
                questions[currentQuestionIndex] = GetQuestionFromInputs();
            }
            else
            {
                // Add new question
                questions.Add(GetQuestionFromInputs());
            }
        }

        private void GoToNextQuestion()
        {
            currentQuestionIndex++;
            if (currentQuestionIndex < questions.Count)
            {
                LoadQuestion(questions[currentQuestionIndex]);
            }
            else
            {
                ClearInputs();
            }

            UpdateQuestionNumberLabel();
            BackButton.IsEnabled = currentQuestionIndex > 0;
        }

        private void GoToPreviousQuestion()
        {
            if (currentQuestionIndex > 0)
            {
                currentQuestionIndex--;
                LoadQuestion(questions[currentQuestionIndex]);
            }

            UpdateQuestionNumberLabel();
            BackButton.IsEnabled = currentQuestionIndex > 0;
        }

        private void LoadQuestion(SurveyQuestion question)
        {
            questionTitleTextBox.Text = question.Question;
            for (int i = 0; i < question.Answers.Length; i++)
            {
               
                answerBoxes[i].Text = question.Answers[i].ToString();
            }
        }

        private void ClearInputs()
        {
            questionTitleTextBox.Clear();
            foreach (TextBox answerBox in answerBoxes)
            {
                answerBox.Clear();
            }
        }

        private SurveyQuestion GetQuestionFromInputs()
        {
            return new SurveyQuestion
            {
                Question = questionTitleTextBox.Text,
                Answers = string.Join(",", answerBoxes.Select(box => box.Text)),
                UserId = Session.LoggedInUser?.Id ?? 0
            };
        }

        private void UpdateQuestionNumberLabel()
        {
            QuestionNumberLabel.Content = $"Question {currentQuestionIndex + 1}";
        }
        
    }
}
