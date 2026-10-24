namespace GeometryHandbook.Core.Exceptions
{
    // Базовий виняток предметної області «Довідник з геометрії».
    // Від нього успадковуються всі винятки, що описують очікувані помилки
    // щоб інтерфейс міг перехопити їх одним блоком catch.
    public class FigureException : Exception
    {

        // Конструктор з повідомленням про помилку
        public FigureException(string message) : base(message)
        {
        }

        // Конструктор з повідомленням і винятком-причиною
        public FigureException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }

}
