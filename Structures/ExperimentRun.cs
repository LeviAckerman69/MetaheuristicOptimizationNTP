using System.ComponentModel.DataAnnotations;

namespace MetaheuristicOptimizationNTP.Structures;

public class ExperimentRun
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User? User { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public DateTime StartedAt { get; set; }
    public double InitialBestDistance { get; set; }
    public double FinalBestDistance { get; set; }
    public string Notes { get; set; } = string.Empty;
}
