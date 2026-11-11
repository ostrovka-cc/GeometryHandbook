using System.Collections.Generic;
using System.Windows.Media;
using GeometryHandbook.App.Drawing;
using GeometryHandbook.Core.Figures;

namespace GeometryHandbook.App.ViewModels
{
    // Обгортка над фігурою з моделі: дає вікну все, що треба показати, і повідомляє про зміни
    public class FigureViewModel : ViewModelBase
    {
        public FigureViewModel(Figure model)
        {
            Model = model;
        }

        // Сама фігура з Core
        public Figure Model { get; }

        // Поточний масштаб відносно початкового розміру (1 — фігура такою, як її створили або відредагували)
        public double CurrentScale { get; private set; } = 1;

        // Текст фігури (назва й площа)
        public string Title => Model.ToString();

        // Площа для списку фігур; оновлюється після масштабування й редагування
        public double Area => Model.Area;

        // Рядки для таблиці «Характеристики»
        public IReadOnlyList<FigureProperty> Properties => Model.GetProperties();

        // Рядки для вкладки «Формули»
        public IReadOnlyList<string> Formulas => Model.GetFormulas();

        // Обрис фігури для малювання на полотні
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

        // Встановлює новий масштаб відносно початкового розміру (2 — удвічі більша за початкову)
        public void SetScale(double scale)
        {
            // Модель уміє лише «помножити розміри на k», тому рахуємо, у скільки разів
            // новий масштаб відрізняється від поточного (було 2, стало 10 — множимо на 5)
            double factor = scale / CurrentScale;
            Model.Scale(factor);

            // Запам'ятовуємо масштаб лише після успішного Model.Scale (якщо там виняток, сюди не дійдемо)
            CurrentScale = scale;

            RefreshDrawingAndInfo();
        }

        // Переписує розміри фігури з відредагованої копії того ж типу (оператор присвоєння AssignFrom)
        public void ApplyEdit(Figure edited)
        {
            // AssignFrom копіює й колір, тому даємо копії поточний колір, щоб він не скинувся
            edited.FillColor = Model.FillColor;

            Model.AssignFrom(edited);

            // Нові розміри стають новим початковим розміром
            CurrentScale = 1;

            RefreshDrawingAndInfo();
        }

        // Повідомляє вікно про все, що змінюється разом із розмірами фігури
        private void RefreshDrawingAndInfo()
        {
            OnPropertyChanged(nameof(Geometry));
            OnPropertyChanged(nameof(Properties));
            OnPropertyChanged(nameof(Formulas));
            OnPropertyChanged(nameof(Area));
            OnPropertyChanged(nameof(Title));
        }
    }
}