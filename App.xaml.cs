using System.Windows;
using MetaheuristicOptimizationNTP.Database;
using MetaheuristicOptimizationNTP.Services;
using Microsoft.EntityFrameworkCore;

namespace MetaheuristicOptimizationNTP;

public partial class App : Application
{
    public App()
    {
        var dbContext = new TspDbContext();

        //dbContext.Database.EnsureDeleted();
        //dbContext.Database.EnsureCreated();

        dbContext.Users.ExecuteDelete();

        if (!dbContext.Users.Any())
        {
            AuthenticationService.Register("admin", "ADMIN123!");
        }
    }
}