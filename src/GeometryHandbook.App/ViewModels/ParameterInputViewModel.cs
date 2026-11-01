using System.Globalization;
using GeometryHandbook.Core.Exceptions;

namespace GeometryHandbook.App.ViewModels
{
    // Одне поле введення в редакторі фігури: підпис (наприклад, «Радіус»)
    // і текст, який ввів користувач. Редактор створює стільки таких полів,
    // скільки параметрів має обрана фігура.
    public class ParameterInputViewModel : ViewModelBase
    {
        // Поле для властивості Text
        private string _text = "";

        // Конструктор: отримує підпис поля з каталогу
        public ParameterInputViewModel(string name)
        {
            Name = name;
        }

        // Підпис поля (назва параметра фігури)
        public string Name { get; }

        // Текст, введений користувачем
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        // Перетворює введений текст на число.
        // Приймає і кому, і крапку як роздільник дробової частини.
        // Кидає FigureException, якщо поле порожнє або містить не число.
        public double GetValue()
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                throw new FigureException($"Поле «{Name}» не заповнене.");
            }

            // Кома замінюється на крапку, щоб «12,5» і «12.5» читалися однаково
            string normalized = Text.Replace(',', '.');

            if (!double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
            {
                throw new FigureException($"Поле «{Name}»: «{Text}» не є числом.");
            }

            return value;
        }
    }
}