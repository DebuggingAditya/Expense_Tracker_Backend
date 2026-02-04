using DailyExpensesTracker.Models;

namespace DailyExpensesTracker.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        Task<bool> UserExistsAsync(string email);
    }
}