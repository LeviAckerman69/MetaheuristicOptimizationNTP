using Microsoft.Data.Sqlite;
using System.IO;

namespace MetaheuristicOptimizationNTP.Database;

public static class DatabaseConfig
{
    public static string ConnectionString
    {
        get
        {
            {
                var path = Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData),
                    "MetaheuristicOptimizationNTP",
                    "tsp.db");

                var directory = Path.GetDirectoryName(path)!;
                Directory.CreateDirectory(directory);

                var connectionString = new SqliteConnectionStringBuilder
                {
                    DataSource = path
                }.ToString();

                return connectionString;
            }
        }
    }
}