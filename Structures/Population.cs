using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace MetaheuristicOptimizationNTP.Structures;

public class Population
{
    private ObservableCollection<Solution> Solutions { get; }

    private IReadOnlyList<Town> TownsList { get; set; }

    public Population(List<Town> townsList, int popSize = 100)
    {
        TownsList = townsList;
        Solutions = [];

        var solutions = new List<Solution>();

        for (var i = 0; i < popSize; i++)
        {
            var solution = new Solution(TownsList, true);
            solution.Evaluate(TownsList);
            Solutions.Add(solution);
        }

        Solutions = new ObservableCollection<Solution>(solutions.OrderBy(solution => solution.Fitness).ToList());
    }
 
}