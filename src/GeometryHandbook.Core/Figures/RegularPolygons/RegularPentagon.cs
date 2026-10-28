namespace GeometryHandbook.Core.Figures.RegularPolygons
{
    // Правильний п'ятикутник — правильний многокутник із п'ятьма сторонами.
    // Задається лише довжиною сторони; формули успадковує від RegularPolygon.
    public class RegularPentagon : RegularPolygon
    {
        // Конструктор за замовчуванням: правильний п'ятикутник зі стороною 2
        public RegularPentagon() : this(2)
        {
        }

        // Конструктор з параметром: передає в RegularPolygon фіксовану кількість сторін 5
        public RegularPentagon(double side) : base(5, side)
        {
        }

        // Службова назва для запису у файл
        public override string TypeKey => "RegularPentagon";

        // Назва фігури для користувача
        public override string DisplayName => "Правильний п'ятикутник";

        // Параметри конструктора для запису у файл: лише довжина сторони,
        // оскільки кількість сторін п'ятикутника завжди дорівнює 5
        public override IReadOnlyList<double> GetParameters() => [Sides[0]];
    }
}