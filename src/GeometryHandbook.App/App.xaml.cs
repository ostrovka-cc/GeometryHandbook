using System.Windows;
using GeometryHandbook.App.ViewModels;
using GeometryHandbook.App.Views;
using GeometryHandbook.Core.Catalog;

namespace GeometryHandbook.App
{
    // Точка входу WPF-застосунку
    public partial class App : Application
    {
        // Запуск програми: створює каталог фігур, головну ViewModel і вікно,
        // передає їх одне одному та показує вікно
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var catalog = new FigureCatalog();
            var mainViewModel = new MainViewModel(catalog);
            var mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();
        }
    }
}