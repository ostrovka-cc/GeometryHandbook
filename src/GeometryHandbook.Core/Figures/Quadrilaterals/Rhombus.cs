namespace GeometryHandbook.Core.Figures.Quadrilaterals
{
    // Ромб — паралелограм з рівними сторонами.
    // Задається стороною та кутом; формули площі, діагоналей і вершин
    // успадковує від класу Parallelogram. Додає радіус вписаного кола.
    public class Rhombus : Parallelogram
    {
        // Конструктор за замовчуванням: ромб зі стороною 4 і кутом 60°
        public Rhombus() : this(4, 60)
        {
        }

        // Конструктор з параметрами: передає сторону як обидві сторони паралелограма.
        // Перевірку сторони й кута виконує конструктор Parallelogram.
        public Rhombus(double side, double angle) : base(side, side, angle)
        {
        }

        // Службова назва для запису у файл
        public override string TypeKey => "Rhombus";

        // Назва фігури для користувача
        public override string DisplayName => "Ромб";

        // Радіус вписаного кола — половина висоти ромба
        public double Inradius => Height / 2;

        // Параметри конструктора для запису у файл: сторона й кут α
        public override IReadOnlyList<double> GetParameters() => [Sides[0], Angles[0]];

        // Характеристики ромба: усі характеристики паралелограма
        // доповнені радіусом вписаного кола
        public override IReadOnlyList<FigureProperty> GetProperties()
        {
            var properties = new List<FigureProperty>(base.GetProperties());
            properties.Add(new FigureProperty("Радіус вписаного кола, см", Inradius));
            return properties;
        }
    }
}