using library_management.Data;
using library_management.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace library_management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly MongoDbContext _dbContext;

        public MembersController(MongoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Members
        [HttpGet]
        public async Task<IActionResult> GetMembers()
        {
            var members = await _dbContext.Members.Find(_ => true).ToListAsync();
            return Ok(members);
        }

        // GET: api/Members/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemberById(string id)
        {
            var member = await _dbContext.Members.Find(m => m.Id == id).FirstOrDefaultAsync();
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
            await _dbContext.Members.InsertOneAsync(member);
            return CreatedAtAction(nameof(GetMemberById), new { id = member.Id }, member);
        }

        // PUT: api/Members/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMember(string id, [FromBody] Member updatedMember)
        {
            var result = await _dbContext.Members.ReplaceOneAsync(m => m.Id == id, updatedMember);
            if (result.MatchedCount == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Members/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(string id)
        {
            var result = await _dbContext.Members.DeleteOneAsync(m => m.Id == id);
            if (result.DeletedCount == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
