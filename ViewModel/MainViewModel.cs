using System.Collections.ObjectModel;
using System.Windows;
using MetaheuristicOptimizationNTP.Components;
using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.ViewModel;

public class MainViewModel : ITownDisplayViewModel
{
    private int NextTownId { get; set; }

    public ObservableCollection<Town> Towns { get; } = [];

    public void AddTownAt(Point point)
    {
        NextTownId++;
        var town = new Town
        {
            Name = $"Town #{NextTownId}",
            X = point.X,
            Y = point.Y
        };
        Towns.Add(town);
    }

    public Town? FindTownAtPosition(Point position, double scalingFactor = 1)
    {
        foreach (var town in Towns.Reverse())
        {
            if (town.ContainsAtScale(position, scalingFactor))
            {
                return town;
            }
        }

        return null;
    }
}