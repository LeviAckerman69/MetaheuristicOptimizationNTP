using System.ComponentModel.DataAnnotations;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MetaheuristicOptimizationNTP.Helper;
using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class MainViewModel
{
    [ObservableProperty]
    [Range(5, int.MaxValue, ErrorMessage = "Enter population size >= 5.")]
    public partial int PopulationSize { get; set; } = 100;

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

    [RelayCommand]
    public void StepEvolution()
    {
        var parentA = ParentSelectionHelper.TournamentSelection(Population, 5);
        var parentB = ParentSelectionHelper.TournamentSelection(Population, 5, parentA);

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