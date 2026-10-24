namespace GeometryHandbook.Core.Figures
{
    // Точка на площині з координатами X і Y, що використовується
    // для опису вершин многокутників під час малювання.
    // readonly record struct: незмінне значення, яке порівнюється за координатами.
    public readonly record struct Point2D(double X, double Y)
    {
    }
}
