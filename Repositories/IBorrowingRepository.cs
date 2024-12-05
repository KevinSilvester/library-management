using library_management.Models;

namespace library_management.Repositories
{
    public interface IBorrowingRepository
    {
        Task<IEnumerable<Borrowing>> GetAllBorrowingsAsync();
        Task<Borrowing> GetBorrowingByIdAsync(int id);
        Task AddBorrowingAsync(Borrowing borrowing);
        Task UpdateBorrowingAsync(Borrowing borrowing);
        Task DeleteBorrowingAsync(int id);
    }
}
