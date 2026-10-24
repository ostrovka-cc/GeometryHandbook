namespace GeometryHandbook.Core.Exceptions
{
    // Виняток, що виникає при спробі виконати операцію над порожнім списком фігур.
    public class EmptyFigureCollectionException : FigureException
    {
        // Конструктор з повідомленням про помилку
        public EmptyFigureCollectionException(string message) : base(message)
        {
        }
    }
}
