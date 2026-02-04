// Interfaces/IExpenseRepository.cs
using DailyExpensesTracker.Models;

namespace DailyExpensesTracker.Interfaces
{
    public interface IExpenseRepository
    {
        Task<IEnumerable<Expense>> GetByUserIdAsync(int userId);
        Task<Expense> GetByIdAsync(int id);
        Task<Expense> CreateAsync(Expense expense);
        Task<Expense> UpdateAsync(Expense expense);
        Task<bool> DeleteAsync(int id);
        Task<bool> BelongsToUserAsync(int expenseId, int userId);
    }
}