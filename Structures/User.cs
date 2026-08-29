using System.ComponentModel.DataAnnotations;

namespace MetaheuristicOptimizationNTP.Structures
{
    public class User
    {

        public Guid Id { get; set; }

        [MinLength(5), MaxLength(60)]
        public string Name { get; set; }

        [StringLength(64, MinimumLength = 64)]
        public string PasswordHash { get; set; }


    }
}
