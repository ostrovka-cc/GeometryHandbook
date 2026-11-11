using System.Windows;
using GeometryHandbook.App.Services;
using GeometryHandbook.App.ViewModels;
using GeometryHandbook.App.Views;
using GeometryHandbook.Core.Catalog;

namespace GeometryHandbook.App
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Каталог усіх фігур, які вміє створювати програма
            var catalog = new FigureCatalog();

            // Сервіс, через який ViewModel відкриває вікна
            var dialogService = new WpfDialogService();

            var mainViewModel = new MainViewModel(catalog, dialogService);
            var mainWindow = new MainWindow(mainViewModel);
            mainWindow.Show();
        }
    }
}