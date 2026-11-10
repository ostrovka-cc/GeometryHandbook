using System.Windows;
using System.Windows.Input;
using GeometryHandbook.App.ViewModels;

namespace GeometryHandbook.App.Views
{
    // Головне вікно програми. Уся логіка роботи з фігурами — у MainViewModel;
    // тут лише поведінка, що стосується вигляду: масштаб перегляду полотна.
    public partial class MainWindow : Window
    {
        // Межі масштабу перегляду: від 2% (видно фігуру 1000 см цілком) до 500%
        private const double MinZoom = 0.02;
        private const double MaxZoom = 5;

        // Нижче цього масштабу клітинка 1 см займає менше 7 пікселів на екрані,
        // тонкі лінії зливаються, тому їх ховаємо й лишаємо лише лінії через 10 см
        private const double MinorGridMinZoom = 0.35;

        // Конструктор: отримує ViewModel і встановлює її як джерело даних для прив'язок
        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        // Масштабування полотна: Ctrl + колесо миші або щипок двома пальцями на тачпаді
        // (Windows передає щипок програмам саме як Ctrl + колесо)
        private void FigureScroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            // Без Ctrl колесо працює як звичайна прокрутка
            if (Keyboard.Modifiers != ModifierKeys.Control)
            {
                return;
            }

            // Колесо від себе — збільшити на 10%, до себе — зменшити
            double factor;
            if (e.Delta > 0)
            {
                factor = 1.1;
            }
            else
            {
                factor = 1 / 1.1;
            }

            SetZoom(SheetScale.ScaleX * factor);

            // Подію оброблено — ScrollViewer не повинен ще й прокручувати
            e.Handled = true;
        }

        // Застосовує масштаб до аркуша й підлаштовує під нього сітку та контур фігури
        private void SetZoom(double zoom)
        {
            zoom = Math.Clamp(zoom, MinZoom, MaxZoom);

            SheetScale.ScaleX = zoom;
            SheetScale.ScaleY = zoom;

            // Товщина ліній ділиться на масштаб, тому на екрані вона завжди однакова
            MinorGridPen.Thickness = 2 / zoom;
            MajorGridPen.Thickness = 4 / zoom;
            FigurePath.StrokeThickness = 2 / zoom;

            // При сильному віддаленні тонка сітка ховається
            if (zoom < MinorGridMinZoom)
            {
                MinorGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                MinorGrid.Visibility = Visibility.Visible;
            }

            ZoomText.Text = $"Масштаб: {zoom:P0}";
        }
    }
}