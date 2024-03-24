using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ITKarieraAnketi.View.UserControls
{
    /// <summary>
    /// Interaction logic for ClearableTextbox.xaml
    /// </summary>
    public partial class ClearableTextbox : UserControl, INotifyPropertyChanged
    {
        private string textboxPlaceholder;

        public event PropertyChangedEventHandler? PropertyChanged;
        public string Text
        {
            get { return TextInput.Text; }
            set { TextInput.Text = value; }
        }

        public string TBplaceholder
        {
            get { return textboxPlaceholder; }
            set
            {
                textboxPlaceholder = value;
                TextBoxPlaceholder.Text = textboxPlaceholder;
            }
        }

        public ClearableTextbox()
        {
            DataContext = this;
            InitializeComponent();
        }

        public void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            TextInput.Clear();
            TextInput.Focus();
        }

        public void TextInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextInput.Text))
            {
                TextBoxPlaceholder.Visibility = Visibility.Visible;
            }
            else
            {
                TextBoxPlaceholder.Visibility = Visibility.Hidden;
            }
        }

    }
}
