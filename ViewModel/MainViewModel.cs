using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MetaheuristicOptimizationNTP.Structures;
using MetaheuristicOptimizationNTP.View;


namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class MainViewModel : ObservableValidator, IViewModel
{
    private static Random Random { get; } = new();

    [ObservableProperty]
    [Range(5, int.MaxValue, ErrorMessage = "Enter population size >= 5.")]
    public partial int PopulationSize { get; set; } = 100;

    public ObservableCollection<Town> Towns { get; } = new();
    public Population Population { get; set; } = new();

    [ObservableProperty] public partial Solution? SelectedSolution { get; set; } = null;

    public ConfigurationDialogViewModel ConfigurationDialogViewModel { get; }

    public MainViewModel()
    {
        ConfigurationDialogViewModel = new ConfigurationDialogViewModel();
    }

    [RelayCommand]
    public void OpenConfigurationDialog()
    {
        var dialog = new ConfigurationDialog(ConfigurationDialogViewModel);
        dialog.ShowDialog();
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

        Population.Populate(Towns, PopulationSize);
        SelectedSolution = Population.Solutions.FirstOrDefault();
    }

    public void AddNewTownAtPosition(Point position)
    {
        if (FindTownAtPosition(position, 4) == null)
        {
            var id = Towns.Count + 1;
            var town = new Town { X = position.X, Y = position.Y, Name = $"Town {id}" };
            Towns.Add(town);
        }
    }

    public void RemoveTownAtPosition(Point position)
    {
        var town = FindTownAtPosition(position);
        if (town is not null)
        {
            Towns.Remove(town);
        }
    }

    private Town? FindTownAtPosition(Point position, double scalingFactor = 1)
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
    public void StepEvolution()
    {
        var solutionIndex = Random.Shared.Next(Population.Count);
        var parentA = Population.Solutions[solutionIndex];
        var parentB = Population.Solutions[solutionIndex];

        do
        {
            solutionIndex = Random.Shared.Next(Population.Count);
            parentB = Population.Solutions[solutionIndex];
        } while (ReferenceEquals(parentA, parentB));

        var crossoverOperation = ConfigurationDialogViewModel.PickRandomCrossoverOperation();
        var child = crossoverOperation(parentA, parentB);

        var mutationOperation = ConfigurationDialogViewModel.PickRandomMutationOperation();
        var mutatedChild = mutationOperation(child);

        mutatedChild.Evaluate(Towns);

        Population.Solutions.Add(mutatedChild);

        var worstSolution = Population.Solutions.MaxBy(solution => solution.Fitness)!;
        Population.Solutions.Remove(worstSolution);

        var previouslySelected = SelectedSolution;

        if (previouslySelected == worstSolution)
        {
            previouslySelected = null;
        }

        var sorted = Population.Solutions.OrderBy(solution => solution.Fitness).ToList();
        Population.Solutions.Clear();
        foreach (var solution in sorted)
        {
            Population.Solutions.Add(solution);
        }

        SelectedSolution = previouslySelected;
    }
}