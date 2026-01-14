using BookManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BookManagement.Data
{
    public class BookshelfContext : DbContext
    {
        public BookshelfContext(DbContextOptions<BookshelfContext> options) : base(options)
        {
        }
        public DbSet<Bookshelf> Bookshelves { get; set; } = null!;
        public DbSet<Book> Books { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Bookshelf)
                .WithMany(s => s.Books)
                .HasForeignKey(b => b.BookshelfId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
