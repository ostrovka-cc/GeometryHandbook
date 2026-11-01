using System.Windows;
using GeometryHandbook.App.ViewModels;
using GeometryHandbook.App.Views;

namespace GeometryHandbook.App
{
    // Точка входу WPF-застосунку
    public partial class App : Application
    {
        // Запуск програми: створює головну ViewModel і вікно,
        // передає ViewModel у вікно та показує його
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainViewModel = new MainViewModel();
            var mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();
        }
    }
}