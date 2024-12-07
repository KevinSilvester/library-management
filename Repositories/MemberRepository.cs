using library_management.Data;
using library_management.Models;
using Microsoft.EntityFrameworkCore;

namespace library_management.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly LibraryDbContext _dbContext;

        public MemberRepository(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Member>> GetAllMembersAsync()
        {
            return await _dbContext.Members.ToListAsync();
        }

        public async Task<Member> GetMemberByIdAsync(int id)
        {
            return await _dbContext.Members.FindAsync(id);
        }

        public async Task AddMemberAsync(Member member)
        {
            await _dbContext.Members.AddAsync(member);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateMemberAsync(Member member)
        {
            _dbContext.Members.Update(member);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteMemberAsync(int id)
        {
            var member = await GetMemberByIdAsync(id);
            if (member != null)
            {
                _dbContext.Members.Remove(member);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
