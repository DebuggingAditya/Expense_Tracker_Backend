using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DailyExpensesTracker.Models
{
    public class Expense
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]   
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User? User { get; set; }  

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
