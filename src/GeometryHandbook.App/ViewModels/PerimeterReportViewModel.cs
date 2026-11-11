using System.Collections.Generic;
using GeometryHandbook.Core.Analysis;
using GeometryHandbook.Core.Figures;

namespace GeometryHandbook.App.ViewModels
{
    // Дані вікна звіту «Фільтр за периметром ±10%»; усе рахується один раз у конструкторі
    public class PerimeterReportViewModel
    {
        // Кидає EmptyFigureCollectionException, якщо список фігур порожній (з PerimeterAnalyzer)
        public PerimeterReportViewModel(IReadOnlyList<Figure> figures)
        {
            AveragePerimeter = PerimeterAnalyzer.GetAveragePerimeter(figures);

            // Межі: середнє мінус 10% і середнє плюс 10%
            LowerBound = AveragePerimeter * (1 - PerimeterAnalyzer.DefaultTolerance);
            UpperBound = AveragePerimeter * (1 + PerimeterAnalyzer.DefaultTolerance);

            TotalCount = figures.Count;

            // Фігури, периметр яких потрапив у межі
            FoundFigures = PerimeterAnalyzer.FindNearAverage(figures, PerimeterAnalyzer.DefaultTolerance);
        }

        // Середній периметр усіх фігур
        public double AveragePerimeter { get; }

        // Нижня межа (−10% від середнього)
        public double LowerBound { get; }

        // Верхня межа (+10% від середнього)
        public double UpperBound { get; }

        // Скільки фігур було перевірено
        public int TotalCount { get; }

        // Фігури, що потрапили в межі (рядки таблиці)
        public IReadOnlyList<Figure> FoundFigures { get; }

        // Перший рядок у вікні
        public string AverageText => $"Середній периметр: {AveragePerimeter:0.##} см";

        // Другий рядок у вікні
        public string BoundsText => $"Межі ±10%: від {LowerBound:0.##} до {UpperBound:0.##} см";

        // Підсумок над таблицею
        public string Summary
        {
            get
            {
                if (FoundFigures.Count == 0)
                {
                    return $"Жодна з {TotalCount} фігур не потрапила в межі";
                }

                return $"Знайдено {FoundFigures.Count} з {TotalCount} фігур";
            }
        }
    }
}