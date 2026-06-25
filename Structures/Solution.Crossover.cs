namespace MetaheuristicOptimizationNTP.Structures;

public partial class Solution
{
    public Solution OrderCrossover(Solution other)
    {
        var permutationA = PermutationView;
        var permutationB = other.PermutationView;

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
            foreach (var element in permutationB.Where(p => !copied.Contains(p)))
            {
                permutationResult[offset] = element;
                offset = (offset + 1) % count;
            }
        }

        return new Solution(Towns, permutationResult);
    }

    public Solution PartiallyMappedCrossover(Solution other)
    {
        var permutationA = PermutationView;
        var permutationB = other.PermutationView;

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

        return new Solution(Towns, permutationResult);
    }

    public Solution CycleCrossover(Solution other)
    {
        var permutationA = PermutationView.ToList();
        var permutationB = other.PermutationView.ToList();

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

        return new Solution(Towns, permutationResult);
    }
}