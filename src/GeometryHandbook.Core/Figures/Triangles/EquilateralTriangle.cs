namespace GeometryHandbook.Core.Figures.Triangles
{
    // Рівносторонній трикутник — трикутник з трьома рівними сторонами.
    // Задається однією стороною; площу, кути (по 60°), радіуси й вершини
    // успадковує від класу Triangle.
    public class EquilateralTriangle : Triangle
    {
        // Конструктор за замовчуванням: рівносторонній трикутник зі стороною 1
        public EquilateralTriangle() : this(1)
        {
        }

        // Конструктор з параметром: передає одну сторону як три сторони трикутника.
        // Перевірку сторони виконує конструктор Triangle.
        public EquilateralTriangle(double side) : base(side, side, side)
        {
        }

        // Службова назва для запису у файл
        public override string TypeKey => "EquilateralTriangle";

        // Назва фігури для користувача
        public override string DisplayName => "Рівносторонній трикутник";

        // Параметри конструктора для запису у файл: лише довжина сторони,
        // оскільки рівносторонній трикутник створюється з одного числа
        public override IReadOnlyList<double> GetParameters() => [Sides[0]];
    }
}