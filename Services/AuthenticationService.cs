using System.Security.Cryptography;
using System.Text;
using MetaheuristicOptimizationNTP.Database;
using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.Services;

public static class AuthenticationService
{
    private static TspDbContext DbContext { get; } = new();

    private static string HashPassword(string password)
    {
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = SHA256.HashData(bytes);

        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    public static User? Authenticate(string username, string password)
    {
        username = username.ToLower();

        var user = DbContext.Users.FirstOrDefault(user => user.Name == username);

        if (user == null)
        {
            return null;
        }

        var passwordHash = HashPassword(password);

        if (user.PasswordHash != passwordHash)
        {
            return null;
        }

        return user;
    }

    public static bool Register(string username, string password)
    {
        username = username.ToLower();

        if (DbContext.Users.Any(user => user.Name == username))
        {
            return false;
        }

        var user = new User
        {
            Name = username,
            PasswordHash = HashPassword(password)
        };

        DbContext.Users.Add(user);
        DbContext.SaveChanges();

        return true;
    }
}