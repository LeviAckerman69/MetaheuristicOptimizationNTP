using System.Windows.Controls;
using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.ViewModel;

public class PopulationListHelper(Population population, ListBox listPopulation)
{
    private Population Population { get; } = population;
    private ListBox ListPopulation { get; } = listPopulation;

    private List<IPopulationListHelperListener> Listeners { get; } = [];

    public int SelectedIndex
    {
        get;
        set
        {
            field = value;
            NotifyListeners();
        }
    } = -1;

    public Solution? SelectedSolution => SelectedIndex != -1 ? Population.SolutionsView[SelectedIndex] : null;

    public IEnumerable<string> SolutionsFormatted =>
        Population.SolutionsView.Select(solution => $"Solution #{solution.Id}, Fitness: {solution.Fitness}");

    public void Register(IPopulationListHelperListener listHelperListener)
    {
        Listeners.Add(listHelperListener);
    }

    private void NotifyListeners()
    {
        Listeners.ForEach(listener => listener.OnSelectionChanged());
    }
}