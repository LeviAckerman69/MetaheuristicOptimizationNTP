using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MetaheuristicOptimizationNTP.Components;
using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class MainViewModel : ObservableValidator, ITownDisplayViewModel
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Range(5, int.MaxValue, ErrorMessage = "Enter population size >= 5.")]
    public partial int PopulationSize { get; set; } = 0;

    private int NextTownId { get; set; }

    public ObservableCollection<Solution> Solution { get; } = [];

    [ObservableProperty]
    public partial Solution? SelectedSolution { get; set; }

    public ObservableCollection<Town> Towns { get; } = [];
    public ObservableCollection<Solution> Population { get; } = [];

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

    [RelayCommand]
    public void CreatePopulation()
    {
        ValidateAllProperties();
        if (HasErrors)
        {
            var errors = GetErrors(nameof(PopulationSize));

            foreach (var error in errors)
            {
                MessageBox.Show(error.ErrorMessage);
            }

            return;
        }

        Population.Clear();

        for (var i = 0; i < PopulationSize; i++)
        {
            var solution = new Solution(Towns, true);
            solution.Evaluate(Towns);
            Population.Add(solution);
        }
    }
}