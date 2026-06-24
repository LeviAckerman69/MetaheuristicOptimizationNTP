using System.Collections.ObjectModel;
using MetaheuristicOptimizationNTP.Components;
using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.ViewModel;

public class MockViewModel : ITownDisplayViewModel
{
    public ObservableCollection<Town> Towns { get; } = [];

    public ObservableCollection<Solution> Population { get; } = [];
}