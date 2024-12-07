using AutoMapper;
using library_management.Data;
using library_management.DTOs;
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
        private readonly IMapper _mapper;

        public BorrowingsController(LibraryDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBorrowings()
        {
            var borrowings = await _dbContext.Borrowings
                .Include(b => b.Book)
                .Include(b => b.Member)
                .ToListAsync();
            var borrowingDtos = _mapper.Map<List<BorrowingDto>>(borrowings);
            return Ok(borrowingDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBorrowing(int id)
        {
            var borrowing = await _dbContext.Borrowings
                .Include(b => b.Book)
                .Include(b => b.Member)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (borrowing == null) return NotFound();
            var borrowingDto = _mapper.Map<BorrowingDto>(borrowing);
            return Ok(borrowingDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBorrowing([FromBody] Borrowing borrowing)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
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
            if (borrowing == null)
                return NotFound(new { Message = $"Borrowing with ID {id} not found." });

            _dbContext.Borrowings.Remove(borrowing);
            await _dbContext.SaveChangesAsync();
            return Ok(new { Message = $"Borrowing with ID {id} deleted successfully." });
        }

    }
}
