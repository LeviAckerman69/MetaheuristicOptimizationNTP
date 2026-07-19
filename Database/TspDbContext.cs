using System;
using System.Collections.Generic;
using System.Text;
using MetaheuristicOptimizationNTP.Structures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace MetaheuristicOptimizationNTP.Database
{
    public class TspDbContext : DbContext
    {
        public DbSet<Town> Towns => Set<Town>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(DatabaseConfig.ConnectionString);
        }

    }
}
