using Keeper.Desktop.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Keeper.Desktop.Services
{
    public class ActivitiesService : ModelsService<Activity>
    {
        public ActivitiesService(DatabaseContext _context) : base(_context) { }

        public override DbSet<Activity> GetDbSet(DatabaseContext context) => context.Activities;

        public override List<Activity> GetAll() => context.Activities.Include(a => a.Category).ToList();
    }
}
