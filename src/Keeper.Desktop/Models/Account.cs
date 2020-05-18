using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Keeper.Desktop.Models
{
    [Serializable]
    public class Account
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public decimal Balance { get; set; } = 0;
        [Required]
        public string Currency { get; set; } = RegionInfo.CurrentRegion.ISOCurrencySymbol;
        [Required]
        public bool Deleted { get; set; } = false;
    }
}
