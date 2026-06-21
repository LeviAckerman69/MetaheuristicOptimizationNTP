using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MetaheuristicOptimizationNTP.Structures
{
    public class Solution
    {
        private static readonly Random Random = new Random();

        public List<int> Permutation { get; }

        public Solution(TownsList townsList, bool shuffle = false)
        {
            Permutation = Enumerable.Range(0, townsList.Count).ToList();

            if (shuffle)
            {
                Permutation = Permutation.Shuffle().ToList();
            }
        }

        private Solution(List<int> currentPermutation)
        {
            Permutation = new List<int>(currentPermutation);
        }


        public Solution SwapMutation()
        {
            var townCount = Permutation.Count;

            if (townCount < 2)
            {
                return new Solution(Permutation);
            }

            var mutatedSolution = new Solution(Permutation);

            var i = Random.Next(townCount);
            var j = Random.Next(townCount);

            while (i == j)
            {
                j = Random.Next(townCount);
            }

            mutatedSolution.Permutation[i] = Permutation[j];
            mutatedSolution.Permutation[j] = Permutation[i];

            if (mutatedSolution.Permutation.Distinct().Count() != townCount)
            {
                throw new Exception("Permutation corrupted!");
            }

            return mutatedSolution;
        }

        public Solution InsertMutation()
        {
            var townCount = Permutation.Count;

            if (townCount < 2)
            {
                return new Solution(Permutation);
            }

            var mutatedSolution = new Solution(Permutation);

            var currentPosition = Random.Next(townCount);

            var newPosition = Random.Next(townCount);

            while (newPosition == currentPosition)
            {
                newPosition = Random.Next(townCount);
            }

            var temp = mutatedSolution.Permutation[currentPosition];
            mutatedSolution.Permutation.RemoveAt(currentPosition);
            mutatedSolution.Permutation.Insert(newPosition, temp);

            if (mutatedSolution.Permutation.Distinct().Count() != townCount)
            {
                throw new Exception("Permutation corrupted!");
            }

            return mutatedSolution;
        }


        public Solution InversionMutation(double percentageMutated = 0.2)
        {
            var townCount = Permutation.Count;

            if (townCount < 2)
            {
                return new Solution(Permutation);
            }

            var mutatedSolution = new Solution(Permutation);

            var mutationLength = (int)Math.Ceiling(percentageMutated * townCount);

            if (mutationLength < 3)
            {
                mutationLength = 3;
            }

            var i = Random.Next(townCount);


            for (var j = 0; j < mutationLength; j++)
                mutatedSolution.Permutation[(i + j) % townCount] =
                    Permutation[(i + mutationLength - 1 - j) % townCount];

            if (mutatedSolution.Permutation.Distinct().Count() != townCount)
            {
                throw new Exception("Permutation corrupted!");
            }

            return mutatedSolution;
        }

        public Solution ScrambleMutation(double percentageMutated = 0.2)
        {
            var townCount = Permutation.Count;

            if (townCount < 2)
            {
                return new Solution(Permutation);
            }

            var mutatedSolution = new Solution(Permutation);

            var mutationLength = (int)Math.Ceiling(percentageMutated * townCount);

            if (mutationLength < 3)
            {
                mutationLength = 3;
            }

            var i = Random.Next(townCount);

            var segment = new List<int>();

            for (var j = 0; j < mutationLength; j++)
            {
                segment.Add(mutatedSolution.Permutation[(i + j) % townCount]);
            }

            segment = segment.Shuffle().ToList();

            for (var j = 0; j < mutationLength; j++)
            {
                mutatedSolution.Permutation[(i + j) % townCount] = segment[j];
            }

            if (mutatedSolution.Permutation.Distinct().Count() != townCount)
            {
                throw new Exception("Permutation corrupted!");
            }

            return mutatedSolution;
        }
    }
}