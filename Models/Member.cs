using System.ComponentModel.DataAnnotations;

namespace library_management.Models
{
    public class Member
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        // Navigation Property
        public ICollection<Borrowing> Borrowings { get; set; }
    }
}
