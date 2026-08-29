using CommunityToolkit.Mvvm.ComponentModel;
using MetaheuristicOptimizationNTP.Database;
using MetaheuristicOptimizationNTP.Structures;
using System.Collections.ObjectModel;

namespace MetaheuristicOptimizationNTP.ViewModel;


public partial class MainViewModel : ObservableValidator, IViewModel
{
    public TspDbContext DbContext { get; } = new();
    public ObservableCollection<Town> Towns { get; } = new();
    public Population Population { get; set; } = new();

    public MainViewModel()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Database.EnsureCreated();

        var towns = DbContext.Towns;

        foreach (var town in towns)
        {
            Towns.Add(town);
        }
    }

}
