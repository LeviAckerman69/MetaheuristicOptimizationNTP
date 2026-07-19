using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MetaheuristicOptimizationNTP.Structures;
using MetaheuristicOptimizationNTP.View;

namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class MainViewModel : ObservableValidator, IViewModel
{
    private static Random Random { get; } = new();

    public ConfigurationDialogViewModel ConfigurationDialogViewModel { get; }

    public MainViewModel()
    {
        ConfigurationDialogViewModel = new ConfigurationDialogViewModel();
    }

    [ObservableProperty]
    [Range(5, int.MaxValue, ErrorMessage = "Enter population size >= 5.")]
    public partial int PopulationSize { get; set; } = 100;

    public ObservableCollection<Town> Towns { get; } = new();
    public Population Population { get; set; } = new();

    [ObservableProperty]
    public partial Solution? SelectedSolution { get; set; } = null;

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

    [RelayCommand]
    public void OpenConfigurationDialog()
    {
        var configurationDialogViewModelCopy = new ConfigurationDialogViewModel();
        configurationDialogViewModelCopy.Fill(ConfigurationDialogViewModel);

        var dialog = new ConfigurationDialog(configurationDialogViewModelCopy)
        {
            Owner = Application.Current.MainWindow
        };
        dialog.ShowDialog();

        if (dialog.DialogResult == true)
        {
            ConfigurationDialogViewModel.Fill(configurationDialogViewModelCopy);
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
        var indexA = Random.Shared.Next(Population.Count);
        var indexB = Random.Shared.Next(Population.Count);

        while (indexA == indexB)
        {
            indexB = Random.Shared.Next(Population.Count);
        }

        var parentA = Population.Solutions[indexA];
        var parentB = Population.Solutions[indexB];

        var crossoverOperation = ConfigurationDialogViewModel.PickRandomCrossoverOperation();
        var child = crossoverOperation(parentA, parentB);

        var mutationOperation = ConfigurationDialogViewModel.PickRandomMutationOperation();
        var mutatedChild = mutationOperation(child);

        mutatedChild.Evaluate(Towns);

        var solutionIndex = Population.Solutions.TakeWhile(solution => solution.Fitness < mutatedChild.Fitness).Count();
        Population.Solutions.Insert(solutionIndex, mutatedChild);

        Population.Solutions.RemoveAt(Population.Solutions.Count - 1);
    }
}