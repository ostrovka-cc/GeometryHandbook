namespace GeometryHandbook.Core.Figures.RegularPolygons
{
    // Правильний шестикутник — правильний многокутник із шістьма сторонами.
    // Задається лише довжиною сторони; формули успадковує від RegularPolygon.
    public class RegularHexagon : RegularPolygon
    {
        // Конструктор за замовчуванням: правильний шестикутник зі стороною 2
        public RegularHexagon() : this(2)
        {
        }

        // Конструктор з параметром: передає в RegularPolygon фіксовану кількість сторін 6
        public RegularHexagon(double side) : base(6, side)
        {
        }

        // Службова назва для запису у файл
        public override string TypeKey => "RegularHexagon";

        // Назва фігури для користувача
        public override string DisplayName => "Правильний шестикутник";

        // Параметри конструктора для запису у файл: лише довжина сторони,
        // оскільки кількість сторін шестикутника завжди дорівнює 6
        public override IReadOnlyList<double> GetParameters() => [Sides[0]];
    }
}