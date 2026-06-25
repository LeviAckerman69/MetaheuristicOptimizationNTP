using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using MetaheuristicOptimizationNTP.Genetic;
using MetaheuristicOptimizationNTP.Structures;
using Solution = MetaheuristicOptimizationNTP.Genetic.Solution;

namespace MetaheuristicOptimizationNTP.Components;

public interface ITownDisplayViewModel
{
    ObservableCollection<Town> Towns { get; }

    Population Population { get; }

    Solution? SelectedSolution { get; }

    IRelayCommand<Point> AddTownAtPointCommand { get; }

    IRelayCommand<Point> RemoveTownAtPointCommand { get; }
}