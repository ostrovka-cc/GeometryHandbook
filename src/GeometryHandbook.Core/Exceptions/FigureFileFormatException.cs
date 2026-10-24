namespace GeometryHandbook.Core.Exceptions
{
    // Виняток, що виникає при зчитуванні фігур з файлу, якщо рядок файлу
    // має неправильний формат: невідомий тип фігури, нечислове значення
    // або неправильна кількість параметрів.
    public class FigureFileFormatException : FigureException
    {
        // Номер рядка файлу (починаючи з 1), у якому знайдено помилку
        public int LineNumber { get; }
        // Конструктор з номером рядка та повідомленням про помилку
        public FigureFileFormatException(int lineNumber, string message) : base(message)
        {
            LineNumber = lineNumber;
        }
        // Конструктор з номером рядка, повідомленням і винятком-причиною
        public FigureFileFormatException(int lineNumber, string message, Exception innerException) : base(message, innerException)
        {
            LineNumber = lineNumber;
        }
    }
}
