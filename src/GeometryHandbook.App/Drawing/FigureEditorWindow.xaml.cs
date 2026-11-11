using System.Windows;
using GeometryHandbook.App.ViewModels;

namespace GeometryHandbook.App.Views.Dialogs
{
    // Вікно створення нової фігури; вся логіка — у FigureEditorViewModel
    public partial class FigureEditorWindow : Window
    {
        private readonly FigureEditorViewModel _editor;

        public FigureEditorWindow(FigureEditorViewModel editor)
        {
            InitializeComponent();
            _editor = editor;
            DataContext = editor;
        }

        // Кнопка «Додати»: закриваємо вікно лише тоді, коли фігуру вдалося створити
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (_editor.TryCreateFigure())
            {
                // DialogResult = true закриває вікно, і ShowDialog() повертає true
                DialogResult = true;
            }
        }
    }
}