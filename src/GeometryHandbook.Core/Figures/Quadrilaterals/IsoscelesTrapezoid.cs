using GeometryHandbook.Core.Exceptions;

namespace GeometryHandbook.Core.Figures.Quadrilaterals
{
    // Рівнобедрена трапеція — чотирикутник з однією парою паралельних сторін (основ)
    // і рівними бічними сторонами. Задається нижньою основою, верхньою основою
    // та бічною стороною.
    public class IsoscelesTrapezoid : Quadrilateral
    {
        // Нижня основа, верхня основа та бічна сторона
        private double _bottomBase, _topBase, _leg;

        // Конструктор з параметрами: створює трапецію із заданими основами та бічною стороною.
        // Усі розміри мають бути додатними, а бічна сторона — довшою за половину різниці основ;
        // інакше кидає InvalidFigureDimensionsException.
        public IsoscelesTrapezoid(double bottomBase, double topBase, double leg)
        {
            _bottomBase = EnsurePositive(bottomBase, nameof(bottomBase));
            _topBase = EnsurePositive(topBase, nameof(topBase));
            _leg = EnsurePositive(leg, nameof(leg));

            // Якщо бічна сторона не довша за зсув основ, вона не дістає від нижньої основи
            // до верхньої: висота виходить нульовою або неіснуючою
            if (_leg <= Math.Abs(_bottomBase - _topBase) / 2)
            {
                throw new InvalidFigureDimensionsException(nameof(leg),
                    $"Бічна сторона {leg} занадто коротка для основ {bottomBase} і {topBase}: " +
                    "вона має бути більшою за половину різниці основ.");
            }
        }

        // Конструктор за замовчуванням: трапеція з основами 10 і 4 та бічною стороною 5 (висота 4)
        public IsoscelesTrapezoid() : this(10, 4, 5)
        {
        }

        // Зсув верхньої основи відносно нижньої з кожного боку: off = (a − b) / 2
        private double Offset => (_bottomBase - _topBase) / 2;

        // Висота трапеції за теоремою Піфагора: h = √(c² − off²)
        public double Height => Math.Sqrt(_leg * _leg - Offset * Offset);

        // Середня лінія трапеції — півсума основ: m = (a + b) / 2
        public double Midline => (_bottomBase + _topBase) / 2;

        // Кут при нижній основі в градусах: cos α = off / c
        public double BaseAngle => Math.Acos(Offset / _leg) * 180 / Math.PI;

        // Радіус описаного кола:
        // R = c · d / (2h), де d — діагональ
        public double Circumradius => _leg * Diagonals[0] / (2 * Height);

        // Сторони в порядку обходу A-B-C-D: нижня основа, бічна, верхня основа, бічна
        public override IReadOnlyList<double> Sides => [_bottomBase, _leg, _topBase, _leg];

        // Кути при вершинах A, B, C, D: кути при кожній основі рівні між собою
        public override IReadOnlyList<double> Angles
        {
            get
            {
                double angle = BaseAngle;
                return [angle, angle, 180 - angle, 180 - angle];
            }
        }

        // Діагоналі рівнобедреної трапеції рівні: d = √(h² + m²)
        public override IReadOnlyList<double> Diagonals
        {
            get
            {
                double diagonal = Math.Sqrt(Height * Height + Midline * Midline);
                return [diagonal, diagonal];
            }
        }

        // Службова назва для запису у файл
        public override string TypeKey => "IsoscelesTrapezoid";

        // Назва фігури для користувача
        public override string DisplayName => "Рівнобедрена трапеція";

        // Площа трапеції: S = m · h (середня лінія, помножена на висоту)
        public override double Area => Midline * Height;

        // Параметри конструктора для запису у файл: нижня основа, верхня основа, бічна сторона
        public override IReadOnlyList<double> GetParameters() => [_bottomBase, _topBase, _leg];

        // Характеристики трапеції для таблиці властивостей
        public override IReadOnlyList<FigureProperty> GetProperties()
        {
            // Кут обчислюється один раз для двох рядків таблиці
            double angle = BaseAngle;

            return new List<FigureProperty>
            {
                new FigureProperty("Нижня основа, см", _bottomBase),
                new FigureProperty("Верхня основа, см", _topBase),
                new FigureProperty("Бічна сторона, см", _leg),
                new FigureProperty("Кут при нижній основі, °", angle),
                new FigureProperty("Кут при верхній основі, °", 180 - angle),
                new FigureProperty("Площа, см²", Area),
                new FigureProperty("Периметр, см", Perimeter),
                new FigureProperty("Висота, см", Height),
                new FigureProperty("Середня лінія, см", Midline),
                new FigureProperty("Діагональ, см", Diagonals[0]),
                new FigureProperty("Радіус описаного кола, см", Circumradius),
            };
        }

        // Координати вершин у порядку обходу A, B, C, D.
        // Нижня основа лежить на осі X, верхня — на висоті h і зсунута на off з кожного боку.
        public override IReadOnlyList<Point2D> GetVertices()
        {
            double offset = Offset;
            double height = Height;

            return
            [
                new Point2D(0, 0),
                new Point2D(_bottomBase, 0),
                new Point2D(_bottomBase - offset, height),
                new Point2D(offset, height),
            ];
        }

        // Копіює розміри з іншої трапеції. .
        protected override void AssignCore(Figure source)
        {
            var other = (IsoscelesTrapezoid)source;
            _bottomBase = other._bottomBase;
            _topBase = other._topBase;
            _leg = other._leg;
        }

        // Масштабує трапецію: множаться всі три розміри, кути не змінюються
        protected override void ScaleCore(double factor)
        {
            _bottomBase *= factor;
            _topBase *= factor;
            _leg *= factor;
        }
    }
}