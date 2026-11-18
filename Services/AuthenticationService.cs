using MetaheuristicOptimizationNTP.Database;
using MetaheuristicOptimizationNTP.Structures;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace MetaheuristicOptimizationNTP.Services
{
    public static class AuthenticationService
    {
        public static TspDbContext DbContext { get; } = new TspDbContext();

        private const int PepperMax = 100000;

        private static string SaltUser(Guid userId)
        {
            var hash = SHA256.HashData(userId.ToByteArray());
            return Convert.ToHexString(hash).ToLowerInvariant();
        }

        private static int GeneratedPepper()
        {
            return RandomNumberGenerator.GetInt32(0, PepperMax);
        }

        private static string HashPassword(string password, string salt, int pepper)
        {
            var passwordWithSaltAndPepper = $"{salt}{password}{pepper}";
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(passwordWithSaltAndPepper));
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
            var pepper = 0;

            for (var i = 0; i < PepperMax; i++)
            {
                var passwordHash = HashPassword(password, salt, pepper);
                if (passwordHash == user.PasswordHash)
                {
                    return user;
                }
                pepper++;
            }

            return null;
        }

        public static bool UpdateAccount(User user, string username, string newPassword)
        {
            username = username.Trim().ToLowerInvariant();
            if (DbContext.Users.Any(other => other.Id != user.Id && other.Name == username))
            {
                return false;
            }

            var oldName = user.Name;
            var oldHash = user.PasswordHash;
            user.Name = username;
            if (!string.IsNullOrEmpty(newPassword))
            {
                user.PasswordHash = HashPassword(newPassword, SaltUser(user.Id), GeneratedPepper());
            }

            try
            {
                DbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                user.Name = oldName;
                user.PasswordHash = oldHash;
                throw;
            }

            return true;
        }

        public static void DeleteAccount(User user)
        {
            DbContext.Users.Remove(user);
            try
            {
                DbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                DbContext.Entry(user).State = EntityState.Unchanged;
                throw;
            }
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
            var pepper = GeneratedPepper();
            var passwordHash = HashPassword(password, salt, pepper);

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
