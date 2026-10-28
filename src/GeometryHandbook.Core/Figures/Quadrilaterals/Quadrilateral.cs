namespace GeometryHandbook.Core.Figures.Quadrilaterals
{
    // Абстрактний чотирикутник — спільний предок паралелограма та рівнобедреної трапеції.
    // Додає до многокутника діагоналі, які є в кожного чотирикутника.
    public abstract class Quadrilateral : Polygon
    {
        // Довжини діагоналей чотирикутника
        public abstract IReadOnlyList<double> Diagonals { get; }
    }
}