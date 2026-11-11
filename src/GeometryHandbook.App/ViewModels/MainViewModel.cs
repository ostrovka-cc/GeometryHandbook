using System.Collections.Generic;
using System.Collections.ObjectModel;
using GeometryHandbook.App.Commands;
using GeometryHandbook.App.Services;
using GeometryHandbook.Core.Catalog;
using GeometryHandbook.Core.Figures;

namespace GeometryHandbook.App.ViewModels
{
    // Головна ViewModel: список фігур, вибрана фігура, команди головного вікна
    public class MainViewModel : ViewModelBase
    {
        // Каталог фігур; передається кожному новому вікну «Нова фігура»
        private readonly FigureCatalog _catalog;

        // Сервіс для відкриття вікон (ViewModel не працює з вікнами напряму)
        private readonly IDialogService _dialogService;

        // Поле для властивості SelectedFigure (null, якщо нічого не вибрано)
        private FigureViewModel? _selectedFigure;

        // Поле для властивості StatusMessage; при запуску показуємо «Готово»
        private string _statusMessage = "Готово";

        // Поле для властивості SelectedColor; за замовчуванням перший колір зі списку
        private string _selectedColor = "SteelBlue";

        // Каталог і сервіс вікон створюються в App.OnStartup і передаються сюди
        public MainViewModel(FigureCatalog catalog, IDialogService dialogService)
        {
            _catalog = catalog;
            _dialogService = dialogService;

            // Кожна команда знає, який метод виконати; деякі — ще й коли це можна робити
            AddFigureCommand = new RelayCommand(AddFigure);
            DeleteFigureCommand = new RelayCommand(DeleteFigure, HasSelectedFigure);
            FillFigureCommand = new RelayCommand(FillFigure, HasSelectedFigure);
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

        // Команда кнопки «Нова фігура», пункту меню й контекстного меню
        public RelayCommand AddFigureCommand { get; }

        // Команда кнопки «Видалити», пункту меню, контекстного меню й клавіші Delete
        public RelayCommand DeleteFigureCommand { get; }

        // Команда кнопки «Зафарбувати», пункту меню й контекстного меню
        public RelayCommand FillFigureCommand { get; }

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

        // Чи вибрана фігура у списку; від цього кнопки «Видалити» й «Зафарбувати» активні або сірі
        private bool HasSelectedFigure()
        {
            return SelectedFigure != null;
        }
    }
}