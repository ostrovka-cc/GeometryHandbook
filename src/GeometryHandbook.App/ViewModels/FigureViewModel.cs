using GeometryHandbook.Core.Figures;

namespace GeometryHandbook.App.ViewModels
{
    // Обгортка над фігурою з моделі для відображення в інтерфейсі.
    public class FigureViewModel : ViewModelBase
    {
        // Фігура з моделі, яку загортає ця ViewModel
        public Figure Model { get; }

        // Конструктор: приймає фігуру з моделі
        public FigureViewModel(Figure model)
        {
            Model = model;
        }

        // Характеристики фігури для таблиці властивостей (назва — значення)
        public IReadOnlyList<FigureProperty> Properties => Model.GetProperties();

        // Текст для рядка списку 
        public string Title => Model.ToString();
    }
}