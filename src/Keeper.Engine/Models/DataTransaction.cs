using System.ComponentModel.DataAnnotations;

namespace Keeper.Engine.Models
{
    public class DataTransaction
    {
        [Required]
        public Type Action { get; set; }
        [Required]
        public Model Data { get; set; }

        public enum Type
        {
            Create,
            Delete,
            Edit
        }
    }
}
