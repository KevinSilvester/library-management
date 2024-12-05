using library_management.Models;
using library_management.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace library_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _repository;

        public BooksController(IBookRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _repository.GetAllBooksAsync();
            return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(Book book)
        {
            await _repository.AddBookAsync(book);
            return CreatedAtAction(nameof(GetBooks), new { id = book.Id }, book);
        }
    }
}
