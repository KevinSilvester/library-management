using System.ComponentModel.DataAnnotations;

namespace library_management.Models
{
    public class Book
    {
        [Key]
        [MaxLength(13)] // ISBN-13 standard
        public string ISBN { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        [MaxLength(50)]
        public string Author { get; set; }

        public int CopiesAvailable { get; set; }
    }
}
