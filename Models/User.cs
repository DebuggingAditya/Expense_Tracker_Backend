// Models/User.cs
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DailyExpensesTracker.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [JsonIgnore]
        public string PasswordHash { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Expense> Expenses { get; set; }
    }
}