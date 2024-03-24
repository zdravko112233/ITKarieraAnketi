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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        public void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new UserContext())
            {
                var hashedUserPassword = HashUserPassword(textLoginPassword.Text);
                var user = context.Users.FirstOrDefault(u => u.Name == textLoginName.Text && u.Password == hashedUserPassword);
                if (user != null)
                {
                    Session.LoggedInUser = user;
                    LandingPageWindow landingPageWindow = new LandingPageWindow();
                    landingPageWindow.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Invalid account details.");
                }
            }
        }
        // hashes the password, checks if its the same as the one in the database; if it is, open LandingPageWindow; if not, show an error message
        public string HashUserPassword(string password)
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
        public void SetLoginName(string name)
        {
            textLoginName.Text = name;
        }

        public void SetLoginPassword(string password)
        {
            textLoginPassword.Text = password;
        }
    }
}
