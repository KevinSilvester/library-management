using System.ComponentModel.DataAnnotations;

namespace library_management.Models;

public class Book
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [Required]
    [MaxLength(50)]
    public string Author { get; set; }

    [Required]
    [MaxLength(13)]
    public string ISBN { get; set; }

    public int CopiesAvailable { get; set; }
}
