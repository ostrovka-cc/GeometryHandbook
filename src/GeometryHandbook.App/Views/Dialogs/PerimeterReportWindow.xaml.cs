using System.Windows;
using GeometryHandbook.App.ViewModels;

namespace GeometryHandbook.App.Views.Dialogs
{
    // Вікно звіту «Фільтр за периметром ±10%»; лише показує готові дані
    public partial class PerimeterReportWindow : Window
    {
        public PerimeterReportWindow(PerimeterReportViewModel report)
        {
            InitializeComponent();
            DataContext = report;
        }
    }
}