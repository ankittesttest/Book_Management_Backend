using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace BookManagement.Models
{
    public class Bookshelf
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public int Capacity { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
