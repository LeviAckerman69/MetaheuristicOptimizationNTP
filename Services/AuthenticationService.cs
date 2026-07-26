using MetaheuristicOptimizationNTP.Database;
using MetaheuristicOptimizationNTP.Structures;
using Microsoft.AspNetCore.Identity;

namespace MetaheuristicOptimizationNTP.Services;

public class AuthenticationService
{
    private static TspDbContext DbContext { get; } = new();

    private PasswordHasher<User> PasswordHasher { get; } = new();

    public User? Authenticate(string username, string password)
    {
        username = username.ToLower();

        var user = DbContext.Users.FirstOrDefault(user => user.Name == username);

        if (user == null)
        {
            return null;
        }

        var passwordHash = PasswordHasher.HashPassword(user, password);

        if (user.PasswordHash != passwordHash)
        {
            return null;
        }

        return user;
    }

    public bool Register(string username, string password)
    {
        username = username.ToLower();

        var user = DbContext.Users.FirstOrDefault(user => user.Name == username);

        if (user != null)
        {
            return false;
        }

        user = new User
        {
            Name = username
        };

        user.PasswordHash = PasswordHasher.HashPassword(user, password);

        DbContext.Users.Add(user);
        DbContext.SaveChanges();

        return true;
    }
}