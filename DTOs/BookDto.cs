namespace library_management.DTOs
{
    public class BookDto
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int CopiesAvailable { get; set; }
        public bool IsAvailable { get; set; }
    }

}
