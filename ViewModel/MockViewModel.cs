using System.Collections.ObjectModel;
using System.Windows;
using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.ViewModel;

public class MockViewModel : IViewModel
{
    public int PopulationSize { get; set; } = 0;
    public ObservableCollection<Town> Towns { get; } = new();
    public Population Population { get; set; } = new();
    public Solution? SelectedSolution { get; set; }

    public void AddNewTownAtPosition(Point position)
    {
        throw new NotImplementedException();
    }

    public void RemoveTownAtPosition(Point position)
    {
        throw new NotImplementedException();
    }

    public void CreatePopulation()
    {
        throw new NotImplementedException();
    }
}