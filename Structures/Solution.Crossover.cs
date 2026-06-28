using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging.Effects;
using System.Text;

namespace MetaheuristicOptimizationNTP.Structures
{
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

            stepCount = (count - stepCount);

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
    }
}
