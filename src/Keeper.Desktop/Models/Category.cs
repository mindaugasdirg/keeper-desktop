using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Keeper.Desktop.Models
{
    [Serializable]
    public class Category
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Scope CategoryScope { get; set; } = Scope.All;
        [Required]
        public bool Deleted { get; set; } = false;

        public enum Scope
        {
            Activity,
            Transaction,
            All
        }
    }
}
