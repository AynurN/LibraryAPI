namespace LibraryAPI.DTOs.Book
{
    public record BookCreateDTO(string Title, double SalePrice, double CostPrice, int GenreId);
    
}
