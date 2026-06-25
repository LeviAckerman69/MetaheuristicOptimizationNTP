using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.Genetic;

using CrossoverAlgorithm = Func<Solution, Solution, Solution>;

public class Crossover
{
    private static Random Random { get; } = new();

    public static List<CrossoverAlgorithm> CrossoverAlgorithms =>
        [OrderCrossover, PartiallyMappedCrossover, CycleCrossover];

    public static Solution OrderCrossover(Solution parentA, Solution parentB)
    {
        var permutationA = parentA.PermutationView;
        var permutationB = parentB.PermutationView;

        var count = permutationA.Count;
        var beginOffset = Random.Next(count);
        var endOffset = Random.Next(count);

        while (beginOffset == endOffset)
        {
            endOffset = Random.Next(count);
        }

        var permutationResult = Enumerable.Repeat(-1, count).ToList();
        var delta = (endOffset - beginOffset + count) % count;
        var copied = new HashSet<int>();

        for (var i = 0; i < delta; i++)
        {
            var offset = (beginOffset + i) % count;
            permutationResult[offset] = permutationA[offset];
            copied.Add(permutationA[offset]);
        }

        {
            var offset = endOffset;
            foreach (var element in Enumerable.Where<int>(permutationB, p => !copied.Contains(p)))
            {
                permutationResult[offset] = element;
                offset = (offset + 1) % count;
            }
        }

        return new Solution(parentA.Towns, permutationResult);
    }

    public static Solution PartiallyMappedCrossover(Solution parentA, Solution parentB)
    {
        var permutationA = parentA.PermutationView;
        var permutationB = parentB.PermutationView;

        var count = permutationA.Count;
        var beginOffset = Random.Next(count);
        var endOffset = Random.Next(count);

        while (beginOffset == endOffset)
        {
            endOffset = Random.Next(count);
        }

        var permutationResult = Enumerable.Repeat(-1, count).ToList();
        var delta = (endOffset - beginOffset + count) % count;
        var mapping = Enumerable.Repeat(-1, count).ToList();

        for (var i = 0; i < delta; i++)
        {
            var valueA = permutationA[(beginOffset + i) % count];
            var valueB = permutationB[(beginOffset + i) % count];
            permutationResult[(beginOffset + i) % count] = permutationA[(beginOffset + i) % count];
            mapping[valueA] = valueB;
        }

        var invDelta = count - delta;
        for (var i = 0; i < invDelta; i++)
        {
            var valueB = permutationB[(endOffset + i) % count];
            while (mapping[valueB] != -1)
            {
                valueB = mapping[valueB];
            }

            permutationResult[(endOffset + i) % count] = valueB;
        }

        return new Solution(parentA.Towns, permutationResult);
    }

    public static Solution CycleCrossover(Solution parentA, Solution parentB)
    {
        var permutationA = Enumerable.ToList<int>(parentA.PermutationView);
        var permutationB = Enumerable.ToList<int>(parentB.PermutationView);

        var count = permutationA.Count;

        var permutationResult = Enumerable.Repeat(-1, count).ToList();
        var pos = Random.Next(count);

        while (permutationResult[pos] != -1)
        {
            var valueA = permutationA[pos];
            permutationResult[pos] = valueA;

            var valueB = permutationB[pos];
            pos = permutationA.IndexOf(valueB);
        }

        for (var i = 0; i < count; i++)
        {
            if (permutationResult[i] == -1)
            {
                permutationResult[i] = permutationB[i];
            }
        }

        return new Solution(parentA.Towns, permutationResult);
    }
}