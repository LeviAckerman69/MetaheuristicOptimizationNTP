using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MetaheuristicOptimizationNTP.Helper;
using MetaheuristicOptimizationNTP.Structures;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Windows;

namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class MainViewModel
{
    private CancellationTokenSource? _evolutionCancellation;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EvolveText))]
    [NotifyPropertyChangedFor(nameof(CanEditProblem))]
    [NotifyCanExecuteChangedFor(nameof(CreatePopulationCommand))]
    [NotifyCanExecuteChangedFor(nameof(StepEvolutionCommand))]
    [NotifyCanExecuteChangedFor(nameof(OpenConfigurationDialogCommand))]
    [NotifyCanExecuteChangedFor(nameof(OpenTownManagementCommand))]
    [NotifyCanExecuteChangedFor(nameof(ClearTownsCommand))]
    public partial bool IsEvolving { get; private set; }

    public string EvolveText => IsEvolving ? "Stop" : "Evolve";

    public bool CanEditProblem => !IsEvolving;

    [ObservableProperty]
    [Range(5, int.MaxValue, ErrorMessage = "Enter population size >= 5.")]
    public partial int PopulationSize { get; set; } = 100;

    [ObservableProperty]
    public partial Solution? SelectedSolution { get; set; } = null;

    [RelayCommand(CanExecute = nameof(CanEditProblem))]
    public void CreatePopulation()
    {
        if (IsEvolving)
        {
            return;
        }

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


    [RelayCommand(CanExecute = nameof(CanEditProblem))]
    public void StepEvolution()
    {
        if (IsEvolving || !ValidateEvolution())
        {
            return;
        }

        EvolveOneStep();
        SelectedSolution = Population.Solutions[0];
    }

    // Keep the toggle command available while the async run is active so it can stop it.
    [RelayCommand(AllowConcurrentExecutions = true)]
    public async Task EvolveAsync()
    {
        if (IsEvolving)
        {
            StopEvolution();
            return;
        }

        if (!ValidateEvolution())
        {
            return;
        }

        using (var cancellation = new CancellationTokenSource())
        {
            _evolutionCancellation = cancellation;
            IsEvolving = true;
            try
            {
                await RunEvolutionAsync(cancellation.Token);
            }
            catch (OperationCanceledException) when (cancellation.IsCancellationRequested)
            {
                // Stopping a run is normal completion, not an error.
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Evolution stopped", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _evolutionCancellation = null;
                IsEvolving = false;
            }
        }
    }

    public void StopEvolution()
    {
        _evolutionCancellation?.Cancel();
    }

    private async Task RunEvolutionAsync(CancellationToken cancellationToken)
    {
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Keep observable collection updates on the UI context and yield between batches.
            var stopwatch = Stopwatch.StartNew();
            do
            {
                cancellationToken.ThrowIfCancellationRequested();
                EvolveOneStep();
            }
            while (stopwatch.ElapsedMilliseconds < 8);

            SelectedSolution = Population.Solutions[0];
            await Task.Delay(16, cancellationToken);
        }
    }

    private bool ValidateEvolution()
    {
        var error = GetEvolutionValidationError();
        if (error is null)
        {
            return true;
        }

        MessageBox.Show(error, "Cannot evolve", MessageBoxButton.OK, MessageBoxImage.Warning);
        return false;
    }

    private string? GetEvolutionValidationError()
    {
        if (Towns.Count < 3)
        {
            return "Add at least three towns before evolving.";
        }

        if (Population.Count < 2)
        {
            return "Create a population before evolving.";
        }

        if (Population.Solutions.Any(solution => solution.PermutationView.Count != Towns.Count))
        {
            return "The towns have changed. Create a new population before evolving.";
        }

        if (!ConfigurationDialogViewModel.CrossoverOptions.Any(option => option.IsSelected) ||
            !ConfigurationDialogViewModel.MutationOptions.Any(option => option.IsSelected))
        {
            return "Select at least one crossover and one mutation in Configuration.";
        }

        return null;
    }

    private void EvolveOneStep()
    {
        var parentA = ParentSelectionHelper.TournamentSelection(Population, Math.Min(5, Population.Count));
        var parentB = ParentSelectionHelper.TournamentSelection(Population, Math.Min(5, Population.Count - 1), parentA);

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

