using System.Windows;
using Microsoft.Win32;
using GeometryHandbook.App.ViewModels;
using GeometryHandbook.App.Views.Dialogs;
using GeometryHandbook.Core.Figures;

namespace GeometryHandbook.App.Services
{
    // Справжнє відкриття вікон WPF
    public class WpfDialogService : IDialogService
    {
        // Які файли показувати у вікнах відкриття й збереження
        private const string FileFilter = "Файли фігур (*.txt)|*.txt|Усі файли (*.*)|*.*";

        public Figure? ShowFigureEditor(FigureEditorViewModel editor)
        {
            var window = new FigureEditorWindow(editor);

            // Вікно з'являється по центру головного й блокує його, поки відкрите
            window.Owner = Application.Current.MainWindow;

            bool? result = window.ShowDialog();

            if (result == true)
            {
                return editor.CreatedFigure;
            }

            return null;
        }

        public void ShowPerimeterReport(PerimeterReportViewModel report)
        {
            var window = new PerimeterReportWindow(report);
            window.Owner = Application.Current.MainWindow;
            window.ShowDialog();
        }

        public void ShowError(string message)
        {
            MessageBox.Show(
                Application.Current.MainWindow,
                message,
                "Помилка",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        public string? AskOpenFilePath()
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "Відкрити фігури з файлу";
            dialog.Filter = FileFilter;

            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }

            return null;
        }

        public string? AskSaveFilePath()
        {
            var dialog = new SaveFileDialog();
            dialog.Title = "Зберегти фігури у файл";
            dialog.Filter = FileFilter;
            dialog.FileName = "figures.txt";

            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }

            return null;
        }

        public void ShowAbout()
        {
            string text =
                "Довідник з геометрії\n\n" +
                "Курсова робота з дисципліни «Об'єктно-орієнтоване програмування»\n" +
                "Студент групи ПЗ-24 Островка Петро\n" +
                "НУ «Львівська політехніка», 2026";

            MessageBox.Show(
                Application.Current.MainWindow,
                text,
                "Про програму",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        public void ExitApplication()
        {
            Application.Current.Shutdown();
        }
    }
}