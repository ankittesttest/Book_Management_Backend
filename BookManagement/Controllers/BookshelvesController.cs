using BookManagement.Data;
using BookManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookshelvesController : ControllerBase
    {
        private readonly BookshelfContext _context;

        public BookshelvesController(BookshelfContext context)
        {
            _context = context;
        }

        // GET: api/Bookshelves
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bookshelf>>> GetBookshelves()
        {
            return await _context.Bookshelves
                .Include(bs => bs.Books.OrderBy(b => b.Position))
                .ToListAsync();
        }

        // GET: api/Bookshelves/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bookshelf>> GetBookshelf(int id)
        {
            var bookshelf = await _context.Bookshelves
                .Include(bs => bs.Books.OrderBy(b => b.Position))
                .FirstOrDefaultAsync(bs => bs.Id == id);

            if (bookshelf == null)
            {
                return NotFound();
            }

            return bookshelf;
        }

        // POST: api/Bookshelves
        [HttpPost]
        public async Task<ActionResult<Bookshelf>> PostBookshelf(Bookshelf bookshelf)
        {
            _context.Bookshelves.Add(bookshelf);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookshelf), new { id = bookshelf.Id }, bookshelf);
        }

        // PUT: api/Bookshelves/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookshelf(int id, Bookshelf bookshelf)
        {
            if (id != bookshelf.Id)
            {
                return BadRequest();
            }

            _context.Entry(bookshelf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookshelfExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Bookshelves/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookshelf(int id)
        {
            var bookshelf = await _context.Bookshelves.FindAsync(id);
            if (bookshelf == null)
            {
                return NotFound();
            }

            _context.Bookshelves.Remove(bookshelf);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookshelfExists(int id)
        {
            return _context.Bookshelves.Any(e => e.Id == id);
        }
    }
}