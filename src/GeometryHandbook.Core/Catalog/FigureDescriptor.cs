using GeometryHandbook.Core.Figures;

namespace GeometryHandbook.Core.Catalog
{
    // Опис фігури в каталозі: службовий ключ, назви типу й підтипу для меню,
    // назви параметрів для полів введення та функція створення фігури (фабрика).
    public record FigureDescriptor(
        string Key,
        string TypeName,
        string SubtypeName,
        IReadOnlyList<string> ParameterNames,
        Func<IReadOnlyList<double>, Figure> Factory)
    {
        // Створює фігуру з переданих значень параметрів.
        // Кидає ArgumentException, якщо кількість значень не збігається з кількістю параметрів.
        public Figure Create(IReadOnlyList<double> values)
        {
            if (values.Count != ParameterNames.Count)
            {
                throw new ArgumentException(
                    $"Фігура «{SubtypeName}» потребує {ParameterNames.Count} параметр(и), а отримано {values.Count}.",
                    nameof(values));
            }

            return Factory(values);
        }
      
    }
}