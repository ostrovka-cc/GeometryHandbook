using GeometryHandbook.Core.Exceptions;

namespace GeometryHandbook.Core.Figures.RegularPolygons
{
    // Правильний многокутник — многокутник, у якого всі сторони та всі кути рівні.
    // Задається кількістю сторін і довжиною сторони.
    // Базовий клас для правильного п'ятикутника та правильного шестикутника.
    public class RegularPolygon : Polygon
    {
        // Кількість сторін (не менше 3)
        private int _sideCount;

        // Довжина сторони
        private double _side;

        // Конструктор з параметрами: створює правильний многокутник із заданою кількістю сторін
        // і довжиною сторони. Кидає InvalidFigureDimensionsException, якщо сторін менше 3
        // або довжина сторони не є додатною.
        public RegularPolygon(int sideCount, double side)
        {
            if (sideCount < 3)
            {
                throw new InvalidFigureDimensionsException(nameof(sideCount),
                    $"Многокутник має містити щонайменше 3 сторони, а задано {sideCount}.");
            }

            _sideCount = sideCount;
            _side = EnsurePositive(side, nameof(side));
        }

        // Конструктор за замовчуванням: правильний п'ятикутник зі стороною 2
        public RegularPolygon() : this(5, 2)
        {
        }

        // Кількість сторін
        public int SideCount => _sideCount;

        // Внутрішній кут у градусах: α = (n − 2) · 180° / n.
        // 180.0 замість 180 потрібне, щоб ділення було дробовим, а не цілочисельним.
        public double InteriorAngle => (_sideCount - 2) * 180.0 / _sideCount;

        // Радіус вписаного кола: r = a / (2 · tg(π / n))
        public double Inradius => _side / (2 * Math.Tan(Math.PI / _sideCount));

        // Радіус описаного кола: R = a / (2 · sin(π / n))
        public double Circumradius => _side / (2 * Math.Sin(Math.PI / _sideCount));

        // Сторони: n однакових значень a
        public override IReadOnlyList<double> Sides => Enumerable.Repeat(_side, _sideCount).ToList();

        // Кути: n однакових внутрішніх кутів
        public override IReadOnlyList<double> Angles => Enumerable.Repeat(InteriorAngle, _sideCount).ToList();

        // Службова назва для запису у файл
        public override string TypeKey => "RegularPolygon";

        // Назва фігури для користувача з кількістю сторін 
        public override string DisplayName => $"Правильний {_sideCount}-кутник";

        // Площа правильного многокутника: S = n · a² / (4 · tg(π / n))
        public override double Area => _sideCount * _side * _side / (4 * Math.Tan(Math.PI / _sideCount));

        // Параметри конструктора для запису у файл: кількість сторін і довжина сторони
        public override IReadOnlyList<double> GetParameters() => [_sideCount, _side];

        // Характеристики правильного многокутника для таблиці властивостей
        public override IReadOnlyList<FigureProperty> GetProperties()
        {
            return new List<FigureProperty>
            {
                new FigureProperty("Кількість сторін", _sideCount),
                new FigureProperty("Сторона, см", _side),
                new FigureProperty("Внутрішній кут, °", InteriorAngle),
                new FigureProperty("Площа, см²", Area),
                new FigureProperty("Периметр, см", Perimeter),
                new FigureProperty("Радіус вписаного кола, см", Inradius),
                new FigureProperty("Радіус описаного кола, см", Circumradius),
            };
        }

        // Координати вершин: вершини лежать на описаному колі з центром у початку координат
        // через рівні кути 2π / n. Початковий кут −π/2 − π/n повертає многокутник так,
        // щоб його нижня сторона була горизонтальною.
        public override IReadOnlyList<Point2D> GetVertices()
        {
            double radius = Circumradius;
            double startAngle = -Math.PI / 2 - Math.PI / _sideCount;
            var vertices = new List<Point2D>();

            for (int i = 0; i < _sideCount; i++)
            {
                double angle = startAngle + 2 * Math.PI * i / _sideCount;
                vertices.Add(new Point2D(radius * Math.Cos(angle), radius * Math.Sin(angle)));
            }

            return vertices;
        }

        // Копіює кількість сторін і довжину сторони з іншого многокутника. 
        protected override void AssignCore(Figure source)
        {
            var other = (RegularPolygon)source;
            _sideCount = other._sideCount;
            _side = other._side;
        }

        // Масштабує многокутник: множиться лише довжина сторони,
        // кількість сторін і кути залишаються незмінними
        protected override void ScaleCore(double factor)
        {
            _side *= factor;
        }
    }
}