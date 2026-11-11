using System.Globalization;
using GeometryHandbook.Core.Exceptions;

namespace GeometryHandbook.App.ViewModels
{
    // Одне поле введення параметра фігури у вікні створення чи редагування (наприклад «Радіус, см»)
    public class ParameterInputViewModel : ViewModelBase
    {
        // Поле для властивості Text; спочатку поле порожнє
        private string _text = "";

        public ParameterInputViewModel(string name)
        {
            Name = name;
        }

        // Підпис над полем (назва параметра з каталогу)
        public string Name { get; }

        // Текст, який ввів користувач
        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        // Перетворює введений текст на число; кидає FigureException, якщо поле порожнє або там не число
        public double GetValue()
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                throw new FigureException($"Поле «{Name}» не заповнене.");
            }

            // Кома й крапка обидві підходять як десятковий роздільник
            string text = Text.Replace(',', '.');

            bool isNumber = double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double value);
            if (isNumber == false)
            {
                throw new FigureException($"Поле «{Name}»: «{Text}» не є числом.");
            }

            return value;
        }
    }
}