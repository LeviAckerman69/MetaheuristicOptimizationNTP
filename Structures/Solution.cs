using System.Collections.ObjectModel;

namespace MetaheuristicOptimizationNTP.Structures;

public partial class Solution
{
    private static int SolutionCounter;

    private static readonly Random Random = new();

    public int Id { get; private set; }

    private List<int> Permutation { get; }

    public IReadOnlyList<int> PermutationView => Permutation.AsReadOnly();

    public double Fitness { get; private set; }

    private Solution()
    {
        SolutionCounter++;
        Id = SolutionCounter;
    }

    public Solution(ObservableCollection<Town> townsList, bool shuffle = false) : this()
    {
        Permutation = Enumerable.Range(0, townsList.Count).ToList();

        if (shuffle)
        {
            Permutation = Permutation.Shuffle().ToList();
        }
    }

    private Solution(List<int> currentPermutation) : this()
    {
        Permutation = new List<int>(currentPermutation);
    }

    public void Evaluate(ObservableCollection<Town> townsList)
    {
        Fitness = 0;

        for (var i = 0; i < townsList.Count; i++)
        {
            var currentTownId = Permutation[i];
            var currentTown = townsList[currentTownId];

            var nextTownId = Permutation[(i + 1) % townsList.Count];
            var nextTown = townsList[nextTownId];

            var distance = currentTown.DistanceTo(nextTown);
            Fitness += distance;
        }
    }
}