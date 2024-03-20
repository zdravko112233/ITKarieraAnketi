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
