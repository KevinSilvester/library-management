using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace library_management.Models;

public class Borrowing
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int MemberId { get; set; }

    [ForeignKey(nameof(MemberId))]
    public Member Member { get; set; }

    [Required]
    public int BookId { get; set; }

    [ForeignKey(nameof(BookId))]
    public Book Book { get; set; }

    [Required]
    public DateTime BorrowedDate { get; set; }

    public DateTime? ReturnedDate { get; set; }
}
