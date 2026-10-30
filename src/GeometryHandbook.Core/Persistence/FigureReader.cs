using System.Globalization;
using GeometryHandbook.Core.Catalog;
using GeometryHandbook.Core.Exceptions;
using GeometryHandbook.Core.Figures;

namespace GeometryHandbook.Core.Persistence
{
    // Читання фігур із текстового файлу.
    // Кожен рядок файлу має формат: тип;колір;параметр1;параметр2;...
    // Фігура потрібного типу створюється через каталог.
    public class FigureReader : IDisposable
    {
        // Потік для читання тексту з файлу
        private readonly StreamReader _reader;

        // Каталог фігур — знає, як створити фігуру за її типом
        private readonly FigureCatalog _catalog;

        // Конструктор: відкриває файл для читання.
        // Якщо файлу не існує, StreamReader кидає FileNotFoundException.
        public FigureReader(string path, FigureCatalog catalog)
        {
            _reader = new StreamReader(path);
            _catalog = catalog;
        }

        // Оператор >> читає всі фігури з файлу й додає їх до списку figures
        public static FigureReader operator >>(FigureReader reader, ICollection<Figure> figures)
        {
            // Спочатку збираємо фігури в тимчасовий список. Якщо в якомусь рядку
            // буде помилка, у список figures не потрапить жодна фігура з файлу.
            List<Figure> loaded = new List<Figure>();
            int lineNumber = 0;

            // ReadLine повертає наступний рядок файлу або null, коли файл закінчився
            string? line = reader._reader.ReadLine();
            while (line != null)
            {
                lineNumber++;

                // Порожні рядки пропускаємо
                if (line.Trim() != "")
                {
                    Figure figure = reader.ReadFigure(line, lineNumber);
                    loaded.Add(figure);
                }

                line = reader._reader.ReadLine();
            }

            // Файл прочитано без помилок — додаємо всі фігури
            foreach (Figure figure in loaded)
            {
                figures.Add(figure);
            }

            return reader;
        }

        // Закриває файл
        public void Dispose()
        {
            _reader.Dispose();
        }

        // Створює одну фігуру з одного рядка файлу.
        // У разі помилки кидає FigureFileFormatException з номером рядка.
        private Figure ReadFigure(string line, int lineNumber)
        {
            // Розбиваємо рядок на частини: "Circle;Orange;5" → ["Circle", "Orange", "5"]
            string[] parts = line.Split(';');

            // У рядку мають бути щонайменше тип, колір і один параметр
            if (parts.Length < 3)
            {
                throw new FigureFileFormatException(lineNumber,
                    $"Рядок {lineNumber}: очікується формат «тип;колір;параметри».");
            }

            //  Шукаємо тип фігури в каталозі
            string key = parts[0].Trim();
            FigureDescriptor? descriptor = _catalog.Find(key);
            if (descriptor == null)
            {
                throw new FigureFileFormatException(lineNumber,
                    $"Рядок {lineNumber}: невідомий тип фігури «{key}».");
            }

            //  Колір заливки
            string color = parts[1].Trim();

            //  Перетворюємо решту частин рядка на числа
            List<double> values = new List<double>();
            for (int i = 2; i < parts.Length; i++)
            {
                try
                {
                    double value = double.Parse(parts[i], CultureInfo.InvariantCulture);
                    values.Add(value);
                }
                catch (FormatException ex)
                {
                    throw new FigureFileFormatException(lineNumber,
                        $"Рядок {lineNumber}: значення «{parts[i]}» не є числом.", ex);
                }
            }

            //  Перевіряємо, чи чисел стільки, скільки потрібно цій фігурі
            if (values.Count != descriptor.ParameterNames.Count)
            {
                throw new FigureFileFormatException(lineNumber,
                    $"Рядок {lineNumber}: фігура «{descriptor.SubtypeName}» потребує " +
                    $"{descriptor.ParameterNames.Count} параметр(и), а знайдено {values.Count}.");
            }

            //  Створюємо фігуру. Якщо розміри неможливі
            // (наприклад, трикутник 1;2;10), конструктор фігури кине виняток.
            Figure figure;
            try
            {
                figure = descriptor.Create(values);
            }
            catch (InvalidFigureDimensionsException ex)
            {
                throw new FigureFileFormatException(lineNumber,
                    $"Рядок {lineNumber}: {ex.Message}", ex);
            }

            // Якщо колір у файлі вказано — застосовуємо його
            if (color != "")
            {
                figure.FillColor = color;
            }

            return figure;
        }
    }
}