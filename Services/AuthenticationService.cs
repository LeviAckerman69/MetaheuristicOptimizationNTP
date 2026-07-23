using MetaheuristicOptimizationNTP.Database;
using MetaheuristicOptimizationNTP.Structures;
using Microsoft.AspNetCore.Identity;

namespace MetaheuristicOptimizationNTP.Services
{
    public class AuthenticationService
    {
        private PasswordHasher<User> PasswordHasher { get; set; } = new();

        public User? CurrentUser { get; private set; } = null;

        public bool Login(string username, string password)
        {
            var dbContext = new TspDbContext();
            var user = dbContext.Users.FirstOrDefault(u => u.Name == username);

            if (user == null)
            {
                return false;
            }

            var passwordHash = PasswordHasher.HashPassword(user, password);

            if (user.PasswordHash != passwordHash)
            {
                return false;
            }

            CurrentUser = user;

            return true;
        }

        public void Logout()
        {
            CurrentUser = null;
        }

    }
}
