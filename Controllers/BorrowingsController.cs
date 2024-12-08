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
        public async Task<IActionResult> GetBorrowings([FromQuery] string? query,
    [FromQuery] DateTime? startDate,
    [FromQuery] DateTime? endDate,
    [FromQuery] int? pageNumber = 1,
    [FromQuery] int? pageSize = 10)
        {
            var borrowingsQuery = _dbContext.Borrowings
       .Include(b => b.Book)
       .Include(b => b.Member)
       .AsQueryable();

            // Search by query across related entities
            if (!string.IsNullOrEmpty(query))
            {
                borrowingsQuery = borrowingsQuery.Where(b =>
                    b.Book.Title.Contains(query) ||
                    b.Book.Author.Contains(query) ||
                    b.Member.Name.Contains(query) ||
                    b.Member.Email.Contains(query));
            }

            // Filter by date range
            if (startDate.HasValue)
                borrowingsQuery = borrowingsQuery.Where(b => b.BorrowedDate >= startDate.Value);
            if (endDate.HasValue)
                borrowingsQuery = borrowingsQuery.Where(b => b.BorrowedDate <= endDate.Value);

            var totalRecords = await borrowingsQuery.CountAsync();

            var borrowings = await borrowingsQuery
                .Skip((pageNumber.Value - 1) * pageSize.Value)
                .Take(pageSize.Value)
                .ToListAsync();

            var borrowingDtos = _mapper.Map<List<BorrowingDto>>(borrowings);

            return Ok(new
            {
                Data = borrowingDtos,
                Pagination = new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalRecords = totalRecords
                }
            });
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
            // Ensure foreign keys are provided and valid
            if (borrowing.MemberId == 0 || string.IsNullOrEmpty(borrowing.BookISBN))
                return BadRequest(new { Message = "MemberId and BookISBN are required." });

            // Check if the Member and Book exist in the database
            var memberExists = await _dbContext.Members.AnyAsync(m => m.Id == borrowing.MemberId);
            var bookExists = await _dbContext.Books.AnyAsync(b => b.ISBN == borrowing.BookISBN);

            if (!memberExists || !bookExists)
                return NotFound(new { Message = "The specified Member or Book does not exist." });

            // Add borrowing
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
