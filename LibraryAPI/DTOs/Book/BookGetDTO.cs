namespace LibraryAPI.DTOs.Book
{
    public record BookGetDTO(int id, string Title,double SalePrice, double CostPrice, string GenreName);
   
}
