﻿using System;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new UserContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Name == textLoginName.Text && u.Password == textLoginPassword.Text);
                if (user != null)
                {
                    // set the logged in user
                    Session.LoggedInUser = user;

                    // open the landing page
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
    }
}
