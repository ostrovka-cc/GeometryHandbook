namespace GeometryHandbook.Core.Figures.Quadrilaterals
{
    // Квадрат — прямокутник з рівними сторонами.
    // Задається однією стороною; успадковує формули прямокутника
    // (зокрема радіус описаного кола) та додає радіус вписаного кола.
    public class Square : Rectangle
    {
        // Конструктор за замовчуванням: квадрат зі стороною 2
        public Square() : this(2)
        {
        }

        // Конструктор з параметром: передає сторону як ширину й висоту прямокутника
        public Square(double side) : base(side, side)
        {
        }

        // Службова назва для запису у файл
        public override string TypeKey => "Square";

        // Назва фігури для користувача
        public override string DisplayName => "Квадрат";

        // Радіус вписаного кола — половина сторони
        public double Inradius => Sides[0] / 2;

        // Параметри конструктора для запису у файл: лише довжина сторони
        public override IReadOnlyList<double> GetParameters() => [Sides[0]];

        // Характеристики квадрата: усі характеристики прямокутника
        // доповнені радіусом вписаного кола
        public override IReadOnlyList<FigureProperty> GetProperties()
        {
            var properties = new List<FigureProperty>(base.GetProperties());
            properties.Add(new FigureProperty("Радіус вписаного кола, см", Inradius));
            return properties;
        }
    }
}