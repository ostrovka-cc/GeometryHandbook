using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GeometryHandbook.App.ViewModels
{
    // Базовий клас для всіх ViewModel.
    // Реалізує INotifyPropertyChanged: повідомляє інтерфейс про зміну властивостей,
    // щоб прив'язані до них елементи вікна оновилися автоматично.
    internal class ViewModelBase : INotifyPropertyChanged
    {
        // Подія, на яку підписується WPF, щоб дізнаватися про зміну властивостей
        public event PropertyChangedEventHandler? PropertyChanged;

        // Повідомляє інтерфейс, що властивість змінилась.
        // [CallerMemberName] підставляє назву властивості автоматично.
        protected void OnPropertyChanged([CallerMemberName] string? _propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(_propertyName));
            }
        }
    }
}
