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
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;

namespace ITKarieraAnketi.UIWindows
{
    /// <summary>
    /// Interaction logic for SurveyCreationWindow.xaml
    /// </summary>
    public partial class SurveyCreationWindow : Window
    {
        
        public List<TextBox> answerBoxes = new List<TextBox>();
        private List<SurveyQuestion> questions = new List<SurveyQuestion>();
        private int currentQuestionIndex = -1;
        public string surveyName;

        public SurveyCreationWindow(string surveyName)
        {
            InitializeComponent();
            // Set the survey name
            this.surveyName = surveyName;

            // Add two initial answer boxes
            answerBoxes.Add(answer1TextBox);
            answerBoxes.Add(answer2TextBox);

            GoToNextQuestion();
        }
        public int CurrentQuestionIndex => currentQuestionIndex;
        public TextBox QuestionTitleTextBox => questionTitleTextBox;
        public class SurveyQuestion
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

            public int QuestionId { get; set; }
            public string QuestionText { get; set; }
            public int SurveyId { get; set; }
            public Survey Survey { get; set; }
            public List<Answer> Answers { get; set; }
        }

        public class Answer
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int AnswerId { get; set; }
            public string AnswerText { get; set; }
            [ForeignKey("SurveyQuestion")]
            public int QuestionId { get; set; }
            public SurveyQuestion SurveyQuestion { get; set; }
        }

        public class MyDbContext : DbContext
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
            }
        }

        public void AddAnswerBox(object sender, RoutedEventArgs e)
        {
            if (answerBoxes.Count >= 4)
            {
                return;
            }

            TextBox newAnswerBox = new TextBox
            {
                Margin = new Thickness(10),
                BorderThickness = new Thickness(2)
            };
            answerBoxes.Add(newAnswerBox);
            answersPanel.Children.Add(newAnswerBox);
        }
        // add a new answer box, with the same properties as the first two

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

            // save the survey to the database
            using (var context = new MyDbContext())
            {
                var survey = new Survey
                {
                    SurveyName = surveyName,
                    UserId = Session.LoggedInUser?.Id ?? 0
                };
                context.Surveys.Add(survey);
                context.SaveChanges();

                foreach (var question in questions)
                {
                    // set the survey ID of the question
                    question.SurveyId = survey.SurveyId;

                    // add the question to the database
                    context.SurveyQuestions.Add(question);

                    // add the answers to the database
                    foreach (var answer in question.Answers)
                    {
                        context.Answers.Add(answer);
                    }
                }

                // save changes after adding all questions and answers
                context.SaveChanges();
            }

            // close the window and open the landing page
            LandingPageWindow landingPageWindow = new LandingPageWindow();
            landingPageWindow.Show();
            this.Close();
        }

        private void SaveCurrentQuestion()
        {
            if (currentQuestionIndex >= 0 && currentQuestionIndex < questions.Count)
            {
                // update existing question
                questions[currentQuestionIndex] = GetQuestionFromInputs();
            }
            else
            {
                // add new question
                questions.Add(GetQuestionFromInputs());
            }
        }

        public void GoToNextQuestion()
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

        public void GoToPreviousQuestion()
        {
            if (currentQuestionIndex > 0)
            {
                currentQuestionIndex--;
                LoadQuestion(questions[currentQuestionIndex]);
                UpdateQuestionNumberLabel();
            }
            BackButton.IsEnabled = currentQuestionIndex > 0;
        }

        public void LoadQuestion(SurveyQuestion question)
        {
            questionTitleTextBox.Text = question.QuestionText;
            for (int i = 0; i < question.Answers.Count; i++)
            {
                answerBoxes[i].Text = question.Answers[i].AnswerText;
            }
        }

        public void ClearInputs()
        {
            questionTitleTextBox.Clear();
            foreach (TextBox answerBox in answerBoxes)
            {
                answerBox.Clear();
            }
        }

        public SurveyQuestion GetQuestionFromInputs()
        {
            var answers = answerBoxes.Select(box => new Answer { AnswerText = box.Text }).ToList();
            return new SurveyQuestion
            {
                QuestionText = questionTitleTextBox.Text,
                Answers = answers
                // create a new question with the text from the question title box and the answers from the answer boxes
            };
        }

        private void UpdateQuestionNumberLabel()
        {
            QuestionNumberLabel.Content = $"Question {currentQuestionIndex + 1}";
        }

        public void RemoveAnswerBox(object sender, RoutedEventArgs e)
        {
            if (answerBoxes.Count > 2)
            {
                var lastAnswerBox = answerBoxes[answerBoxes.Count - 1];
                answerBoxes.Remove(lastAnswerBox);
                answersPanel.Children.Remove(lastAnswerBox);
                // remove the last answer box from the list and the panel
            }
        }
        public void SetQuestionTitle(string text)
        {
            questionTitleTextBox.Text = text;
        }

        public void SetAnswerBoxText(int index, string text)
        {
            if (index >= 0 && index < answerBoxes.Count)
            {
                answerBoxes[index].Text = text;
            }
        }

        public string GetAnswerBoxText(int index)
        {
            if (index >= 0 && index < answerBoxes.Count)
            {
                return answerBoxes[index].Text;
            }
            return null;
        }
    }
}
