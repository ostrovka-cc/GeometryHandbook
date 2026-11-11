using System.Windows;
using GeometryHandbook.App.ViewModels;
using GeometryHandbook.App.Views.Dialogs;
using GeometryHandbook.Core.Figures;

namespace GeometryHandbook.App.Services
{
    // Справжнє відкриття вікон WPF
    public class WpfDialogService : IDialogService
    {
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
    }
}