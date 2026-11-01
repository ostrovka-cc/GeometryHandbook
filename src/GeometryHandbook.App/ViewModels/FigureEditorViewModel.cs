using System.Collections.ObjectModel;
using GeometryHandbook.Core.Catalog;
using GeometryHandbook.Core.Exceptions;
using GeometryHandbook.Core.Figures;

namespace GeometryHandbook.App.ViewModels
{
    // ViewModel редактора нової фігури.
    // Керує вибором типу й підтипу фігури, створює поля введення
    // для параметрів обраної фігури та створює фігуру через каталог.
    public class FigureEditorViewModel : ViewModelBase
    {
        // Каталог фігур — джерело типів, підтипів і назв параметрів
        private readonly FigureCatalog _catalog;

        // Поле для властивості SelectedTypeName
        private string? _selectedTypeName;

        // Поле для властивості SelectedSubtype
        private FigureDescriptor? _selectedSubtype;

        // Конструктор: отримує каталог і одразу обирає перший тип,
        // щоб редактор при запуску не був порожнім
        public FigureEditorViewModel(FigureCatalog catalog)
        {
            _catalog = catalog;
            SelectedTypeName = TypeNames[0];
        }

        // Назви типів фігур для першого випадаючого списку
        public IReadOnlyList<string> TypeNames => _catalog.TypeNames;

        // Обраний тип фігури (наприклад, «Трикутники»).
        // Під час зміни оновлює список підтипів і обирає перший із них.
        public string? SelectedTypeName
        {
            get { return _selectedTypeName; }
            set
            {
                _selectedTypeName = value;
                OnPropertyChanged();

                // Subtypes залежить від SelectedTypeName, тому повідомляємо інтерфейс,
                // що список підтипів теж змінився
                OnPropertyChanged(nameof(Subtypes));

                // Автоматично обираємо перший підтип нового типу
                if (Subtypes.Count > 0)
                {
                    SelectedSubtype = Subtypes[0];
                }
                else
                {
                    SelectedSubtype = null;
                }
            }
        }

        // Підтипи обраного типу для другого випадаючого списку
        public IReadOnlyList<FigureDescriptor> Subtypes
        {
            get
            {
                if (SelectedTypeName == null)
                {
                    return new List<FigureDescriptor>();
                }

                return _catalog.GetSubtypes(SelectedTypeName);
            }
        }

        // Обраний підтип фігури (наприклад, «Прямокутний»).
        // Під час зміни перебудовує поля введення під параметри цієї фігури.
        public FigureDescriptor? SelectedSubtype
        {
            get { return _selectedSubtype; }
            set
            {
                _selectedSubtype = value;
                OnPropertyChanged();

                // Прибираємо поля попередньої фігури
                Parameters.Clear();

                // Створюємо по одному полю на кожен параметр нової фігури
                if (value != null)
                {
                    foreach (string parameterName in value.ParameterNames)
                    {
                        Parameters.Add(new ParameterInputViewModel(parameterName));
                    }
                }
            }   
        }

        // Поля введення для параметрів обраної фігури
        public ObservableCollection<ParameterInputViewModel> Parameters { get; } = new();

        // Створює фігуру з введених значень.
        // Кидає FigureException, якщо підтип не обрано, поле заповнене неправильно
        // або розміри неможливі (наприклад, трикутник 1, 2, 10).
        public Figure CreateFigure()
        {
            if (SelectedSubtype == null)
            {
                throw new FigureException("Оберіть тип і підтип фігури.");
            }

            // Збираємо числа з усіх полів у порядку параметрів
            List<double> values = new List<double>();
            foreach (ParameterInputViewModel parameter in Parameters)
            {
                values.Add(parameter.GetValue());
            }

            // Каталог сам знає, яку фігуру створити з цих чисел
            return SelectedSubtype.Create(values);
        }
    }
}