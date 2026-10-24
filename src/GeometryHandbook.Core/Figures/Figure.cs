using GeometryHandbook.Core.Exceptions;

namespace GeometryHandbook.Core.Figures
{
    // Абстрактна геометрична фігура - це базовий клас ієрархії фігур.
    // Визначає спільні для всіх фігур характеристики (колір, площа, периметр)
    // та операції (масштабування, присвоєння); конкретні формули реалізують нащадки.
    public abstract class Figure
    {
        // Колір заливки фігури 
        public string FillColor { get; set; } = "LightSkyBlue";

        // Службова назва типу фігури, що записується у файл 
        public abstract string TypeKey { get; }

        // Назва фігури для відображення користувачу 
        public abstract string DisplayName { get; }

        // Площа фігури; обчислюється нащадком за формулою відповідної фігури
        public abstract double Area { get; }

        // Периметр фігури (для кола — довжина кола); обчислюється нащадком
        public abstract double Perimeter { get; }

        // Захищений конструктор за замовчуванням. 
        protected Figure()
        {
        }

        // Масштабує фігуру в задану кількість разів (factor > 1 — збільшення,
        // 0 < factor < 1 — зменшення). Спочатку перевіряє коефіцієнт, потім передає
        // масштабування конкретній фігурі через ScaleCore.
        public void Scale(double factor)
        {
            EnsurePositive(factor, nameof(factor));
            ScaleCore(factor);
        }

        // Аналог оператора присвоєння: копіює стан фігури-джерела в поточну фігуру.
        // Оператор = у C# не перевантажується і для класів копіює лише посилання,
        // тому присвоєння реалізовано методом. Джерело має бути фігурою того самого типу.
        public void AssignFrom(Figure source)
        {
            ArgumentNullException.ThrowIfNull(source);

            // Присвоєння самому собі тому копіювати нічого не потрібно
            if (ReferenceEquals(source, this))
                return;

            // GetType() повертає точний тип, тому прямокутник не можна
            // присвоїти квадрату, хоча Square успадковує Rectangle
            if (source.GetType() != GetType())
            {
                throw new ArgumentException("Неможливо присвоїти фігуру іншого типу.", nameof(source));
            }

            FillColor = source.FillColor;
            AssignCore(source);
        }

        // Повертає числа, з яких створено фігуру, у порядку параметрів конструктора
        // Використовується для запису фігури у файл.
        public abstract IReadOnlyList<double> GetParameters();

        // Повертає список характеристик фігури (назва — значення) для таблиці властивостей.
        // Кожна фігура формує власний набір, тому інтерфейс не залежить від типу фігури.
        public abstract IReadOnlyList<FigureProperty> GetProperties();

        // Повертає короткий опис фігури: назву та площу з двома знаками після коми
        // (наприклад, «Коло, S = 12,57»). Перевизначає ToString() класу object.

        public override string ToString() => $"Назва фігури: {DisplayName}, її площа {Area:F2}";

        // Перевіряє, що значення є скінченним додатним числом, інакше кидає
        // InvalidFigureDimensionsException. Повертає перевірене значення,
        // щоб його можна було одразу присвоїти полю в конструкторі нащадка.
        protected static double EnsurePositive(double value, string parameterName)
        {
            if (!double.IsFinite(value) || value <= 0)
            {
                throw new InvalidFigureDimensionsException(parameterName, $"Значення {value} неприпустиме: очікується додатне число.");
            }
            return value;
        }

        // Масштабування розмірів конкретної фігури; реалізується в кожному нащадку.
        // Викликається лише з методу Scale, тому коефіцієнт уже перевірено.
        protected abstract void ScaleCore(double factor);

        // Копіювання розмірів конкретної фігури; реалізується в кожному нащадку.
        // Викликається лише з AssignFrom, тому тип джерела вже перевірено.
        protected abstract void AssignCore(Figure source);
    }
}