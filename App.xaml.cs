using System.Windows;
using MetaheuristicOptimizationNTP.Database;
using MetaheuristicOptimizationNTP.Structures;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MetaheuristicOptimizationNTP;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        var dbContext = new TspDbContext();
        var passwordHasher = new PasswordHasher<User>();

        //dbContext.Database.EnsureDeleted();
        //dbContext.Database.EnsureCreated();

        dbContext.Users.ExecuteDelete();

        if (!dbContext.Users.Any())
        {
            var user = new User
            {
                Name = "admin"
            };

            const string password = "ADMIN123!";
            var passwordHash = passwordHasher.HashPassword(user, password);
            user.PasswordHash = passwordHash;
            dbContext.Users.Add(user);
            dbContext.SaveChanges();
        }
    }
}