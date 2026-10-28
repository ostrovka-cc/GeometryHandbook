using GeometryHandbook.Core.Exceptions;

namespace GeometryHandbook.Core.Figures.Quadrilaterals
{
    // Паралелограм — чотирикутник, у якого протилежні сторони попарно паралельні й рівні.
    // Задається двома сусідніми сторонами та кутом між ними.
    // Базовий клас для прямокутника, квадрата й ромба.
    public class Parallelogram : Quadrilateral
    {
        // Сусідні сторони a і b та кут α між ними (у градусах)
        private double _sideA, _sideB, _angle;

        // Конструктор з параметрами: створює паралелограм зі сторонами a, b і кутом α між ними.
        // Сторони мають бути додатними, кут — у межах (0°; 180°);
        // інакше кидає InvalidFigureDimensionsException.
        public Parallelogram(double sideA, double sideB, double angle)
        {
            _sideA = EnsurePositive(sideA, nameof(sideA));
            _sideB = EnsurePositive(sideB, nameof(sideB));
            _angle = EnsurePositive(angle, nameof(angle));

            // При куті 180° паралелограм вироджується у відрізок
            if (angle >= 180)
            {
                throw new InvalidFigureDimensionsException(nameof(angle),
                    $"Кут {angle}° неприпустимий: кут паралелограма має бути більшим за 0° і меншим за 180°.");
            }
        }

        // Конструктор за замовчуванням: паралелограм зі сторонами 5 і 3 та кутом 60°
        public Parallelogram() : this(5, 3, 60)
        {
        }

        // Кут α у радіанах — для тригонометричних функцій Math.Sin і Math.Cos
        private double AngleRadians => _angle * Math.PI / 180;

        // Сторони в порядку обходу A→B→C→D: протилежні сторони рівні
        public override IReadOnlyList<double> Sides => [_sideA, _sideB, _sideA, _sideB];

        // Кути при вершинах A, B, C, D: протилежні кути рівні, сусідні в сумі дають 180°
        public override IReadOnlyList<double> Angles => [_angle, 180 - _angle, _angle, 180 - _angle];

        // Службова назва для запису у файл
        public override string TypeKey => "Parallelogram";

        // Назва фігури для користувача
        public override string DisplayName => "Паралелограм";

        // Площа паралелограма: S = a · b · sin α
        public override double Area
        {
            get
            {
                return _sideA * _sideB * Math.Sin(AngleRadians);
            }
        }

        // Висота, опущена на сторону a: h = b · sin α
        public double Height
        {
            get
            {
                return _sideB * Math.Sin(AngleRadians);
            }
        }

        // Діагоналі за теоремою косинусів: коротка d₁ = √(a² + b² − 2ab·cos α)
        // і довга d₂ = √(a² + b² + 2ab·cos α)
        public override IReadOnlyList<double> Diagonals =>
        [
            Math.Sqrt(_sideA * _sideA + _sideB * _sideB - 2 * _sideA * _sideB * Math.Cos(AngleRadians)),
            Math.Sqrt(_sideA * _sideA + _sideB * _sideB + 2 * _sideA * _sideB * Math.Cos(AngleRadians)),
        ];

        // Параметри конструктора для запису у файл: сторони a, b і кут α
        public override IReadOnlyList<double> GetParameters() => [_sideA, _sideB, _angle];

        // Характеристики паралелограма для таблиці властивостей
        public override IReadOnlyList<FigureProperty> GetProperties()
        {
            // Діагоналі обчислюються один раз для обох рядків таблиці
            var diagonals = Diagonals;

            return new List<FigureProperty>
            {
                new FigureProperty("Сторона a, см", _sideA),
                new FigureProperty("Сторона b, см", _sideB),
                new FigureProperty("Кут α, °", _angle),
                new FigureProperty("Кут β, °", 180 - _angle),
                new FigureProperty("Площа, см²", Area),
                new FigureProperty("Периметр, см", Perimeter),
                new FigureProperty("Висота, см", Height),
                new FigureProperty("Коротка діагональ, см", diagonals[0]),
                new FigureProperty("Довга діагональ, см", diagonals[1]),
            };
        }

        // Координати вершин у порядку обходу A, B, C, D.
        // A — у початку координат, B — на осі X; C і D зсунуті відносно B і A
        // на вектор сторони b, нахиленої під кутом α: (b·cos α; b·sin α).
        public override IReadOnlyList<Point2D> GetVertices()
        {
            double dx = _sideB * Math.Cos(AngleRadians);
            double dy = _sideB * Math.Sin(AngleRadians);

            return
            [
                new Point2D(0, 0),
                new Point2D(_sideA, 0),
                new Point2D(_sideA + dx, dy),
                new Point2D(dx, dy),
            ];
        }

        // Копіює сторони й кут з іншого паралелограма. Приведення типу безпечне,
        // оскільки AssignFrom уже перевірив, що source має той самий тип.
        protected override void AssignCore(Figure source)
        {
            var other = (Parallelogram)source;
            _sideA = other._sideA;
            _sideB = other._sideB;
            _angle = other._angle;
        }

        // Масштабує паралелограм: множаться лише сторони, а кут залишається незмінним,
        // тому фігура змінює розмір, але не форму
        protected override void ScaleCore(double factor)
        {
            _sideA *= factor;
            _sideB *= factor;
        }
    }
}