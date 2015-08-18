using System.Windows;
using EECT.ViewModel;

namespace EECT.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }
    }
}