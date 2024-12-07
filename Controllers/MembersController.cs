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
    public class MembersController : ControllerBase
    {
        private readonly LibraryDbContext _dbContext;
        private readonly IMapper _mapper;

        public MembersController(LibraryDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetMembers([FromQuery] string? query, [FromQuery] int? pageNumber = 1, [FromQuery] int? pageSize = 10)
        {
            var membersQuery = _dbContext.Members.Include(m => m.Borrowings).AsQueryable();
            if (!string.IsNullOrEmpty(query))
            {
                membersQuery = membersQuery.Where(m =>
                    m.Name.Contains(query) ||
                    m.Email.Contains(query) ||
                    m.PhoneNumber.Contains(query));
            }

            var totalRecords = await membersQuery.CountAsync();

            var members = await membersQuery
                .Skip((pageNumber.Value - 1) * pageSize.Value)
                .Take(pageSize.Value)
                .ToListAsync();
            var memberDtos = _mapper.Map<List<MemberDto>>(members);
            return Ok(new
            {
                Data = memberDtos,
                Pagination = new
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalRecords = totalRecords
                }
            });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetMember(int id)
        {
            var member = await _dbContext.Members.Include(m => m.Borrowings)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null) return NotFound();
            var memberDto = _mapper.Map<MemberDto>(member);
            return Ok(memberDto);
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
