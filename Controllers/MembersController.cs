using library_management.Models;
using library_management.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace library_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMemberRepository _repository;

        public MembersController(IMemberRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Members
        [HttpGet]
        public async Task<IActionResult> GetMembers()
        {
            var members = await _repository.GetAllMembersAsync();
            return Ok(members);
        }

        // GET: api/Members/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemberById(int id)
        {
            var member = await _repository.GetMemberByIdAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }

        // POST: api/Members
        [HttpPost]
        public async Task<IActionResult> CreateMember([FromBody] Member member)
        {
            await _repository.AddMemberAsync(member);
            return CreatedAtAction(nameof(GetMemberById), new { id = member.Id }, member);
        }

        // PUT: api/Members/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(int id, [FromBody] Member updatedMember)
        {
            var existingMember = await _repository.GetMemberByIdAsync(id);
            if (existingMember == null)
            {
                return NotFound();
            }

            existingMember.Name = updatedMember.Name;
            existingMember.Email = updatedMember.Email;
            existingMember.MembershipDate = updatedMember.MembershipDate;

            await _repository.UpdateMemberAsync(existingMember);
            return NoContent();
        }

        // DELETE: api/Members/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _repository.GetMemberByIdAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            await _repository.DeleteMemberAsync(id);
            return NoContent();
        }
    }
}
