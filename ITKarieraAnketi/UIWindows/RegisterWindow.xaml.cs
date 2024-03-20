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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void buttonCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new UserContext())
                {
                    var user = new User { Name = textRegisterName.Text, Password = textRegisterPassword.Text };
                    context.Users.Add(user);
                    context.SaveChanges();
                }

                LandingPageWindow landingPageWindow = new LandingPageWindow();
                landingPageWindow.Show();
                Close();

            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(ex.Message, "Database error, please try again later.", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
