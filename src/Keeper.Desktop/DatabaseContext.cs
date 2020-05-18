using Keeper.Desktop.Models;
using Microsoft.EntityFrameworkCore;

namespace Keeper.Desktop
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<TimeEntry> TimeEntries { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite("Data Source=db.db");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // exclude deleted items
            builder.Entity<Account>().HasQueryFilter(item => !item.Deleted);
            builder.Entity<Activity>().HasQueryFilter(item => !item.Deleted);
            builder.Entity<Category>().HasQueryFilter(item => !item.Deleted);
            builder.Entity<TimeEntry>().HasQueryFilter(item => !item.Deleted);
            builder.Entity<Transaction>().HasQueryFilter(item => !item.Deleted);
        }
    }
}
