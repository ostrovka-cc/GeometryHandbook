namespace GeometryHandbook.Core.Figures.Triangles
{
    // Прямокутний трикутник — трикутник з прямим кутом (90°).
    // Задається двома катетами; гіпотенуза обчислюється за теоремою Піфагора.
    // Формули площі, кутів, радіусів і вершин успадковує від класу Triangle.
    public class RightTriangle : Triangle
    {
        // Конструктор за замовчуванням: прямокутний трикутник з катетами 3 і 4 (гіпотенуза 5)
        public RightTriangle() : this(3, 4)
        {
        }

        // Конструктор з параметрами: передає катети як сторони a і b,
        // а гіпотенузу c = √(a² + b²) обчислює за теоремою Піфагора.
        // Перевірку додатності сторін виконує конструктор Triangle.
        public RightTriangle(double legA, double legB)
            : base(legA, legB, Math.Sqrt(legA * legA + legB * legB))
        {
        }

        // Службова назва для запису у файл
        public override string TypeKey => "RightTriangle";

        // Назва фігури для користувача
        public override string DisplayName => "Прямокутний трикутник";

        // Параметри конструктора для запису у файл: два катети в порядку (a, b).
        // Гіпотенуза не зберігається, бо обчислюється з катетів.
        public override IReadOnlyList<double> GetParameters() => [Sides[0], Sides[1]];
    }
}