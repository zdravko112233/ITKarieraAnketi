using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
                    var hashedUserPassword = HashUserPassword(textRegisterPassword.Text);
                    var user = new User { Name = textRegisterName.Text, Password = HashUserPassword(textRegisterPassword.Text) };
                    context.Users.Add(user);
                    context.SaveChanges();
                }

                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                Close();

            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                MessageBox.Show(ex.Message, "Database error, please try again later.", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private string HashUserPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        // hash the user's inputted password using SHA256 and set it as their password in the database(through buttonCreateAccount_Click)
    }
}
