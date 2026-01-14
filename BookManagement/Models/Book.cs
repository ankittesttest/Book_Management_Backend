using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookManagement.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Author { get; set; }

        public int Position { get; set; }

        [ForeignKey("Bookshelf")]
        public int BookshelfId { get; set; }

        [JsonIgnore]
        public virtual Bookshelf? Bookshelf { get; set; } = null;
    }
}
