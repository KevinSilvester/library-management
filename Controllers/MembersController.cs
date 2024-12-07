using AutoMapper;
using library_management.Data;
using library_management.DTOs;
using library_management.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace library_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly LibraryDbContext _dbContext;
        private readonly IMapper _mapper;

        public MembersController(LibraryDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetMembers()
        {
            var members = await _dbContext.Members.Include(m => m.Borrowings).ToListAsync();
            var memberDtos = _mapper.Map<List<MemberDto>>(members);
            return Ok(memberDtos);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMember(int id)
        {
            var member = await _dbContext.Members.Include(m => m.Borrowings)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null) return NotFound();
            var memberDto = _mapper.Map<MemberDto>(member);
            return Ok(memberDto);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateMember([FromBody] Member member)
        {
            await _dbContext.Members.AddAsync(member);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member);
        }

        [Authorize]
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

        [Authorize]
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
