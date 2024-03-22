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
using System.Windows.Shapes;

namespace ITKarieraAnketi.UIWindows
{
    /// <summary>
    /// Interaction logic for ViewSurveyWindow.xaml
    /// </summary>
    public partial class ViewSurveyWindow : Window
    {
        public ViewSurveyWindow(int surveyId)
        {
            InitializeComponent();
            LoadSurvey(surveyId);
        }
        private void LoadSurvey(int surveyId)
        {
            using (var context = new UserContext())
            {
                var survey = context.Surveys
                    .Include(s => s.SurveyQuestions)
                    .ThenInclude(q => q.Answers)
                    .Single(s => s.SurveyId == surveyId);

                SurveyNameLabel.Content = survey.SurveyName;

                foreach (var question in survey.SurveyQuestions)
                {
                    QuestionsPanel.Children.Add(new Label { Content = question.QuestionText });

                    var listBox = new ListBox();
                    foreach (var answer in question.Answers)
                    {
                        listBox.Items.Add(answer.AnswerText);
                    }
                    QuestionsPanel.Children.Add(listBox);
                }
            }
        }
    }
}
