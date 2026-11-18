using MetaheuristicOptimizationNTP.Structures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.IO;

namespace MetaheuristicOptimizationNTP.Database
{
    public class TspDbContext : DbContext
    {
        public DbSet<Town> Towns => Set<Town>();

        public DbSet<User> Users => Set<User>();
        public DbSet<ExperimentRun> ExperimentRuns => Set<ExperimentRun>();
        public DbSet<EvolutionStep> EvolutionSteps => Set<EvolutionStep>();

        public void InitializeDatabase()
        {
            Database.OpenConnection();
            try
            {
                var connection = (SqliteConnection)Database.GetDbConnection();
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name IN ('Towns', 'Users')";
                if (Convert.ToInt32(command.ExecuteScalar()) > 0 && !Database.GetAppliedMigrations().Any())
                {
                    var backupPath = connection.DataSource + ".before-migrations.bak";
                    if (!File.Exists(backupPath))
                    {
                        using var backup = new SqliteConnection(new SqliteConnectionStringBuilder { DataSource = backupPath }.ToString());
                        backup.Open();
                        connection.BackupDatabase(backup);
                    }
                }
            }
            finally
            {
                Database.CloseConnection();
            }

            Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(DatabaseConfig.ConnectionString);
        }

    }
}
