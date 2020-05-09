using System;
using System.ComponentModel.DataAnnotations;

namespace Keeper.Engine.Models
{
    public class TimeEntry : Model
    {
        [Required]
        public Activity Activity { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }
    }
}
