namespace MetaheuristicOptimizationNTP.Structures;

public partial class Solution
{
    public Solution SwapMutation()
    {
        var townCount = Permutation.Count;

        if (townCount < 2)
        {
            return this;
        }

        var i = Random.Next(townCount);
        var j = Random.Next(townCount);

        while (i == j)
        {
            j = Random.Next(townCount);
        }

        var permutation = new List<int>(Permutation)
        {
            [i] = Permutation[j],
            [j] = Permutation[i]
        };


        return new Solution(Towns, permutation);
    }

    public Solution InsertMutation()
    {
        var townCount = Permutation.Count;

        if (townCount < 2)
        {
            return this;
        }

        var currentPosition = Random.Next(townCount);
        var newPosition = Random.Next(townCount);

        while (newPosition == currentPosition)
        {
            newPosition = Random.Next(townCount);
        }

        var permutation = new List<int>(Permutation);

        permutation.RemoveAt(currentPosition);
        permutation.Insert(newPosition, Permutation[currentPosition]);

        return new Solution(Towns, permutation);
    }


    public Solution InversionMutation(double percentageMutated = 0.2)
    {
        var townCount = Permutation.Count;

        if (townCount < 2)
        {
            return this;
        }

        var mutationLength = (int)Math.Ceiling(percentageMutated * townCount);

        if (mutationLength < 3)
        {
            mutationLength = 3;
        }


        var permutation = new List<int>(Permutation);

        var i = Random.Next(townCount);


        for (var j = 0; j < mutationLength; j++)
        {
            permutation[(i + j) % townCount] = Permutation[(i + mutationLength - 1 - j) % townCount];
        }

        return new Solution(Towns, permutation);
    }

    public Solution ScrambleMutation(double percentageMutated = 0.2)
    {
        var townCount = Permutation.Count;

        if (townCount < 2)
        {
            return this;
        }

        var mutationLength = (int)Math.Ceiling(percentageMutated * townCount);

        if (mutationLength < 3)
        {
            mutationLength = 3;
        }

        var i = Random.Next(townCount);

        var permutation = new List<int>(Permutation);
        var segment = new List<int>();

        for (var j = 0; j < mutationLength; j++)
        {
            segment.Add(permutation[(i + j) % townCount]);
        }

        segment = segment.Shuffle().ToList();

        for (var j = 0; j < mutationLength; j++)
        {
            permutation[(i + j) % townCount] = segment[j];
        }


        return new Solution(Towns, permutation);
    }
}