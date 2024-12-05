using library_management.Models;
using library_management.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace library_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowingsController : ControllerBase
    {
        private readonly IBorrowingRepository _repository;

        public BorrowingsController(IBorrowingRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Borrowings
        [HttpGet]
        public async Task<IActionResult> GetBorrowings()
        {
            var borrowings = await _repository.GetAllBorrowingsAsync();
            return Ok(borrowings);
        }

        // GET: api/Borrowings/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBorrowingById(int id)
        {
            var borrowing = await _repository.GetBorrowingByIdAsync(id);
            if (borrowing == null)
            {
                return NotFound();
            }

            return Ok(borrowing);
        }

        // POST: api/Borrowings
        [HttpPost]
        public async Task<IActionResult> CreateBorrowing([FromBody] Borrowing borrowing)
        {
            await _repository.AddBorrowingAsync(borrowing);
            return CreatedAtAction(nameof(GetBorrowingById), new { id = borrowing.Id }, borrowing);
        }

        // PUT: api/Borrowings/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBorrowing(int id, [FromBody] Borrowing updatedBorrowing)
        {
            var existingBorrowing = await _repository.GetBorrowingByIdAsync(id);
            if (existingBorrowing == null)
            {
                return NotFound();
            }

            existingBorrowing.BorrowedDate = updatedBorrowing.BorrowedDate;
            existingBorrowing.ReturnedDate = updatedBorrowing.ReturnedDate;
            existingBorrowing.BookId = updatedBorrowing.BookId;
            existingBorrowing.MemberId = updatedBorrowing.MemberId;

            await _repository.UpdateBorrowingAsync(existingBorrowing);
            return NoContent();
        }

        // DELETE: api/Borrowings/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBorrowing(int id)
        {
            var borrowing = await _repository.GetBorrowingByIdAsync(id);
            if (borrowing == null)
            {
                return NotFound();
            }

            await _repository.DeleteBorrowingAsync(id);
            return NoContent();
        }
    }
}
