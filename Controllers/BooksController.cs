using AutoMapper;
using library_management.Data;
using library_management.DTOs;
using library_management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace library_management.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _dbContext;
        private readonly IMapper _mapper;

        public BooksController(LibraryDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            // Start with all books
            var booksQuery = _dbContext.Books.AsQueryable();

            // Perform search against all relevant columns
            if (!string.IsNullOrEmpty(search))
            {
                booksQuery = booksQuery.Where(b =>
    b.Title.ToLower().Contains(search.ToLower()) ||
    b.Author.ToLower().Contains(search.ToLower()) ||
    b.ISBN.ToLower().Contains(search.ToLower()) ||
    b.CopiesAvailable.ToString().ToLower().Contains(search.ToLower()));

            }

            // Pagination
            var totalBooks = await booksQuery.CountAsync();
            var books = await booksQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Map to DTO
            var bookDtos = _mapper.Map<List<BookDto>>(books);

            return Ok(new
            {
                TotalBooks = totalBooks,
                Page = page,
                PageSize = pageSize,
                Books = bookDtos
            });
        }


        [HttpGet("{isbn}")]
        public async Task<IActionResult> GetBook(string isbn)
        {
            Console.WriteLine("Authenticated User: " + User.Identity?.Name);
            var book = await _dbContext.Books.FindAsync(isbn);
            if (book == null) return NotFound();
            var bookDto = _mapper.Map<BookDto>(book);

            return Ok(bookDto);
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
