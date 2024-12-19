using System.Windows;

namespace LingoScape_Translation_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LingoScape.DataAccessLayer.DatabaseInitializer.InitializeEmptyDatabase();
            Console.WriteLine("Database setup completed.");
        }
    }
}