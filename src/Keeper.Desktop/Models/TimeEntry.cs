using System;
using System.ComponentModel.DataAnnotations;

namespace Keeper.Desktop.Models
{
    [Serializable]
    public class TimeEntry
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public Activity Activity { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }
        [Required]
        public bool Deleted { get; set; } = false;
    }
}
