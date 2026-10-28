using GeometryHandbook.Core.Exceptions;

namespace GeometryHandbook.Core.Figures.Triangles
{
    // Довільний трикутник, заданий довжинами трьох сторін.
    // Базовий клас для рівностороннього, рівнобедреного та прямокутного трикутників:
    // підтипи передають сторони в конструктор і успадковують усі формули.
    public class Triangle : Polygon
    {
        // Довжини сторін трикутника; завжди додатні й задовольняють нерівність трикутника
        private double _sideA, _sideB, _sideC;

        // Конструктор з параметрами: створює трикутник із заданими сторонами.
        // Перевіряє, що кожна сторона додатна та що з цих сторін можна побудувати
        // трикутник; інакше кидає InvalidFigureDimensionsException.
        public Triangle(double sideA, double sideB, double sideC)
        {
            _sideA = EnsurePositive(sideA, nameof(sideA));
            _sideB = EnsurePositive(sideB, nameof(sideB));
            _sideC = EnsurePositive(sideC, nameof(sideC));

            // Нерівність трикутника: кожна сторона має бути меншою за суму двох інших.
            // Якщо сторона дорівнює сумі двох інших, трикутник вироджується у відрізок.
            if (_sideA >= _sideB + _sideC || _sideB >= _sideA + _sideC || _sideC >= _sideA + _sideB)
            {
                throw new InvalidFigureDimensionsException("sides", $"Зі сторін {sideA}, {sideB} і {sideC} " +
                    "неможливо побудувати трикутник: кожна сторона має бути меншою за суму двох інших.");
            }
        }

        // Конструктор за замовчуванням: створює прямокутний трикутник зі сторонами 3, 4, 5
        public Triangle() : this(3, 4, 5)
        {
        }

        // Довжини сторін a, b, c
        public override IReadOnlyList<double> Sides => [_sideA, _sideB, _sideC];

        // Внутрішні кути в градусах: α навпроти сторони a, β навпроти b, γ навпроти c
        public override IReadOnlyList<double> Angles =>
        [
            AngleOpposite(_sideA, _sideB, _sideC),
            AngleOpposite(_sideB, _sideA, _sideC),
            AngleOpposite(_sideC, _sideA, _sideB),
        ];

        // Радіус вписаного кола: r = S / p, де p — півпериметр
        public double Inradius => Area / (Perimeter / 2);

        // Радіус описаного кола: R = abc / (4S)
        public double Circumradius => _sideA * _sideB * _sideC / (4 * Area);

        // Середні лінії трикутника, паралельні сторонам a, b, c;
        // кожна дорівнює половині сторони, якій вона паралельна
        public IReadOnlyList<double> Midlines => [_sideA / 2, _sideB / 2, _sideC / 2];

        // Службова назва для запису у файл
        public override string TypeKey => "Triangle";

        // Назва фігури для користувача
        public override string DisplayName => "Довільний трикутник";

        // Площа за формулою Герона: S = √(p(p − a)(p − b)(p − c)), де p — півпериметр
        public override double Area
        {
            get
            {
                double p = (_sideA + _sideB + _sideC) / 2;
                return Math.Sqrt(p * (p - _sideA) * (p - _sideB) * (p - _sideC));
            }
        }

        // Параметри конструктора для запису у файл: три сторони в порядку a, b, c
        public override IReadOnlyList<double> GetParameters() => [_sideA, _sideB, _sideC];

        // Характеристики трикутника для таблиці властивостей
        public override IReadOnlyList<FigureProperty> GetProperties()
        {
            // Кути й середні лінії обчислюються один раз і зберігаються в змінних,
            // щоб не рахувати весь список заново для кожного рядка таблиці
            var angles = Angles;
            var midlines = Midlines;

            return new List<FigureProperty>
            {
                new FigureProperty("Сторона a, см", _sideA),
                new FigureProperty("Сторона b, см", _sideB),
                new FigureProperty("Сторона c, см", _sideC),
                new FigureProperty("Кут α, °", angles[0]),
                new FigureProperty("Кут β, °", angles[1]),
                new FigureProperty("Кут γ, °", angles[2]),
                new FigureProperty("Площа, см²", Area),
                new FigureProperty("Периметр, см", Perimeter),
                new FigureProperty("Радіус вписаного кола, см", Inradius),
                new FigureProperty("Радіус описаного кола, см", Circumradius),
                new FigureProperty("Середня лінія | a, см", midlines[0]),
                new FigureProperty("Середня лінія | b, см", midlines[1]),
                new FigureProperty("Середня лінія | c, см", midlines[2]),
            };
        }

        // Координати вершин трикутника для малювання.
        // Вершина A — у початку координат, B — на осі X на відстані c,
        // C — на відстані b від A під кутом α до осі X.
        // Вісь Y спрямована вгору (як у математиці); у WPF вона спрямована вниз,
        // тому перевертання виконується під час малювання.
        public override IReadOnlyList<Point2D> GetVertices()
        {
            // Косинус кута α при вершині A за теоремою косинусів
            double cosAlpha = (_sideB * _sideB + _sideC * _sideC - _sideA * _sideA) / (2 * _sideB * _sideC);

            // Координати C: проекція сторони b на вісь X і висота над нею (за теоремою Піфагора).
            // Math.Max захищає від від'ємного числа під коренем через похибку округлення.
            double x = _sideB * cosAlpha;
            double y = Math.Sqrt(Math.Max(0, _sideB * _sideB - x * x));

            return
            [
                new Point2D(0, 0),
                new Point2D(_sideC, 0),
                new Point2D(x, y),
            ];
        }

        // Масштабує трикутник: усі сторони множаться на коефіцієнт,
        // тому кути не змінюються, а площа змінюється в factor² разів
        protected override void ScaleCore(double factor)
        {
            _sideA *= factor;
            _sideB *= factor;
            _sideC *= factor;
        }

        // Копіює сторони з іншого трикутника. Приведення типу безпечне,
        // оскільки AssignFrom уже перевірив, що source має той самий тип.
        protected override void AssignCore(Figure source)
        {
            var other = (Triangle)source;
            _sideA = other._sideA;
            _sideB = other._sideB;
            _sideC = other._sideC;
        }

        // Обчислює кут (у градусах) навпроти сторони opposite за теоремою косинусів:
        // cos = (side1² + side2² − opposite²) / (2 · side1 · side2)
        private static double AngleOpposite(double opposite, double side1, double side2)
        {
            double cos = (side1 * side1 + side2 * side2 - opposite * opposite) / (2 * side1 * side2);

            // Через похибку округлення значення може трохи вийти за межі [−1; 1],
            // а Math.Acos для таких чисел повертає NaN
            cos = Math.Clamp(cos, -1, 1);

            // Math.Acos повертає кут у радіанах; переводимо в градуси
            return Math.Acos(cos) * 180 / Math.PI;
        }
    }
}