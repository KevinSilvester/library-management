namespace library_management.DTOs;

public class BorrowingDto
{
    public int Id { get; set; }
    public string BookISBN { get; set; }
    public string BookTitle { get; set; } // Include Book details
    public string MemberName { get; set; } // Include Member details
    public DateTime BorrowedDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
}

