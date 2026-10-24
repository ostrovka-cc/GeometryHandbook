namespace GeometryHandbook.Core.Figures
{
    // Характеристика фігури для відображення в таблиці властивостей.
    // Список таких записів повертає метод Figure.GetProperties(), тому інтерфейс
    // показує характеристики будь-якої фігури однаково, не знаючи її типу.
    public record FigureProperty(string Name, double Value)
    { 
    }
    
}
