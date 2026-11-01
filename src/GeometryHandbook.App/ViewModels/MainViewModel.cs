using System.Collections.ObjectModel;
using GeometryHandbook.App.Commands;
using GeometryHandbook.Core.Figures;

namespace GeometryHandbook.App.ViewModels
{
    // Головна ViewModel вікна: зберігає список фігур, обрану фігуру
    // та команди, до яких прив'язані кнопки й пункти меню.
    public class MainViewModel : ViewModelBase
    {
        // Поле для властивості SelectedFigure
        private FigureViewModel? _selectedFigure;

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

        // Команда додавання кола (тимчасова — для перевірки першого зрізу)
        public RelayCommand AddCircleCommand { get; }

        // Конструктор: створює команди
        public MainViewModel()
        {
            AddCircleCommand = new RelayCommand(AddCircle);
        }

        // Додає у список нове коло. Радіус залежить від кількості фігур,
        // щоб кожне наступне коло було більшим за попереднє.
        private void AddCircle()
        {
            var circle = new Circle(Figures.Count + 1);
            var figureViewModel = new FigureViewModel(circle);
            Figures.Add(figureViewModel);
        }
    }
}