using System.ComponentModel.DataAnnotations;

namespace library_management.Models;

public class Member
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public DateTime MembershipDate { get; set; }

    public ICollection<Borrowing> Borrowings { get; set; }
}
