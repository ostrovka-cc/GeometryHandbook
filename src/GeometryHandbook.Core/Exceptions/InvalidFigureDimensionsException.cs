namespace GeometryHandbook.Core.Exceptions
{
    // Виняток, що виникає при спробі створити фігуру з некоректними розмірами:
    // від'ємний або нульовий радіус чи сторона, кут поза допустимими межами,
    // сторони, з яких неможливо скласти трикутник.
    public class InvalidFigureDimensionsException : FigureException
    {
        // Назва параметра, значення якого виявилося некоректним
        public string ParameterName { get; }
        // Конструктор з назвою некоректного параметра та повідомленням про помилку
        public InvalidFigureDimensionsException(string parameterName, string message) : base(message)
        {
            ParameterName = parameterName;
        }

        public InvalidFigureDimensionsException(string message) : base(message)
        {
        }
    }
}
