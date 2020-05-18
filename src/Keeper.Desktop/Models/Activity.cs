using System;
using System.ComponentModel.DataAnnotations;

namespace Keeper.Desktop.Models
{
    [Serializable]
    public class Activity
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public Category Category { get; set; }
        [Required]
        public bool Deleted { get; set; } = false;
    }
}
