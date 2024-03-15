using System.Windows;
using ITKarieraAnketi.UIWindows;
namespace ITKarieraAnketi
{

    public partial class MainWindow : Window
    {
      
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            LandingPageWindow landingPageWindow = new LandingPageWindow();
            landingPageWindow.Show();
            Close();
        }
    }
}
