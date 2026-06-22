using System.Xml.Linq;

namespace MetaheuristicOptimizationNTP.Structures;

public class Population
{
    private List<Solution> Solutions { get; }

    public IReadOnlyList<Solution> SolutionsView => Solutions.AsReadOnly();

    private TownsList TownsList { get; set; }

    public int Count => Solutions.Count;


    public Population(TownsList townsList, int popSize = 100)
    {
        TownsList = townsList;

        Solutions = new List<Solution>();

        for (var i = 0; i < popSize; i++)
        {
            var solution = new Solution(townsList, true);
            solution.Evaluate(townsList);
            Solutions.Add(solution);
        }

        Solutions = Solutions.OrderBy(solution => solution.Fitness).ToList();
    }
 
}