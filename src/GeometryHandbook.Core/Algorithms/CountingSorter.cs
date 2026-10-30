using GeometryHandbook.Core.Exceptions;
using GeometryHandbook.Core.Figures;

namespace GeometryHandbook.Core.Algorithms
{
    // Сортування фігур за площею методом підрахунку (counting sort).
    // Класичний алгоритм працює лише з цілими числами, тому для кожної фігури
    // обчислюється ціле число-ключ: площа × 100, округлена до цілого
    // (точність — два знаки після коми). Самі фігури та їхні площі не змінюються.
    public static class CountingSorter
    {
        // Множник для перетворення площі на ціле число (два знаки після коми)
        private const int Scale = 100;

        // Найбільший допустимий розмір масиву лічильників.
        // Захищає від нестачі пам'яті.
        private const int MaxRange = 10_000_000;

        // Повертає новий список фігур, відсортований за зростанням площі.
        // Вхідний список не змінюється.
        public static List<Figure> SortByArea(IReadOnlyList<Figure> figures)
        {
            int length = figures.Count;

            // Порожній список або одна фігура — сортувати нічого
            if (length <= 1)
            {
                return new List<Figure>(figures);
            }

            // Ключі сортування: площа × 100, округлена до цілого
            int[] keys = new int[length];
            for (int i = 0; i < length; i++)
            {
                double scaledArea = Math.Round(figures[i].Area * Scale);

                if (scaledArea > int.MaxValue)
                {
                    throw new FigureException(
                        $"Площа фігури «{figures[i].DisplayName}» завелика для сортування підрахунком.");
                }

                keys[i] = (int)scaledArea;
            }

            // Мінімальний і максимальний ключі
            int min = keys[0];
            int max = keys[0];
            for (int i = 1; i < length; i++)
            {
                if (keys[i] < min) min = keys[i];
                if (keys[i] > max) max = keys[i];
            }

            // Розмір масиву лічильників — кількість можливих значень ключа
            int range = max - min + 1;
            if (range > MaxRange)
            {
                throw new FigureException(
                    "Площі фігур надто сильно відрізняються для сортування підрахунком.");
            }

            // Підрахунок: count[j] — скільки фігур мають ключ (min + j)
            int[] count = new int[range];
            for (int i = 0; i < length; i++)
            {
                count[keys[i] - min]++;
            }

            // Накопичені суми: тепер count[j] — скільки фігур мають ключ ≤ (min + j),
            //    тобто позиція, ПІСЛЯ якої закінчуються фігури з цим ключем
            for (int j = 1; j < range; j++)
            {
                count[j] += count[j - 1];
            }

            // Розставляємо фігури на свої позиції. Прохід з кінця робить сортування стійким:
            //    фігури з однаковою площею зберігають свій початковий порядок.
            Figure[] output = new Figure[length];
            for (int i = length - 1; i >= 0; i--)
            {
                int j = keys[i] - min;
                count[j]--;
                output[count[j]] = figures[i];
            }

            return new List<Figure>(output);
        }
    }
}