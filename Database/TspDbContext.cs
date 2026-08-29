using MetaheuristicOptimizationNTP.Structures;
using Microsoft.EntityFrameworkCore;

namespace MetaheuristicOptimizationNTP.Database
{
    public class TspDbContext : DbContext
    {
        public DbSet<Town> Towns => Set<Town>();

        public DbSet<User> Users => Set<User>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(DatabaseConfig.ConnectionString);
        }

    }
}
