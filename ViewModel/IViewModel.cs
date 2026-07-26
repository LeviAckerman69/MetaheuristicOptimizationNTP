using System.Collections.ObjectModel;
using System.Windows;
using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.ViewModel;

public interface IViewModel
{
    public int PopulationSize { get; set; }
    public ObservableCollection<Town> Towns { get; }

    public Population Population { get; set; }

    public Solution? SelectedSolution { get; set; }

    public void AddNewTownAtPosition(Point position);

    public void RemoveTownAtPosition(Point position);

    public void CreatePopulation();
}