namespace LibraryAPI.Entities
{
    public class Genre:BaseEntity
    {
        public string Name { get; set; }
        //relational
        public List<Book> Books { get; set; }
    }
}
