using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.Helper
{
    public static class ParentSelectionHelper
    {
        public static Solution TournamentSelection(Population population, int tournamentSize, Solution? excludedSolution = null)
        {
            var solutions = new List<Solution>();

            var solutionIndices = new List<int>();

            for (var i = 0; i < tournamentSize; i++)
            {
                var solutionIndex = Random.Shared.Next(population.Count);
                while (solutionIndices.Contains(solutionIndex) || population.Solutions[solutionIndex] == excludedSolution)
                {
                    solutionIndex = Random.Shared.Next(population.Count);
                }

                solutions.Add(population.Solutions[solutionIndex]);

                solutionIndices.Add(solutionIndex);
            }

            return solutions.MinBy(solution => solution.Fitness)!;
        }
    }
}
