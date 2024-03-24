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
using ITKarieraAnketi.UIWindows;
using Microsoft.EntityFrameworkCore;

namespace ITKarieraAnketi.UIWindows
{
    /// <summary>
    /// Interaction logic for LandingPageWindow.xaml
    /// </summary>
    public partial class LandingPageWindow : Window
    {
        public UserContext UserContext { get; set; }

        public LandingPageWindow()
        {
            InitializeComponent();
            UserContext = new UserContext();
            LoadSurveys();
        }
        private void LoadSurveys()
        {
            int userId = Session.LoggedInUser.Id;
            var surveys = GetSurveysForUser(userId);
            foreach (var survey in surveys)
            {
                var button = new Button
                {
                    Content = survey.SurveyName,
                    Width = 200,
                    Height = 100,
                    Margin = new Thickness(10),
                    BorderThickness = new Thickness(3)

                };
                button.Click += (sender, e) => OpenSurvey(survey.SurveyId);
                SurveyList.Children.Add(button);
            }
        }
        public List<Survey> GetSurveysForUser(int userId)
        {
            return UserContext.Surveys
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.SurveyId)
                .Take(15)
                .ToList();
        }
        private void OpenSurvey(int surveyId)
        {
          ViewSurveyWindow viewSurveyWindow = new ViewSurveyWindow(surveyId);
          viewSurveyWindow.Show();
          Close();
        }

        private void CreateNewServey_Click(object sender, RoutedEventArgs e)
        {
            // Show an input dialog
            var dialog = new InputDialog("Enter the name of the new survey:");
            if (dialog.ShowDialog() == true)
            {
                // Get the name from the dialog
                string surveyName = dialog.ResponseText;

                // Pass the name to the SurveyCreationWindow
                SurveyCreationWindow surveyCreationWindow = new SurveyCreationWindow(surveyName);
                surveyCreationWindow.Show();
                Close();
            }
        }

        private void MenuItemLogout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void MenuItemDeleteAccount_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to delete your account?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                using (var context = new UserContext())
                {
                    var user = context.Users.FirstOrDefault(u => u.Id == Session.LoggedInUser.Id);

                    if (user != null)
                    {
                        var surveys = context.Surveys.Where(s => s.UserId == user.Id).ToList();
                        foreach (var survey in surveys)
                        {
                            var questions = context.SurveyQuestions.Where(q => q.SurveyId == survey.SurveyId).ToList();
                            foreach (var question in questions)
                            {
                                var answers = context.Answers.Where(a => a.QuestionId == question.QuestionId).ToList();
                                context.Answers.RemoveRange(answers);
                            }
                            context.SurveyQuestions.RemoveRange(questions);
                        }
                        context.Surveys.RemoveRange(surveys);
                        context.Users.Remove(user);
                        context.SaveChanges();
                    }
                }

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Close();
            }
        }
    }
}
