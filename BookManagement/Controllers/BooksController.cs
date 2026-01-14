using BookManagement.Data;
using BookManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookshelfContext _context;

        public BooksController(BookshelfContext context)
        {
            _context = context;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await _context.Books
                .Include(b => b.Bookshelf)
                .OrderBy(b => b.BookshelfId)
                .ThenBy(b => b.Position)
                .ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Books
                .Include(b => b.Bookshelf)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // POST: api/Books
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            // Set position to end of bookshelf
            var maxPosition = await _context.Books
                .Where(b => b.BookshelfId == book.BookshelfId)
                .MaxAsync(b => (int?)b.Position) ?? 0;

            // Assign position
            book.Position = maxPosition + 1;

            // Add book to context
            _context.Books.Add(book);

            // Save changes
            await _context.SaveChangesAsync();

            // Return created book
            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
        }

        // PUT: api/Books/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            // Ensure the book ID matches
            if (id != book.Id)
            {
                return BadRequest();
            }

            // Update book details except position and bookshelf
            _context.Entry(book).State = EntityState.Modified;

            // Save changes
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Check if the book still exists
                if (!BookExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            //  Return no content
            return NoContent();
        }

        // POST: api/Books/MoveBook
        [HttpPost("MoveBook")]
        public async Task<IActionResult> MoveBook([FromBody] MoveBookRequest request)
        {
            var book = await _context.Books.FindAsync(request.BookId);
            if (book == null)
            {
                return NotFound("Book not found");
            }

            var oldBookshelfId = book.BookshelfId;
            var oldPosition = book.Position;

            // Update positions in old bookshelf
            if (oldBookshelfId == request.NewBookshelfId)
            {
                // Same bookshelf - reorder
                var books = await _context.Books
                    .Where(b => b.BookshelfId == oldBookshelfId)
                    .OrderBy(b => b.Position)
                    .ToListAsync();

                // Remove the book from its old position
                books.Remove(book);
                // Insert the book at the new position
                books.Insert(request.NewPosition - 1, book);

                // Re-assign positions
                for (int i = 0; i < books.Count; i++)
                {
                    books[i].Position = i + 1;
                }
            }
            else
            {
                // Different bookshelf
                // Update old bookshelf positions
                var oldBooks = await _context.Books
                    .Where(b => b.BookshelfId == oldBookshelfId && b.Position > oldPosition)
                    .ToListAsync();

                // Shift positions up
                foreach (var b in oldBooks)
                {
                    b.Position--;
                }

                // Update new bookshelf positions
                var newBooks = await _context.Books
                    .Where(b => b.BookshelfId == request.NewBookshelfId && b.Position >= request.NewPosition)
                    .ToListAsync();

                // Shift positions down
                foreach (var b in newBooks)
                {
                    b.Position++;
                }

                // new book position and bookshelf
                book.BookshelfId = request.NewBookshelfId;
                book.Position = request.NewPosition;
            }

            //  Save changes
            await _context.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            // Update positions of remaining books
            var remainingBooks = await _context.Books
                .Where(b => b.BookshelfId == book.BookshelfId && b.Position > book.Position)
                .ToListAsync();

            // Shift positions up
            foreach (var b in remainingBooks)
            {
                b.Position--;
            }

            // Delete the book
            _context.Books.Remove(book);

            // Save changes
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }

    // Request model for moving a book
    public class MoveBookRequest
    {
        public int BookId { get; set; }
        public int NewBookshelfId { get; set; }
        public int NewPosition { get; set; }
    }
}