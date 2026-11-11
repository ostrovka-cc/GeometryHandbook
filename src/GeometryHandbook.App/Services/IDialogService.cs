using GeometryHandbook.App.ViewModels;
using GeometryHandbook.Core.Figures;

namespace GeometryHandbook.App.Services
{
    // Вікна, які ViewModel може відкрити, нічого не знаючи про WPF
    public interface IDialogService
    {
        // Показує вікно створення чи редагування фігури; повертає фігуру або null, якщо користувач скасував
        Figure? ShowFigureEditor(FigureEditorViewModel editor);

        // Показує вікно звіту «Фільтр за периметром ±10%»
        void ShowPerimeterReport(PerimeterReportViewModel report);

        // Показує повідомлення про помилку (виняткову ситуацію)
        void ShowError(string message);
    }
}