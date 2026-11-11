using System.Collections.Generic;
using System.Collections.ObjectModel;
using GeometryHandbook.Core.Catalog;
using GeometryHandbook.Core.Exceptions;
using GeometryHandbook.Core.Figures;

namespace GeometryHandbook.App.ViewModels
{
    // Дані вікна «Нова фігура»: вибір типу й підтипу, поля параметрів і створення фігури
    public class FigureEditorViewModel : ViewModelBase
    {
        private readonly FigureCatalog _catalog;
        private string? _selectedTypeName;
        private FigureDescriptor? _selectedSubtype;
        private string _errorMessage = "";

        public FigureEditorViewModel(FigureCatalog catalog)
        {
            _catalog = catalog;

            // Одразу вибираємо перший тип, щоб вікно не відкривалось порожнім
            SelectedTypeName = TypeNames[0];
        }

        // Назви типів для першого випадного списку
        public IReadOnlyList<string> TypeNames => _catalog.TypeNames;

        // Вибраний тип («Коло», «Трикутники»...)
        public string? SelectedTypeName
        {
            get { return _selectedTypeName; }
            set
            {
                _selectedTypeName = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Subtypes));

                // Після зміни типу автоматично вибираємо його перший підтип
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

        // Підтипи вибраного типу для другого випадного списку
        public IReadOnlyList<FigureDescriptor> Subtypes
        {
            get
            {
                if (_selectedTypeName == null)
                {
                    return new List<FigureDescriptor>();
                }

                return _catalog.GetSubtypes(_selectedTypeName);
            }
        }

        // Вибраний підтип; при зміні створюємо нові поля введення
        public FigureDescriptor? SelectedSubtype
        {
            get { return _selectedSubtype; }
            set
            {
                _selectedSubtype = value;
                OnPropertyChanged();

                Parameters.Clear();

                if (_selectedSubtype != null)
                {
                    foreach (string name in _selectedSubtype.ParameterNames)
                    {
                        Parameters.Add(new ParameterInputViewModel(name));
                    }
                }

                ErrorMessage = "";
            }
        }

        // Поля введення параметрів (по одному на кожен параметр підтипу)
        public ObservableCollection<ParameterInputViewModel> Parameters { get; } = new ObservableCollection<ParameterInputViewModel>();

        // Текст помилки під полями; порожній, якщо помилки немає
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        // Фігура, створена кнопкою «Додати»; її забирає WpfDialogService після закриття вікна
        public Figure? CreatedFigure { get; private set; }

        // Створює фігуру з введених значень; кидає FigureException, якщо щось не так
        public Figure CreateFigure()
        {
            if (SelectedSubtype == null)
            {
                throw new FigureException("Оберіть тип і підтип фігури.");
            }

            // Перетворюємо текст кожного поля на число (GetValue кидає FigureException, якщо там не число)
            var values = new List<double>();
            foreach (ParameterInputViewModel parameter in Parameters)
            {
                values.Add(parameter.GetValue());
            }

            // Конструктор фігури сам перевірить, чи можна її побудувати
            return SelectedSubtype.Create(values);
        }

        // Викликається кнопкою «Додати»: true, якщо фігуру створено; інакше показуємо помилку
        public bool TryCreateFigure()
        {
            try
            {
                CreatedFigure = CreateFigure();
                ErrorMessage = "";
                return true;
            }
            catch (FigureException ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }
    }
}