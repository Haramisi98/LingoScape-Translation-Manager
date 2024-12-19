using LingoScape_Translation_Manager.Login;
using System.Configuration;
using System.Data;
using System.Windows;

namespace LingoScape_Translation_Manager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public string AccessToken { get; set; } = string.Empty;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            // Open login window and wait
            LoginWindow loginWindow = new LoginWindow();
            bool? result = loginWindow.ShowDialog();

            if (result == true)
            {
                // Authentication successful, open main window
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                // Close the app if login fails or is canceled
                Shutdown();
            }
        }
    }

}
