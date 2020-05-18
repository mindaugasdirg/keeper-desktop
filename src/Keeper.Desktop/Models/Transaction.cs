using System;
using System.ComponentModel.DataAnnotations;

namespace Keeper.Desktop.Models
{
    [Serializable]
    public class Transaction
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Description { get; set; }
        public Category Category { get; set; }
        [Required]
        public Account Account { get; set; }
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
        [Required]
        public bool Deleted { get; set; } = false;
    }
}
