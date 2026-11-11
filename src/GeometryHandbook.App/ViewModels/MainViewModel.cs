using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using GeometryHandbook.App.Commands;
using GeometryHandbook.App.Services;
using GeometryHandbook.Core.Algorithms;
using GeometryHandbook.Core.Catalog;
using GeometryHandbook.Core.Exceptions;
using GeometryHandbook.Core.Figures;

namespace GeometryHandbook.App.ViewModels
{
    // Головна ViewModel: список фігур, вибрана фігура, команди головного вікна
    public class MainViewModel : ViewModelBase
    {
        // Каталог фігур; передається кожному вікну створення чи редагування фігури
        private readonly FigureCatalog _catalog;

        // Сервіс для відкриття вікон і повідомлень (ViewModel не працює з вікнами напряму)
        private readonly IDialogService _dialogService;

        // Поле для властивості SelectedFigure (null, якщо нічого не вибрано)
        private FigureViewModel? _selectedFigure;

        // Поле для властивості StatusMessage; при запуску показуємо «Готово»
        private string _statusMessage = "Готово";

        // Поле для властивості SelectedColor; за замовчуванням перший колір зі списку
        private string _selectedColor = "SteelBlue";

        // Поле для властивості ScaleText; 1 — початковий розмір
        private string _scaleText = "1";

        // Каталог і сервіс вікон створюються в App.OnStartup і передаються сюди
        public MainViewModel(FigureCatalog catalog, IDialogService dialogService)
        {
            _catalog = catalog;
            _dialogService = dialogService;

            // Кожна команда знає, який метод виконати; деякі — ще й коли це можна робити
            AddFigureCommand = new RelayCommand(AddFigure);
            EditFigureCommand = new RelayCommand(EditFigure, HasSelectedFigure);
            DeleteFigureCommand = new RelayCommand(DeleteFigure, HasSelectedFigure);
            FillFigureCommand = new RelayCommand(FillFigure, HasSelectedFigure);
            ScaleFigureCommand = new RelayCommand(ScaleFigure, HasSelectedFigure);
            SortFiguresCommand = new RelayCommand(SortFigures, HasSeveralFigures);

            // Фільтр завжди активний: для порожнього списку показуємо EmptyFigureCollectionException
            FilterByPerimeterCommand = new RelayCommand(FilterByPerimeter);
        }

        // Усі додані фігури (список ліворуч); ObservableCollection сама повідомляє список про зміни
        public ObservableCollection<FigureViewModel> Figures { get; } = new ObservableCollection<FigureViewModel>();

        // Фігура, вибрана у списку; її малюємо на полотні й показуємо характеристики та формули
        public FigureViewModel? SelectedFigure
        {
            get { return _selectedFigure; }
            set
            {
                _selectedFigure = value;
                OnPropertyChanged();

                // Поле масштабу показує масштаб щойно вибраної фігури
                if (_selectedFigure != null)
                {
                    ScaleText = _selectedFigure.CurrentScale.ToString("0.##");
                }
            }
        }

        // Текст у рядку стану внизу вікна
        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        // Кольори для випадного списку на панелі інструментів (назви, які розуміє WPF)
        public IReadOnlyList<string> FillColors { get; } = new List<string>
        {
            "SteelBlue", "RoyalBlue", "DodgerBlue", "LightSkyBlue", "SlateBlue", "MediumPurple",
            "Orchid", "HotPink", "Crimson", "IndianRed", "Tomato", "Coral",
            "Orange", "Goldenrod", "Khaki", "OliveDrab", "SeaGreen", "MediumSeaGreen",
            "Teal", "CadetBlue", "Sienna", "Gray", "LightGray", "White"
        };

        // Колір, вибраний у випадному списку
        public string SelectedColor
        {
            get { return _selectedColor; }
            set
            {
                _selectedColor = value;
                OnPropertyChanged();
            }
        }

        // Текст масштабу з поля під полотном (перевіряємо в ScaleFigure)
        public string ScaleText
        {
            get { return _scaleText; }
            set
            {
                _scaleText = value;
                OnPropertyChanged();
            }
        }

        // Команда кнопки «Нова фігура», пункту меню й контекстного меню
        public RelayCommand AddFigureCommand { get; }

        // Команда кнопки «Редагувати», пункту меню, контекстного меню й подвійного кліку на фігурі
        public RelayCommand EditFigureCommand { get; }

        // Команда кнопки «Видалити», пункту меню, контекстного меню й клавіші Delete
        public RelayCommand DeleteFigureCommand { get; }

        // Команда кнопки «Зафарбувати», пункту меню й контекстного меню
        public RelayCommand FillFigureCommand { get; }

        // Команда кнопки «Застосувати», клавіші Enter у полі масштабу, пункту меню й контекстного меню
        public RelayCommand ScaleFigureCommand { get; }

        // Команда кнопки «Сортувати» і пункту меню
        public RelayCommand SortFiguresCommand { get; }

        // Команда кнопки «Фільтр ±10%» і пункту меню
        public RelayCommand FilterByPerimeterCommand { get; }

        // Відкриває вікно «Нова фігура» і додає створену фігуру до списку
        private void AddFigure()
        {
            // Для кожного вікна — новий редактор із чистими полями
            var editor = new FigureEditorViewModel(_catalog);

            // Показуємо вікно; повертається фігура або null, якщо натиснули «Скасувати»
            Figure? figure = _dialogService.ShowFigureEditor(editor);

            if (figure == null)
            {
                StatusMessage = "Додавання скасовано";
                return;
            }

            // Обгортаємо модель у FigureViewModel, додаємо до списку й одразу вибираємо її
            var figureViewModel = new FigureViewModel(figure);
            Figures.Add(figureViewModel);
            SelectedFigure = figureViewModel;

            StatusMessage = $"Додано: {figure.DisplayName}";
        }

        // Відкриває те саме вікно в режимі редагування й переписує розміри вибраної фігури
        private void EditFigure()
        {
            if (SelectedFigure == null)
            {
                return;
            }

            // Редактор із заповненими полями й заблокованим типом
            var editor = new FigureEditorViewModel(_catalog);
            editor.LoadFigure(SelectedFigure.Model);

            // Вікно повертає нову фігуру з новими розмірами або null, якщо натиснули «Скасувати»
            Figure? edited = _dialogService.ShowFigureEditor(editor);

            if (edited == null)
            {
                StatusMessage = "Редагування скасовано";
                return;
            }

            // Стара фігура переписує розміри з нової (AssignFrom), лишаючись тим самим об'єктом у списку
            SelectedFigure.ApplyEdit(edited);

            // Після редагування масштаб знову 1
            ScaleText = "1";

            StatusMessage = $"Змінено: {SelectedFigure.Model.DisplayName}";
        }

        // Видаляє вибрану фігуру зі списку
        private void DeleteFigure()
        {
            // Додаткова перевірка для компілятора: HasSelectedFigure вже не пускає сюди без вибраної фігури
            if (SelectedFigure == null)
            {
                return;
            }

            // Назву запам'ятовуємо до видалення, бо після нього SelectedFigure стане null
            string name = SelectedFigure.Model.DisplayName;

            Figures.Remove(SelectedFigure);

            // Скидаємо вибір: полотно й таблиця очистяться самі через прив'язки
            SelectedFigure = null;

            StatusMessage = $"Видалено: {name}";
        }

        // Зафарбовує вибрану фігуру кольором, вибраним у списку
        private void FillFigure()
        {
            if (SelectedFigure == null)
            {
                return;
            }

            // Міняємо колір через FigureViewModel, а не через Model, щоб полотно отримало сповіщення
            SelectedFigure.FillColor = SelectedColor;

            StatusMessage = $"{SelectedFigure.Model.DisplayName} зафарбовано: {SelectedFigure.FillColor}";
        }

        // Встановлює вибраній фігурі масштаб із поля під полотном (відносно початкового розміру)
        private void ScaleFigure()
        {
            if (SelectedFigure == null)
            {
                return;
            }

            // Перетворюємо текст на число; кома й крапка обидві підходять
            string text = ScaleText.Replace(',', '.');
            bool isNumber = double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double scale);
            if (isNumber == false)
            {
                StatusMessage = $"Масштаб «{ScaleText}» не є числом.";
                return;
            }

            // Масштаб 0 або від'ємний не має сенсу
            if (scale <= 0)
            {
                StatusMessage = "Масштаб має бути більшим за нуль.";
                return;
            }

            try
            {
                SelectedFigure.SetScale(scale);
                StatusMessage = $"{SelectedFigure.Model.DisplayName}: масштаб × {scale}";
            }
            catch (FigureException ex)
            {
                // Наприклад, якщо після масштабування розміри стали б завеликими
                StatusMessage = ex.Message;
            }
        }

        // Сортує список фігур за площею (за зростанням) методом підрахунку
        private void SortFigures()
        {
            IReadOnlyList<Figure> sorted;
            try
            {
                sorted = CountingSorter.SortByArea(GetModels());
            }
            catch (FigureException ex)
            {
                // Наприклад, якщо площі відрізняються надто сильно для сортування підрахунком
                StatusMessage = ex.Message;
                return;
            }

            // Для кожної відсортованої моделі знаходимо її FigureViewModel (там колір і масштаб)
            var sortedViewModels = new List<FigureViewModel>();
            foreach (Figure model in sorted)
            {
                foreach (FigureViewModel item in Figures)
                {
                    if (ReferenceEquals(item.Model, model))
                    {
                        sortedViewModels.Add(item);
                        break;
                    }
                }
            }

            // Clear скидає вибір у списку, тому запам'ятовуємо вибрану фігуру, щоб повернути її після
            FigureViewModel? selected = SelectedFigure;

            // Заповнюємо список заново вже у відсортованому порядку
            Figures.Clear();
            foreach (FigureViewModel item in sortedViewModels)
            {
                Figures.Add(item);
            }

            SelectedFigure = selected;

            StatusMessage = $"Відсортовано за площею: {Figures.Count} фігур";
        }

        // Показує вікно звіту з фігурами, периметр яких у межах ±10% від середнього
        private void FilterByPerimeter()
        {
            PerimeterReportViewModel report;
            try
            {
                report = new PerimeterReportViewModel(GetModels());
            }
            catch (EmptyFigureCollectionException ex)
            {
                // Список порожній — середній периметр порахувати неможливо
                StatusMessage = ex.Message;
                _dialogService.ShowError(ex.Message);
                return;
            }

            StatusMessage = report.Summary;
            _dialogService.ShowPerimeterReport(report);
        }

        // Збирає моделі (Figure) з усіх FigureViewModel, бо алгоритми з Core працюють саме з ними
        private List<Figure> GetModels()
        {
            var models = new List<Figure>();
            foreach (FigureViewModel item in Figures)
            {
                models.Add(item.Model);
            }

            return models;
        }

        // Чи вибрана фігура у списку; від цього кнопки редагування, видалення, заливки й масштабу активні або сірі
        private bool HasSelectedFigure()
        {
            return SelectedFigure != null;
        }

        // Чи є у списку хоча б дві фігури; сортувати одну немає сенсу
        private bool HasSeveralFigures()
        {
            return Figures.Count >= 2;
        }
    }
}