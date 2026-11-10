using System.Windows;
using System.Windows.Media;
using GeometryHandbook.Core.Figures;

namespace GeometryHandbook.App.Drawing
{
    // Перетворює фігуру з моделі на WPF-контур (Geometry) для малювання у вікні.
    // Знаходиться в проєкті App, бо Geometry — тип WPF, а модель (Core) про WPF не знає.
    public static class FigureGeometryBuilder
    {
        // Скільки пікселів на екрані відповідає одній одиниці довжини фігури.
        // Завдяки фіксованому масштабу зміна розміру фігури видна на малюнку.
        private const double PixelsPerUnit = 20;

        // Створює контур для будь-якої фігури
        public static Geometry Build(Figure figure)
        {
            // Коло — еліпс з однаковими радіусами.
            // Центр зсунутий на радіус, щоб коло не виходило за верхній і лівий край.
            if (figure is Circle circle)
            {
                double radius = circle.Radius * PixelsPerUnit;
                return new EllipseGeometry(new Point(radius, radius), radius, radius);
            }

            // Многокутник — контур через його вершини
            if (figure is Polygon polygon)
            {
                return BuildPolygon(polygon.GetVertices());
            }

            // Для невідомих фігур — порожній контур
            return Geometry.Empty;
        }

        // Створює замкнений контур многокутника з його вершин
        private static Geometry BuildPolygon(IReadOnlyList<Point2D> vertices)
        {
            // Крок 1. Найменший X і найбільший Y серед вершин —
            // потрібні, щоб зсунути фігуру до лівого верхнього кута полотна
            double minX = vertices[0].X;
            double maxY = vertices[0].Y;
            for (int i = 1; i < vertices.Count; i++)
            {
                if (vertices[i].X < minX) minX = vertices[i].X;
                if (vertices[i].Y > maxY) maxY = vertices[i].Y;
            }

            // Крок 2. Перетворюємо кожну вершину на точку екрана:
            // зсуваємо, перевертаємо вісь Y (на екрані вона спрямована вниз)
            // і переводимо в пікселі
            List<Point> points = new List<Point>();
            foreach (Point2D vertex in vertices)
            {
                double x = (vertex.X - minX) * PixelsPerUnit;
                double y = (maxY - vertex.Y) * PixelsPerUnit;
                points.Add(new Point(x, y));
            }

            // Крок 3. Збираємо контур: починаємо з першої точки,
            // проводимо лінії через решту й замикаємо назад до першої
            var pathFigure = new PathFigure();
            pathFigure.StartPoint = points[0];
            pathFigure.IsClosed = true;
            pathFigure.Segments.Add(new PolyLineSegment(points.Skip(1), true));

            var geometry = new PathGeometry();
            geometry.Figures.Add(pathFigure);
            return geometry;
        }
    }
}