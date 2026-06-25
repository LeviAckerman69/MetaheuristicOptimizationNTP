using MetaheuristicOptimizationNTP.Structures;
using System.Collections.ObjectModel;

namespace MetaheuristicOptimizationNTP.Genetic;

public class Population
{
    private static Random Random { get; } = new();

    public ObservableCollection<Solution> Solutions { get; } = [];

    public void Populate(List<Town> towns, int populationSize)
    {
        Solutions.Clear();

        var solutions = new List<Solution>();

        for (var i = 0; i < populationSize; i++)
        {
            var solution = new Solution(towns, true);
            solutions.Add(solution);
        }

        foreach (var solution in solutions.OrderBy(solution => solution.Fitness))
        {
            Solutions.Add(solution);
        }
    }

    public int RankPick()
    {
        var count = Solutions.Count;
        var totalWeight = count * (count + 1) / 2;

        var pick = Random.Next(totalWeight);
        for (var score = 0; score < count; score++)
        {
            if (pick < count - score)
            {
                return score;
            }

            pick -= count - score;
        }

        return 0;
    }
}