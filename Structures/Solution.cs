namespace MetaheuristicOptimizationNTP.Structures;

public partial class Solution
{
    private static int SolutionCounter;

    private static readonly Random Random = new();

    public int Id { get; }

    public IReadOnlyList<Town> Towns { get; }

    private List<int> Permutation { get; } = [];

    public IReadOnlyList<int> PermutationView => Permutation.AsReadOnly();

    public double Fitness { get; }

    public string SolutionFormat => $"#{Id} - Fitness: {Fitness}";

    private Solution(IReadOnlyList<Town> towns)
    {
        SolutionCounter++;
        Id = SolutionCounter;
        Towns = towns;
    }

    public Solution(IReadOnlyList<Town> towns, bool shuffle = false) : this(towns)
    {
        Permutation = Enumerable.Range(0, towns.Count).ToList();

        if (shuffle)
        {
            Permutation = Permutation.Shuffle().ToList();
        }

        Fitness = Evaluate(towns);
    }

    private Solution(IReadOnlyList<Town> towns, List<int> permutation) : this(towns)
    {
        if (permutation.Distinct().Count() != towns.Count)
        {
            throw new Exception("Permutation corrupted!");
        }

        Permutation = new List<int>(permutation);
        Fitness = Evaluate(towns);
    }

    private double Evaluate(IReadOnlyList<Town> towns)
    {
        var fitness = 0.0d;

        for (var i = 0; i < towns.Count; i++)
        {
            var currentTownId = Permutation[i];
            var currentTown = towns[currentTownId];

            var nextTownId = Permutation[(i + 1) % towns.Count];
            var nextTown = towns[nextTownId];

            var distance = currentTown.DistanceTo(nextTown);
            fitness += distance;
        }

        return fitness;
    }
}