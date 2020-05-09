using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Keeper.Engine.Models
{
    public class Account : Model
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Balance { get; set; }
        [Required]
        public string Currency { get; set; }
        public List<Transaction> Transactions { get; set; }
    }
}
