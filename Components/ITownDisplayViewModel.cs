using System.Collections.ObjectModel;
using System.Windows;
using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.Components;

public interface ITownDisplayViewModel
{
    ObservableCollection<Town> Towns { get; }

    ObservableCollection<Solution> Population { get; }

    void AddTownAt(Point point) { }

    Town? FindTownAtPosition(Point position, double distance) { return null; }

    void CreatePopulation() { }
}