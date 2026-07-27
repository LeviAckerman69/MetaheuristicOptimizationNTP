using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MetaheuristicOptimizationNTP.Database;
using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class MainViewModel : ObservableValidator
{
    public ObservableCollection<Town> Towns { get; } = new();

    public Population Population { get; set; } = new();

    private TspDbContext DbContext { get; } = new();

    public MainViewModel()
    {
        var towns = DbContext.Towns;

        foreach (var town in towns)
        {
            Towns.Add(town);
        }
    }
}