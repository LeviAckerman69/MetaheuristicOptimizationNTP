namespace MetaheuristicOptimizationNTP.Structures;

public class EvolutionStep
{
    public Guid Id { get; set; }
    public Guid ExperimentRunId { get; set; }
    public ExperimentRun? ExperimentRun { get; set; }
    public int StepNumber { get; set; }

    public int ParentAId { get; set; }
    public string ParentAPermutation { get; set; } = string.Empty;
    public double ParentAFitness { get; set; }
    public int ParentBId { get; set; }
    public string ParentBPermutation { get; set; } = string.Empty;
    public double ParentBFitness { get; set; }

    public string CrossoverAlgorithm { get; set; } = string.Empty;
    public string CrossoverPermutation { get; set; } = string.Empty;
    public double CrossoverFitness { get; set; }
    public string MutationAlgorithm { get; set; } = string.Empty;
    public string MutationPermutation { get; set; } = string.Empty;
    public double MutationFitness { get; set; }

    // Route distance is minimized, so a positive gain means a shorter child route.
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public double GainA => ParentAFitness - MutationFitness;

    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public double GainB => ParentBFitness - MutationFitness;

    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public double GainOverBetterParent => Math.Min(GainA, GainB);

    public double BestFitnessBefore { get; set; }
    public double BestFitnessAfter { get; set; }
    public bool OffspringSurvived { get; set; }

    public EvolutionStep() { }

    // Each save gets its own rows, so saving again cannot move an earlier run's log.
    public EvolutionStep(EvolutionStep other)
    {
        StepNumber = other.StepNumber;
        ParentAId = other.ParentAId;
        ParentAPermutation = other.ParentAPermutation;
        ParentAFitness = other.ParentAFitness;
        ParentBId = other.ParentBId;
        ParentBPermutation = other.ParentBPermutation;
        ParentBFitness = other.ParentBFitness;
        CrossoverAlgorithm = other.CrossoverAlgorithm;
        CrossoverPermutation = other.CrossoverPermutation;
        CrossoverFitness = other.CrossoverFitness;
        MutationAlgorithm = other.MutationAlgorithm;
        MutationPermutation = other.MutationPermutation;
        MutationFitness = other.MutationFitness;
        BestFitnessBefore = other.BestFitnessBefore;
        BestFitnessAfter = other.BestFitnessAfter;
        OffspringSurvived = other.OffspringSurvived;
    }
}
