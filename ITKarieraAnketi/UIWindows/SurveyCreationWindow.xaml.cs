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
using System.Windows.Shapes;

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

        public SurveyCreationWindow()
        {
            InitializeComponent();

            // Add two initial answer boxes
            answerBoxes.Add(answer1TextBox);
            answerBoxes.Add(answer2TextBox);

            GoToNextQuestion();
        }
        public class SurveyQuestion
        {
            public string Question { get; set; }
            public List<string> Answers { get; set; }
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
            for (int i = 0; i < question.Answers.Count; i++)
            {
                answerBoxes[i].Text = question.Answers[i];
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
                Answers = answerBoxes.Select(box => box.Text).ToList()
            };
        }

        private void UpdateQuestionNumberLabel()
        {
            QuestionNumberLabel.Content = $"Question {currentQuestionIndex + 1}";
        }
    }
}
