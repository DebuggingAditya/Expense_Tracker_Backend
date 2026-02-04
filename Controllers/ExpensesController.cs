// Controllers/ExpensesController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DailyExpensesTracker.Interfaces;
using DailyExpensesTracker.Models;

namespace DailyExpensesTracker.Controllers
{
    [ApiController]
    [Route("api/expenses")]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseRepository _expenseRepository;
        private readonly IUserRepository _userRepository;

        public ExpensesController(IExpenseRepository expenseRepository, IUserRepository userRepository)
        {
            _expenseRepository = expenseRepository;
            _userRepository = userRepository;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        [HttpGet]
        public async Task<IActionResult> GetExpenses()
        {
            var userId = GetUserId();
            var expenses = await _expenseRepository.GetByUserIdAsync(userId);
            return Ok(expenses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpense(int id)
        {
            var userId = GetUserId();

            // Check if expense belongs to user
            if (!await _expenseRepository.BelongsToUserAsync(id, userId))
                return NotFound();

            var expense = await _expenseRepository.GetByIdAsync(id);
            if (expense == null)
                return NotFound();

            return Ok(expense);
        }

        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromBody] Expense expense)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();
            expense.UserId = userId;

            var createdExpense = await _expenseRepository.CreateAsync(expense);
            return CreatedAtAction(nameof(GetExpense), new { id = createdExpense.Id }, createdExpense);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] Expense expense)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = GetUserId();

            // Check if expense belongs to user
            if (!await _expenseRepository.BelongsToUserAsync(id, userId))
                return NotFound();

            var existingExpense = await _expenseRepository.GetByIdAsync(id);
            if (existingExpense == null)
                return NotFound();

            // Update properties
            existingExpense.Description = expense.Description;
            existingExpense.Amount = expense.Amount;
            existingExpense.Category = expense.Category;
            existingExpense.Date = expense.Date;

            var updatedExpense = await _expenseRepository.UpdateAsync(existingExpense);
            return Ok(updatedExpense);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var userId = GetUserId();

            // Check if expense belongs to user
            if (!await _expenseRepository.BelongsToUserAsync(id, userId))
                return NotFound();

            var result = await _expenseRepository.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}