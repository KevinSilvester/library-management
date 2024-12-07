using library_management.Data;
using library_management.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace library_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly LibraryDbContext _dbContext;

        public MembersController(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetMembers()
        {
            var members = await _dbContext.Members.Include(m => m.Borrowings).ToListAsync();
            return Ok(members);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMember(int id)
        {
            var member = await _dbContext.Members.Include(m => m.Borrowings)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null) return NotFound();
            return Ok(member);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMember([FromBody] Member member)
        {
            await _dbContext.Members.AddAsync(member);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, [FromBody] Member updatedMember)
        {
            var member = await _dbContext.Members.FindAsync(id);
            if (member == null) return NotFound();

            member.Name = updatedMember.Name;
            member.Email = updatedMember.Email;
            member.PhoneNumber = updatedMember.PhoneNumber;

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _dbContext.Members.FindAsync(id);
            if (member == null) return NotFound();

            _dbContext.Members.Remove(member);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
