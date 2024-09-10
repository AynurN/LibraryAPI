namespace LibraryAPI.DTOs.Book
{
    public record BookUpdateDTO(string Title, double SalePrice, double CostPrice, int GenreId);
    
}
