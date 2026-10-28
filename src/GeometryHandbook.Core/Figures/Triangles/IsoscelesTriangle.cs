namespace GeometryHandbook.Core.Figures.Triangles
{
    // Рівнобедрений трикутник — трикутник з двома рівними бічними сторонами.
    // Задається основою та бічною стороною; формули успадковує від класу Triangle.
    public class IsoscelesTriangle : Triangle
    {
        // Конструктор за замовчуванням: рівнобедрений трикутник з основою 4 і бічними сторонами 3
        public IsoscelesTriangle() : this(4, 3)
        {
        }

        // Конструктор з параметрами: передає бічні сторони як a і b, а основу як c,
        // щоб під час малювання трикутник стояв на основі (сторона c лягає на вісь X).
        // Перевірку сторін і нерівності трикутника виконує конструктор Triangle.
        public IsoscelesTriangle(double baseSide, double leg) : base(leg, leg, baseSide)
        {
        }

        // Службова назва для запису у файл
        public override string TypeKey => "IsoscelesTriangle";

        // Назва фігури для користувача
        public override string DisplayName => "Рівнобедрений трикутник";

        // Параметри конструктора для запису у файл у порядку (основа, бічна сторона).
        // Основа зберігається як сторона c (Sides[2]), бічна сторона — як a (Sides[0]).
        public override IReadOnlyList<double> GetParameters() => [Sides[2], Sides[0]];
    }
}