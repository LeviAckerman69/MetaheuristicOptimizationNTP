using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MetaheuristicOptimizationNTP.Structures
{
    public class Solution
    {

        private static readonly Random Random = new Random();

        public List<int> Permutation { get; } = [];

        public Solution(TownsList townsList, bool shuffle = false)
        {
            Permutation = Enumerable.Range(0, townsList.Count).ToList();

            if (shuffle)
            {
                Permutation = Permutation.Shuffle().ToList();
            }
        }

        public void SwapMutation()
        {
            var n = Permutation.Count;

            if (n < 2)
            {
                return;
            }

            var i = Random.Next(n);
            var j = Random.Next(n);

            while (i == j)
            {
                j = Random.Next(n);
            }

            var temp = Permutation[i];

            Permutation[i] = Permutation[j];
            Permutation[j] = temp;
        }


        public void InversionMutation(double percentageMutated = 0.2)
        {
            var townCount = Permutation.Count;

            if (townCount < 2)
            {
                return;
            }

            var mutationLength = (int)Math.Ceiling(percentageMutated * townCount);

            if (mutationLength < 3)
            {
                mutationLength = 3;
            }

            var i = Random.Next(townCount);
            var j = (i + mutationLength - 1) % townCount;

            if (j < i)
            {
                j += townCount;
            }


            while (j > i)
            {
                var temp = Permutation[i % townCount];
                Permutation[i % townCount] = Permutation[j % townCount];
                Permutation[j % townCount] = temp;


                i++;
                j--;

            }

            if (Permutation.Distinct().Count() != townCount)
            {
                throw new Exception("Permutation corrupted!");
            }

        }

    }
}
