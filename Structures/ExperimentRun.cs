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
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public double? ImprovementPercentage => InitialBestDistance > 0
        ? (InitialBestDistance - FinalBestDistance) / InitialBestDistance * 100
        : null;
    public string Notes { get; set; } = string.Empty;
    public string TownSnapshot { get; set; } = string.Empty;
    public byte[]? RoutePreview { get; set; }
    public List<EvolutionStep> Steps { get; set; } = [];
}
