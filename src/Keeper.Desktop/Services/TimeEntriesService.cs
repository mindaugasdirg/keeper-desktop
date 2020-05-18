using Keeper.Desktop.Models;
using Keeper.Desktop.Models.Reports;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Keeper.Desktop.Services
{
    public class TimeEntriesService : ModelsService<TimeEntry>
    {
        public TimeEntriesService(DatabaseContext _context) : base(_context) { }

        public override DbSet<TimeEntry> GetDbSet(DatabaseContext context) => context.TimeEntries;

        public Activity GetStartedActivity() => context.TimeEntries.Where(t => t.StopTime.Equals(default)).Select(t => t.Activity).FirstOrDefault();

        public TimeEntry GetStartedTimeEntry() => context.TimeEntries.Where(t => t.StopTime.Equals(default)).FirstOrDefault();

        public override List<TimeEntry> GetAll() => context.TimeEntries.Include(t => t.Activity).ThenInclude(a => a.Category).ToList();

        public List<MonthsActivity> GetMonthsActivities()
        {
            var today = DateTime.Now;
            var activities = context.TimeEntries
                .Include(t => t.Activity)
                .Where(t => today.Day - t.StartTime.Day <= 30)
                .AsEnumerable()
                .GroupBy(t => t.Activity)
                .ToList();
            return activities
                .Select(g => new MonthsActivity()
                { 
                    Activity = g.Key,
                    TotalDuration = g.Sum(a => a.StopTime.Subtract(a.StartTime).TotalSeconds)
                })
                .ToList();

        }
    }
}
