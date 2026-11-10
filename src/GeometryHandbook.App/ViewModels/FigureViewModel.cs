using GeometryHandbook.App.Drawing;
using GeometryHandbook.Core.Figures;
using System.Windows.Media;
using GeometryHandbook.App.Drawing;

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


        public Geometry Geometry => FigureGeometryBuilder.Build(Model);
        // Текст для рядка списку 
        public string Title => Model.ToString();
    }
}