using GeometryHandbook.App.ViewModels;
using GeometryHandbook.Core.Figures;

namespace GeometryHandbook.App.Services
{
    // Вікна, які ViewModel може відкрити, нічого не знаючи про WPF
    public interface IDialogService
    {
        // Показує вікно «Нова фігура»; повертає створену фігуру або null, якщо користувач скасував
        Figure? ShowFigureEditor(FigureEditorViewModel editor);
    }
}