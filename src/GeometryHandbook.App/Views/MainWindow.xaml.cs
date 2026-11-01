using GeometryHandbook.App.ViewModels;
using System.Windows;

namespace GeometryHandbook.App.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel viewModel)
        {
      
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}