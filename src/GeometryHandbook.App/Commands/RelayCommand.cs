using System.Windows.Input;

namespace GeometryHandbook.App.Commands
{
    // Команда для MVVM: зберігає дію (що робити) та умову (чи можна зараз це зробити).
    // До однієї команди можна прив'язати пункт меню, кнопку панелі інструментів
    // і пункт контекстного меню.
    public class RelayCommand : ICommand
    {
        // Дія, яку виконує команда
        private readonly Action _execute;

        // Умова, за якої команда доступна (null — доступна завжди)
        private readonly Func<bool>? _canExecute;

        // Конструктор: приймає дію та (необов'язково) умову доступності
        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        // WPF сам перевіряє доступність команд, коли користувач щось робить у вікні
        // (клацає, вводить текст), і вмикає або вимикає прив'язані кнопки
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // Чи можна зараз виконати команду
        public bool CanExecute(object? parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute();
        }

        // Виконує дію команди
        public void Execute(object? parameter)
        {
            _execute();
        }
    }
}