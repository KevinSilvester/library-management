using library_management.Data;
using library_management.Models;
using Microsoft.EntityFrameworkCore;

namespace library_management.Repositories
{
    public class BorrowingRepository : IBorrowingRepository
    {
        private readonly LibraryDbContext _dbContext;

        public BorrowingRepository(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Borrowing>> GetAllBorrowingsAsync()
        {
            return await _dbContext.Borrowings
                .Include(b => b.Book) // Include related Book
                .Include(b => b.Member) // Include related Member
                .ToListAsync();
        }

        public async Task<Borrowing> GetBorrowingByIdAsync(int id)
        {
            return await _dbContext.Borrowings
                .Include(b => b.Book)
                .Include(b => b.Member)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddBorrowingAsync(Borrowing borrowing)
        {
            await _dbContext.Borrowings.AddAsync(borrowing);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateBorrowingAsync(Borrowing borrowing)
        {
            _dbContext.Borrowings.Update(borrowing);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteBorrowingAsync(int id)
        {
            var borrowing = await GetBorrowingByIdAsync(id);
            if (borrowing != null)
            {
                _dbContext.Borrowings.Remove(borrowing);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
