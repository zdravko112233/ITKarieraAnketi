using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ITKarieraAnketi.UIWindows
{
    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        public InputDialog(string question)
        {
            InitializeComponent();
            lblQuestion.Content = question;
        }

        public string ResponseText
        {
            get { return nameTextBox.Text; }
            set { nameTextBox.Text = value; }
        }

        public bool TryCloseDialog()
        {
            if (this.IsVisible)
            {
                this.Close();
                return true;
            }
            return false;
        }

        public void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            TryCloseDialog();
            // when the dialog is closed(user clicking the OK button), open SurveyCreationWindow 
        }
    }
}
