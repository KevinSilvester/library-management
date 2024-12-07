using library_management.Data;
using library_management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace library_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _dbContext;

        public BooksController(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _dbContext.Books.ToListAsync();
            return Ok(books);
        }

        [HttpGet("{isbn}")]
        public async Task<IActionResult> GetBook(string isbn)
        {
            var book = await _dbContext.Books.FindAsync(isbn);
            if (book == null) return NotFound();
            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] Book book)
        {
            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBook), new { isbn = book.ISBN }, book);
        }

        [HttpPut("{isbn}")]
        public async Task<IActionResult> UpdateBook(string isbn, [FromBody] Book updatedBook)
        {
            var book = await _dbContext.Books.FindAsync(isbn);
            if (book == null) return NotFound();

            book.Title = updatedBook.Title;
            book.Author = updatedBook.Author;
            book.CopiesAvailable = updatedBook.CopiesAvailable;

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{isbn}")]
        public async Task<IActionResult> DeleteBook(string isbn)
        {
            var book = await _dbContext.Books.FindAsync(isbn);
            if (book == null) return NotFound();

            _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
