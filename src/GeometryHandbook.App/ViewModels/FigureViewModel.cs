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

        // Колір заливки; при зміні повідомляємо вікно, щоб полотно перемалювало фігуру
        public string FillColor
        {
            get { return Model.FillColor; }
            set
            {
                Model.FillColor = value;
                OnPropertyChanged();
            }
        }

        // Текст для рядка списку 
        public string Title => Model.ToString();

        // Формули для вкладки «Формули»
        public IReadOnlyList<string> Formulas => Model.GetFormulas();
    }

}