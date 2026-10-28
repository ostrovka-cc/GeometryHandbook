namespace GeometryHandbook.Core.Figures
{
    // Абстрактний многокутник — спільний предок трикутників, чотирикутників
    // і правильних многокутників. Додає до Figure сторони, кути й вершини
    // та реалізує периметр, однаковий для всіх многокутників.
    public abstract class Polygon : Figure
    {
        // Довжини сторін многокутника; кожен нащадок повертає свої
        public abstract IReadOnlyList<double> Sides { get; }

        // Внутрішні кути многокутника в градусах
        public abstract IReadOnlyList<double> Angles { get; }

        // Периметр многокутника — сума довжин усіх сторін.
        // Реалізовано тут один раз, тому нащадкам не потрібно обчислювати периметр самостійно.
        public override double Perimeter => Sides.Sum();

        // Координати вершин многокутника в порядку обходу; використовуються для малювання
        public abstract IReadOnlyList<Point2D> GetVertices();
    }
}