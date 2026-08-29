using MetaheuristicOptimizationNTP.Database;
using MetaheuristicOptimizationNTP.Structures;
using System.Security.Cryptography;
using System.Text;

namespace MetaheuristicOptimizationNTP.Services
{
    public static class AuthenticationService
    {
        public static TspDbContext DbContext { get; } = new TspDbContext();

        private static string SaltUser(Guid userId)
        {
            var hash = SHA256.HashData(userId.ToByteArray());
            return Convert.ToHexString(hash).ToLowerInvariant();
        }

        private static string HashPassword(string password, string salt)
        {
            var saltedPassword = $"{password}{salt}";
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(saltedPassword));
            return Convert.ToHexString(hash).ToLowerInvariant();
        }

        public static User? Authenticate(string username, string password)
        {
            username = username.ToLowerInvariant();
            var user = DbContext.Users.FirstOrDefault(u => u.Name == username);
            if (user == null)
            {
                return null;
            }

            var salt = SaltUser(user.Id);
            var passwordHash = HashPassword(password, salt);

            return passwordHash == user.PasswordHash ? user : null;
        }

        public static bool Register(string username, string password)
        {
            username = username.ToLowerInvariant();
            var existingUser = DbContext.Users.FirstOrDefault(u => u.Name == username);

            if (existingUser != null)
            {
                return false;
            }

            var guid = Guid.NewGuid();
            var salt = SaltUser(guid);
            var passwordHash = HashPassword(password, salt);

            var newUser = new User
            {
                Id = guid,
                Name = username,
                PasswordHash = passwordHash
            };

            DbContext.Add(newUser);
            DbContext.SaveChanges();

            return true;
        }
    }
}
