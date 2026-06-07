using System;
using System.Collections.Generic;
using System.Text;

namespace MetaheuristicOptimizationNTP.Structures
{
    public class Solution
    {

        private static readonly Random _random = new Random();

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

            var i = _random.Next(n);
            var j = _random.Next(n);

            while (i == j)
            {
                j = _random.Next(n);
            }

            var temp = Permutation[i];

            Permutation[i] = Permutation[j];
            Permutation[j] = temp;
        }
    }
}
