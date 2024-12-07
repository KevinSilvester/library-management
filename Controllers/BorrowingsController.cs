using library_management.Data;
using library_management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace library_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowingsController : ControllerBase
    {
        private readonly LibraryDbContext _dbContext;

        public BorrowingsController(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetBorrowings()
        {
            var borrowings = await _dbContext.Borrowings
                .Include(b => b.Book)
                .Include(b => b.Member)
                .ToListAsync();
            return Ok(borrowings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBorrowing(int id)
        {
            var borrowing = await _dbContext.Borrowings
                .Include(b => b.Book)
                .Include(b => b.Member)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (borrowing == null) return NotFound();
            return Ok(borrowing);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBorrowing([FromBody] Borrowing borrowing)
        {
            await _dbContext.Borrowings.AddAsync(borrowing);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBorrowing), new { id = borrowing.Id }, borrowing);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBorrowing(int id, [FromBody] Borrowing updatedBorrowing)
        {
            var borrowing = await _dbContext.Borrowings.FindAsync(id);
            if (borrowing == null) return NotFound();

            borrowing.BorrowedDate = updatedBorrowing.BorrowedDate;
            borrowing.ReturnedDate = updatedBorrowing.ReturnedDate;

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBorrowing(int id)
        {
            var borrowing = await _dbContext.Borrowings.FindAsync(id);
            if (borrowing == null) return NotFound();

            _dbContext.Borrowings.Remove(borrowing);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
