using MetaheuristicOptimizationNTP.Structures;
using Microsoft.EntityFrameworkCore;

namespace MetaheuristicOptimizationNTP.Database
{
    public class TspDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Town> Towns => Set<Town>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(DatabaseConfig.ConnectionString);
        }

    }
}
