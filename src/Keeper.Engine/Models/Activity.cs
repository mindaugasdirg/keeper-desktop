using System.ComponentModel.DataAnnotations;

namespace Keeper.Engine.Models
{
    public class Activity : Model
    {
        [Required]
        public string Name { get; set; }
        public Category Category { get; set; }
    }
}
