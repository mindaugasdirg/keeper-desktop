using System;
using System.ComponentModel.DataAnnotations;

namespace Keeper.Engine.Models
{
    public class Transaction : Model
    {
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public string Description { get; set; }
        public Category Category { get; set; }
        [Required]
        public Account Account { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
