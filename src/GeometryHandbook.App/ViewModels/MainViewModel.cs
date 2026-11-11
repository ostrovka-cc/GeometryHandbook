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
        private readonly FigureCatalog _catalog;
        private readonly IDialogService _dialogService;
        private FigureViewModel? _selectedFigure;
        private string _statusMessage = "Готово";

        public MainViewModel(FigureCatalog catalog, IDialogService dialogService)
        {
            _catalog = catalog;
            _dialogService = dialogService;
            AddFigureCommand = new RelayCommand(AddFigure);
        }

        // Усі додані фігури (список ліворуч)
        public ObservableCollection<FigureViewModel> Figures { get; } = new ObservableCollection<FigureViewModel>();

        // Фігура, вибрана у списку; її малюємо й показуємо характеристики
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

        public RelayCommand AddFigureCommand { get; }

        // Відкриває вікно «Нова фігура» і додає створену фігуру до списку
        private void AddFigure()
        {
            // Для кожного вікна — новий редактор із чистими полями
            var editor = new FigureEditorViewModel(_catalog);

            Figure? figure = _dialogService.ShowFigureEditor(editor);

            if (figure == null)
            {
                StatusMessage = "Додавання скасовано";
                return;
            }

            var figureViewModel = new FigureViewModel(figure);
            Figures.Add(figureViewModel);
            SelectedFigure = figureViewModel;
            StatusMessage = $"Додано: {figure.DisplayName}";
        }
    }
}