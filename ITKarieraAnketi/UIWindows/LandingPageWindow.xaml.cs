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
        public LandingPageWindow()
        {
            InitializeComponent();
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
                    Margin = new Thickness(10)
                };
                button.Click += (sender, e) => OpenSurvey(survey.SurveyId);
                SurveyList.Children.Add(button);
            }
        }
        public List<Survey> GetSurveysForUser(int userId)
        {
            using (var context = new UserContext())
            {
                return context.Surveys
                    .Where(s => s.UserId == userId)
                    .OrderByDescending(s => s.SurveyId)
                    .Take(9)
                    .ToList();
            }
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
    }
}
