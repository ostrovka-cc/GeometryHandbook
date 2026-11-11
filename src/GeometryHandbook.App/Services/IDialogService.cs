using GeometryHandbook.App.ViewModels;
using GeometryHandbook.Core.Figures;

namespace GeometryHandbook.App.Services
{
    // Вікна й дії з вікнами, які ViewModel може викликати, нічого не знаючи про WPF
    public interface IDialogService
    {
        // Показує вікно створення чи редагування фігури; повертає фігуру або null, якщо користувач скасував
        Figure? ShowFigureEditor(FigureEditorViewModel editor);

        // Показує вікно звіту «Фільтр за периметром ±10%»
        void ShowPerimeterReport(PerimeterReportViewModel report);

        // Показує повідомлення про помилку (виняткову ситуацію)
        void ShowError(string message);

        // Питає, який файл відкрити; повертає шлях або null, якщо користувач скасував
        string? AskOpenFilePath();

        // Питає, куди зберегти файл; повертає шлях або null, якщо користувач скасував
        string? AskSaveFilePath();

        // Показує вікно «Про програму»
        void ShowAbout();

        // Закриває програму
        void ExitApplication();
    }
}