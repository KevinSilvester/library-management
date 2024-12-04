using library_management.Data;
using library_management.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace library_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly MongoDbContext _dbContext;

        public BooksController(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _dbContext.Books.Find(_ => true).ToListAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(string id)
        {
            var book = await _dbContext.Books.Find(b => b.Id == id).FirstOrDefaultAsync();
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        }


        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] Book book)
        {
            // If Id is null or empty, MongoDB will generate an ObjectId automatically
            if (string.IsNullOrEmpty(book.Id))
            {
                book.Id = ObjectId.GenerateNewId().ToString(); // Generate a valid ObjectId
            }

            await _dbContext.Books.InsertOneAsync(book);
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }


        // PUT: api/Books/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(string id, [FromBody] Book updatedBook)
        {
            var result = await _dbContext.Books.ReplaceOneAsync(b => b.Id == id, updatedBook);
            if (result.MatchedCount == 0)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/Books/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            var result = await _dbContext.Books.DeleteOneAsync(b => b.Id == id);
            if (result.DeletedCount == 0)
                return NotFound();

            return NoContent();
        }
    }
}
