namespace GeometryHandbook.Core.Figures.Quadrilaterals
{
    // Прямокутник — паралелограм з прямими кутами.
    // Задається шириною й висотою; формули площі, діагоналей і вершин
    // успадковує від класу Parallelogram. Додає радіус описаного кола.
    public class Rectangle : Parallelogram
    {
        // Конструктор за замовчуванням: прямокутник 4 × 3
        public Rectangle() : this(4, 3)
        {
        }

        // Конструктор з параметрами: передає ширину й висоту як сторони
        // паралелограма з кутом 90°. Перевірку сторін виконує конструктор Parallelogram.
        public Rectangle(double width, double height) : base(width, height, 90)
        {
        }

        // Службова назва для запису у файл
        public override string TypeKey => "Rectangle";

        // Назва фігури для користувача
        public override string DisplayName => "Прямокутник";

        // Радіус описаного кола — половина діагоналі (діагоналі прямокутника рівні)
        public double Circumradius => Diagonals[0] / 2;

        // Параметри конструктора для запису у файл: ширина й висота
        public override IReadOnlyList<double> GetParameters() => [Sides[0], Sides[1]];

        // Характеристики прямокутника: усі характеристики паралелограма
        // доповнені радіусом описаного кола
        public override IReadOnlyList<FigureProperty> GetProperties()
        {
            var properties = new List<FigureProperty>(base.GetProperties());
            properties.Add(new FigureProperty("Радіус описаного кола, см", Circumradius));
            return properties;
        }
    }
}