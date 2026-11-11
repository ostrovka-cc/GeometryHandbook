namespace GeometryHandbook.Core.Figures
{
    // Коло — фігура, що задається одним параметром: радіусом.
    // Реалізує абстрактні члени класу Figure: формули площі та довжини кола,
    // масштабування, присвоєння та перелік характеристик для відображення.
    public class Circle : Figure
    {
        // Радіус кола; завжди додатний, оскільки встановлюється
        // лише після перевірки через EnsurePositive
        private double _radius;

        // Конструктор з параметром: створює коло із заданим радіусом.
        // Якщо радіус не є додатним числом, кидає InvalidFigureDimensionsException.
        public Circle(double radius)
        {
            _radius = EnsurePositive(radius, nameof(radius));
        }

        // Конструктор за замовчуванням: створює коло з радіусом 1,
        // викликаючи конструктор з параметром
        public Circle() : this(1)
        {
        }

        // Радіус кола
        public double Radius => _radius;

        // Діаметр кола: d = 2r
        public double Diameter => _radius * 2;

        // Службова назва для запису у файл
        public override string TypeKey => "Circle";

        // Назва фігури для користувача
        public override string DisplayName => "Коло";

        // Площа кола: S = πr²
        public override double Area => Math.PI * _radius * _radius;

        // Довжина кола: L = 2πr
        public override double Perimeter => 2 * Math.PI * _radius;

        // Параметри конструктора для запису у файл: коло задається лише радіусом
        public override IReadOnlyList<double> GetParameters() => [_radius];

        // Характеристики кола для таблиці властивостей
        public override IReadOnlyList<FigureProperty> GetProperties()
        {
            return new List<FigureProperty>
            {
                new FigureProperty("Радіус, см", Radius),
                new FigureProperty("Діаметр, см", Diameter),
                new FigureProperty("Площа, см²", Area),
                new FigureProperty("Довжина кола, см", Perimeter),
            };
        }

        // Формули кола з підставленим радіусом
        public override IReadOnlyList<string> GetFormulas()
        {
            return new List<string>
            {
                $"Діаметр: d = 2 · r = 2 · {Radius:0.##} = {Diameter:0.##}",
                $"Площа: S = π · r² = {Math.PI:0.##} · {Radius:0.##}² = {Area:0.##}",
                $"Довжина кола: L = 2 · π · r = 2 · {Math.PI:0.##} · {Radius:0.##} = {Perimeter:0.##}"
            };
        }

        // Копіює радіус з іншого кола. Приведення типу безпечне,
        // оскільки AssignFrom уже перевірив, що source — це Circle.
        protected override void AssignCore(Figure source)
        {
            _radius = ((Circle)source)._radius;
        }

        // Масштабує коло: радіус множиться на коефіцієнт,
        // а площа й довжина кола перераховуються автоматично
        protected override void ScaleCore(double factor)
        {
            _radius *= factor;
        }
    }
}