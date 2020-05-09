using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Keeper.Engine.Models
{
    public class Category : Model
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public Scope CategoryScope { get; set; }
        public List<Model> Memebers { get; set; }

        public enum Scope
        {
            Activity,
            Transaction,
            All
        }
    }
}
