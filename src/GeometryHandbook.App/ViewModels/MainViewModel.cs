using System.Collections.ObjectModel;
using GeometryHandbook.App.Commands;
using GeometryHandbook.Core.Catalog;
using GeometryHandbook.Core.Exceptions;

namespace GeometryHandbook.App.ViewModels
{
    // Головна ViewModel вікна: зберігає список фігур, обрану фігуру,
    // редактор нової фігури, текст рядка статусу та команди,
    // до яких прив'язані кнопки й пункти меню.
    public class MainViewModel : ViewModelBase
    {
        // Поле для властивості SelectedFigure
        private FigureViewModel? _selectedFigure;

        // Поле для властивості StatusMessage
        private string _statusMessage = "Готово";

        // Конструктор: отримує каталог фігур, створює редактор і команди
        public MainViewModel(FigureCatalog catalog)
        {
            Editor = new FigureEditorViewModel(catalog);
            AddFigureCommand = new RelayCommand(AddFigure);
        }

        // Список фігур. ObservableCollection сама повідомляє ListBox
        // про додавання й видалення елементів.
        public ObservableCollection<FigureViewModel> Figures { get; } = new();

        // Фігура, обрана у списку (null — нічого не обрано).
        // ListBox записує сюди фігуру, на яку клацнув користувач.
        public FigureViewModel? SelectedFigure
        {
            get { return _selectedFigure; }
            set
            {
                _selectedFigure = value;
                OnPropertyChanged();
            }
        }

        // Редактор нової фігури: вибір типу, підтипу та введення параметрів
        public FigureEditorViewModel Editor { get; }

        // Текст у рядку статусу: результат останньої дії або повідомлення про помилку
        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        // Команда додавання фігури, заданої в редакторі
        public RelayCommand AddFigureCommand { get; }

        // Створює фігуру з даних редактора й додає її у список.
        // Якщо дані неправильні (порожнє поле, не число, неможливі розміри),
        // фігура не додається, а причина показується в рядку статусу.
        private void AddFigure()
        {
            try
            {
                var figureViewModel = new FigureViewModel(Editor.CreateFigure());
                Figures.Add(figureViewModel);
                SelectedFigure = figureViewModel;
                StatusMessage = $"Додано: {figureViewModel.Model.DisplayName}";
            }
            catch (FigureException ex)
            {
                StatusMessage = ex.Message;
            }
        }
    }
}