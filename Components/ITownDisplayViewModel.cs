using System.Collections.ObjectModel;
using System.Windows;
using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.Components;

public interface ITownDisplayViewModel
{
    ObservableCollection<Town> Towns => null;

    Population Population => null;

    Solution? SelectedSolution => null;

    void AddTownAt(Point point) { }

    Town? FindTownAtPosition(Point position, double distance)
    {
        return null;
    }

    bool RemoveTown(Town town)
    {
        return false;
    }
}