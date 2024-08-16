namespace LibraryInventoryAPI.Models.DTOs
{
    public class BookUpdateDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublicationYear { get; set; }
    }
}
