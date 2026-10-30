using System.Globalization;
using GeometryHandbook.Core.Figures;

namespace GeometryHandbook.Core.Persistence
{
    // Запис фігур у текстовий файл.
    // Кожна фігура записується окремим рядком: тип;колір;параметр1;параметр2;...
    // Наприклад: Circle;LightSkyBlue;5
    public class FigureWriter : IDisposable
    {
        // Потік для запису тексту у файл
        private readonly StreamWriter _writer;

        // Конструктор: відкриває файл для запису (якщо файлу немає — створює його)
        public FigureWriter(string path)
        {
            _writer = new StreamWriter(path);
        }

        // Оператор << записує одну фігуру у файл одним рядком
        public static FigureWriter operator <<(FigureWriter writer, Figure figure)
        {
            // Починаємо рядок з типу фігури та кольору
            string line = figure.TypeKey + ";" + figure.FillColor;

            // Додаємо через ; кожен параметр фігури.
            // InvariantCulture — щоб дробові числа завжди записувалися з крапкою (12.5),
            // а не з комою (12,5), як на українській Windows
            foreach (double value in figure.GetParameters())
            {
                line += ";" + value.ToString(CultureInfo.InvariantCulture);
            }

            writer._writer.WriteLine(line);
            return writer;
        }

        // Закриває файл
        public void Dispose()
        {
            _writer.Dispose();
        }
    }
}