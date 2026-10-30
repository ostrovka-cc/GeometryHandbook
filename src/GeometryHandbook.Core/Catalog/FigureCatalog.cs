using GeometryHandbook.Core.Figures;
using GeometryHandbook.Core.Figures.Quadrilaterals;
using GeometryHandbook.Core.Figures.RegularPolygons;
using GeometryHandbook.Core.Figures.Triangles;

namespace GeometryHandbook.Core.Catalog
{
    // Каталог усіх фігур програми.
    // Знає типи й підтипи фігур, назви їхніх параметрів і вміє створити фігуру.
    // Використовується редактором (для списків і полів введення)
    // та FigureReader (для створення фігур із рядків файлу).
    public class FigureCatalog
    {
        // Описи фігур у порядку додавання — для показу в меню
        private readonly List<FigureDescriptor> _descriptors = new();

        // Ті самі описи, але з доступом за ключем — для швидкого пошуку під час читання файлу
        private readonly Dictionary<string, FigureDescriptor> _byKey = new();

        // Конструктор: додає в каталог усі фігури програми.
        // Кожен опис містить:
        //   1) ключ — службова назва для файлу;
        //   2) назву типу — для першого списку в редакторі;
        //   3) назву підтипу — для другого списку;
        //   4) назви параметрів — підписи полів введення;
        //   5) функцію створення: v — список введених чисел, v[0] — перше, v[1] — друге і т.д.
        public FigureCatalog()
        {
            // Коло

            Register(new FigureDescriptor(
                "Circle",
                "Коло",
                "Коло",
                ["Радіус"],
                v => new Circle(v[0])));

            //Трикутники

            Register(new FigureDescriptor(
                "Triangle",
                "Трикутники",
                "Довільний",
                ["Сторона a", "Сторона b", "Сторона c"],
                v => new Triangle(v[0], v[1], v[2])));

            Register(new FigureDescriptor(
                "EquilateralTriangle",
                "Трикутники",
                "Рівносторонній",
                ["Сторона"],
                v => new EquilateralTriangle(v[0])));

            Register(new FigureDescriptor(
                "IsoscelesTriangle",
                "Трикутники",
                "Рівнобедрений",
                ["Основа", "Бічна сторона"],
                v => new IsoscelesTriangle(v[0], v[1])));

            Register(new FigureDescriptor(
                "RightTriangle",
                "Трикутники",
                "Прямокутний",
                ["Катет a", "Катет b"],
                v => new RightTriangle(v[0], v[1])));

            //Чотирикутники 

            Register(new FigureDescriptor(
                "Parallelogram",
                "Чотирикутники",
                "Паралелограм",
                ["Сторона a", "Сторона b", "Кут α, °"],
                v => new Parallelogram(v[0], v[1], v[2])));

            Register(new FigureDescriptor(
                "Rectangle",
                "Чотирикутники",
                "Прямокутник",
                ["Ширина", "Висота"],
                v => new Rectangle(v[0], v[1])));

            Register(new FigureDescriptor(
                "Square",
                "Чотирикутники",
                "Квадрат",
                ["Сторона"],
                v => new Square(v[0])));

            Register(new FigureDescriptor(
                "Rhombus",
                "Чотирикутники",
                "Ромб",
                ["Сторона", "Кут α, °"],
                v => new Rhombus(v[0], v[1])));

            Register(new FigureDescriptor(
                "IsoscelesTrapezoid",
                "Чотирикутники",
                "Рівнобедрена трапеція",
                ["Нижня основа", "Верхня основа", "Бічна сторона"],
                v => new IsoscelesTrapezoid(v[0], v[1], v[2])));

            //  Правильні многокутники 

            Register(new FigureDescriptor(
                "RegularPentagon",
                "Правильні многокутники",
                "П'ятикутник",
                ["Сторона"],
                v => new RegularPentagon(v[0])));

            Register(new FigureDescriptor(
                "RegularHexagon",
                "Правильні многокутники",
                "Шестикутник",
                ["Сторона"],
                v => new RegularHexagon(v[0])));

            // Кількість сторін — ціле число, тому v[0] перетворюється з double на int
            Register(new FigureDescriptor(
                "RegularPolygon",
                "Правильні многокутники",
                "Довільна кількість сторін",
                ["Кількість сторін", "Сторона"],
                v => new RegularPolygon((int)v[0], v[1])));
        }

        // Повертає список назв типів фігур без повторів
        // («Коло», «Трикутники», «Чотирикутники», «Правильні многокутники»)
        public IReadOnlyList<string> TypeNames
        {
            get
            {
                List<string> result = new List<string>();

                foreach (FigureDescriptor descriptor in _descriptors)
                {
                    // Додаємо назву типу, лише якщо її ще немає в списку
                    if (!result.Contains(descriptor.TypeName))
                    {
                        result.Add(descriptor.TypeName);
                    }
                }

                return result;
            }
        }

        // Повертає всі підтипи заданого типу 
        public IReadOnlyList<FigureDescriptor> GetSubtypes(string typeName)
        {
            List<FigureDescriptor> result = new List<FigureDescriptor>();

            foreach (FigureDescriptor descriptor in _descriptors)
            {
                if (descriptor.TypeName == typeName)
                {
                    result.Add(descriptor);
                }
            }

            return result;
        }

        // Шукає опис фігури за ключем (наприклад, "Circle").
        // Повертає null, якщо фігури з таким ключем немає в каталозі.
        public FigureDescriptor? Find(string key)
        {
            if (_byKey.ContainsKey(key))
            {
                return _byKey[key];
            }

            return null;
        }

        // Додає опис фігури в обидві колекції:
        // у список — для показу в меню, у словник — для пошуку за ключем
        private void Register(FigureDescriptor descriptor)
        {
            _descriptors.Add(descriptor);
            _byKey.Add(descriptor.Key, descriptor);
        }
    }
}