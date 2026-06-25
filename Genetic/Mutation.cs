using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.Genetic;

using MutationAlgorithm = Func<Solution, Solution>;

public class Mutation
{
    public static List<MutationAlgorithm> MutationAlgorithms =
    [
        SwapMutation, InsertMutation, InversionMutation, ScrambleMutation, NeutralMutation
    ];

    private static Random Random { get; } = new();

    public static Solution SwapMutation(Solution solution)
    {
        var townCount = solution.PermutationView.Count;

        if (townCount < 2)
        {
            return solution;
        }

        var i = Random.Next(townCount);
        var j = Random.Next(townCount);

        while (i == j)
        {
            j = Random.Next(townCount);
        }

        var permutationResult = new List<int>(solution.PermutationView)
        {
            [i] = solution.PermutationView[j],
            [j] = solution.PermutationView[i]
        };


        return new Solution(solution.Towns, permutationResult);
    }

    public static Solution InsertMutation(Solution solution)
    {
        var townCount = solution.PermutationView.Count;

        if (townCount < 2)
        {
            return solution;
        }

        var currentPosition = Random.Next(townCount);
        var newPosition = Random.Next(townCount);

        while (newPosition == currentPosition)
        {
            newPosition = Random.Next(townCount);
        }

        var permutationResult = new List<int>(solution.PermutationView);

        permutationResult.RemoveAt(currentPosition);
        permutationResult.Insert(newPosition, solution.PermutationView[currentPosition]);

        return new Solution(solution.Towns, permutationResult);
    }


    public static Solution InversionMutation(Solution solution)
    {
        const double percentageMutated = 0.2;
        var townCount = solution.PermutationView.Count;

        if (townCount < 2)
        {
            return solution;
        }

        var mutationLength = (int)Math.Ceiling(percentageMutated * townCount);

        if (mutationLength < 3)
        {
            mutationLength = 3;
        }


        var permutationResult = new List<int>(solution.PermutationView);

        var i = Random.Next(townCount);


        for (var j = 0; j < mutationLength; j++)
        {
            permutationResult[(i + j) % townCount] = solution.PermutationView[(i + mutationLength - 1 - j) % townCount];
        }

        return new Solution(solution.Towns, permutationResult);
    }

    public static Solution ScrambleMutation(Solution solution)
    {
        const double percentageMutated = 0.2;
        var townCount = solution.PermutationView.Count;

        if (townCount < 2)
        {
            return solution;
        }

        var mutationLength = (int)Math.Ceiling(percentageMutated * townCount);

        if (mutationLength < 3)
        {
            mutationLength = 3;
        }

        var i = Random.Next(townCount);

        var permutationResult = new List<int>(solution.PermutationView);
        var segment = new List<int>();

        for (var j = 0; j < mutationLength; j++)
        {
            segment.Add(solution.PermutationView[(i + j) % townCount]);
        }

        segment = segment.Shuffle().ToList();

        for (var j = 0; j < mutationLength; j++)
        {
            permutationResult[(i + j) % townCount] = segment[j];
        }


        return new Solution(solution.Towns, permutationResult);
    }

    public static Solution NeutralMutation(Solution solution)
    {
        return solution;
    }
}