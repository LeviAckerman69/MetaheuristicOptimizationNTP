using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using MetaheuristicOptimizationNTP.Components;
using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.ViewModel;

public class MockViewModel : ITownDisplayViewModel
{
    public ObservableCollection<Town> Towns => [];

    public Population Population { get; } = new();

    public Solution? SelectedSolution => null;

    public IRelayCommand<Point> AddTownAtPointCommand => null;

    public IRelayCommand<Point> RemoveTownAtPointCommand => null;
}
