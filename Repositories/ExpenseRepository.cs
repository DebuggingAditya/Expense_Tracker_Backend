// Repositories/ExpenseRepository.cs
using Microsoft.EntityFrameworkCore;
using DailyExpensesTracker.Data;
using DailyExpensesTracker.Interfaces;
using DailyExpensesTracker.Models;

namespace DailyExpensesTracker.Repositories
{
    public class ExpenseRepository : IExpenseRepository
    {
        private readonly AppDbContext _context;

        public ExpenseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Expense>> GetByUserIdAsync(int userId)
        {
            return await _context.Expenses
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.Date)
                .ThenByDescending(e => e.CreatedAt)
                .ToListAsync();
        }

        public async Task<Expense> GetByIdAsync(int id)
        {
            return await _context.Expenses.FindAsync(id);
        }

        public async Task<Expense> CreateAsync(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task<Expense> UpdateAsync(Expense expense)
        {
            expense.UpdatedAt = DateTime.UtcNow;
            _context.Expenses.Update(expense);
            await _context.SaveChangesAsync();
            return expense;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
                return false;

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BelongsToUserAsync(int expenseId, int userId)
        {
            return await _context.Expenses
                .AnyAsync(e => e.Id == expenseId && e.UserId == userId);
        }
    }
}