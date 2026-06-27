using System.Collections.ObjectModel;

namespace MetaheuristicOptimizationNTP.Structures;

public class Population
{
    public ObservableCollection<Solution> Solutions { get; set; } = new();

    private ObservableCollection<Town> TownsList { get; set; }

    public int Count => Solutions.Count;


    public void Populate(ObservableCollection<Town> townsList, int popSize = 100)
    {
        TownsList = townsList;

        Solutions.Clear();

        var tempSolutions = new List<Solution>();

        for (var i = 0; i < popSize; i++)
        {
            var solution = new Solution(townsList, true);
            solution.Evaluate(townsList);
            tempSolutions.Add(solution);
        }

        var sortedSolutions = tempSolutions.OrderBy(solution => solution.Fitness);

        foreach (var solution in sortedSolutions)
        {
            Solutions.Add(solution);
        }
    }
}