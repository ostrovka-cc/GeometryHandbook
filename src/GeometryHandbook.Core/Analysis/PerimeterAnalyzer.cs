using GeometryHandbook.Core.Exceptions;
using GeometryHandbook.Core.Figures;

namespace GeometryHandbook.Core.Analysis
{
    // Аналіз периметрів фігур: обчислення середнього периметра та пошук фігур,
    // периметр яких відрізняється від середнього не більше ніж на задану частку (±10%).
    public static class PerimeterAnalyzer
    {
        // Допустиме відхилення від середнього за замовчуванням — 10%
        public const double DefaultTolerance = 0.10;

        // Обчислює середній периметр фігур.
        // Кидає EmptyFigureCollectionException, якщо список порожній (ділити на нуль не можна).
        public static double GetAveragePerimeter(IReadOnlyList<Figure> figures)
        {
            if (figures.Count == 0)
            {
                throw new EmptyFigureCollectionException(
                    "Список фігур порожній: неможливо обчислити середній периметр.");
            }

            double sum = 0;
            foreach (Figure figure in figures)
            {
                sum += figure.Perimeter;
            }

            return sum / figures.Count;
        }

        // Повертає фігури, периметр яких лежить у межах [середнє − 10%; середнє + 10%]
        public static List<Figure> FindNearAverage(IReadOnlyList<Figure> figures,
                                                   double tolerance = DefaultTolerance)
        {
            double average = GetAveragePerimeter(figures);

            // Межі допустимого діапазону: наприклад, для середнього 20 — від 18 до 22
            double lower = average * (1 - tolerance);
            double upper = average * (1 + tolerance);

            List<Figure> result = new List<Figure>();
            foreach (Figure figure in figures)
            {
                if (figure.Perimeter >= lower && figure.Perimeter <= upper)
                {
                    result.Add(figure);
                }
            }

            return result;
        }
    }
}