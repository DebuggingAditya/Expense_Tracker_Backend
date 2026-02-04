using DailyExpensesTracker.Models;

namespace DailyExpensesTracker.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(User user);
        bool VerifyPassword(string password, string passwordHash);
        string HashPassword(string password);
    }
}
