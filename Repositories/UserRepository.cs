using library_management.Data;
using library_management.Models;
using library_management.Repositories;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly LibraryDbContext _dbContext;

    public UserRepository(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            Console.WriteLine("User not found: {Username}", username);
            return null;
        }
        return user;

    }
}
