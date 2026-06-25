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
        var delta = (endOffset - beginOffset) % count;
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
}