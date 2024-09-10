namespace LibraryAPI.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public double CostPrice { get; set; }
        public double SalePrice { get; set; }

        //relational
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
