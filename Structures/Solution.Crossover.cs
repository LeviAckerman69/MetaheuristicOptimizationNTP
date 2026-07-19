namespace MetaheuristicOptimizationNTP.Structures;

public partial class Solution
{
    public Solution PartiallyMatchedCrossover(Solution other)
    {
        var permutationA = PermutationView;
        var permutationB = other.PermutationView;
        var count = permutationA.Count;

        var permutationCrossover = Enumerable.Repeat(-1, count).ToList();
        var startPosition = Random.Next(count);
        var endPosition = Random.Next(count);

        while (startPosition == endPosition)
        {
            endPosition = Random.Next(count);
        }

        var stepCount = (endPosition - startPosition + count) % count;

        var mapping = Enumerable.Repeat(-1, count).ToList();

        for (var step = 0; step < stepCount; step++)
        {
            var currentStep = (startPosition + step) % count;
            var valueA = permutationA[currentStep];
            var valueB = permutationB[currentStep];

            permutationCrossover[currentStep] = valueA;
            mapping[valueA] = valueB;
        }

        stepCount = count - stepCount;

        for (var step = 0; step < stepCount; step++)
        {
            var currentStep = (endPosition + step) % count;
            var valueB = permutationB[currentStep];

            while (mapping[valueB] != -1)
            {
                valueB = mapping[valueB];
            }

            permutationCrossover[currentStep] = valueB;
        }

        return new Solution(permutationCrossover);
    }

    public Solution CycleCrossover(Solution other)
    {
        var permutationA = PermutationView.ToList();
        var permutationB = other.PermutationView.ToList();
        var count = permutationA.Count;

        var permutationCrossover = Enumerable.Repeat(-1, count).ToList();

        var selectedIndex = Random.Shared.Next(count);

        while (permutationCrossover[selectedIndex] == -1)
        {
            var valueA = permutationA[selectedIndex];
            var valueB = permutationB[selectedIndex];

            permutationCrossover[selectedIndex] = valueA;

            selectedIndex = permutationA.IndexOf(valueB);
        }

        for (var i = 0; i < count; i++)
        {
            if (permutationCrossover[i] == -1)
            {
                permutationCrossover[i] = permutationB[i];
            }
        }

        return new Solution(permutationCrossover);
    }
}