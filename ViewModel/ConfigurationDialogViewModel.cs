using MetaheuristicOptimizationNTP.Helper;
using System.Collections.ObjectModel;

namespace MetaheuristicOptimizationNTP.ViewModel;

public class ConfigurationDialogViewModel : DialogBaseViewModel
{
    public ObservableCollection<AlgorithmOption<MutationOperation>> MutationOptions { get; } = new();
    public ObservableCollection<AlgorithmOption<CrossoverOperation>> CrossoverOptions { get; } = new();



    private IEnumerable<MutationOperation> SelectedMutations
    {
        get
        {
            foreach (var algorithmOption in MutationOptions)
            {
                if (algorithmOption.IsSelected)
                {
                    yield return algorithmOption.Operation;
                }
            }
        }
    }

    private IEnumerable<CrossoverOperation> SelectedCrossovers
    {
        get
        {
            foreach (var algorithmOption in CrossoverOptions)
            {
                if (algorithmOption.IsSelected)
                {
                    yield return algorithmOption.Operation;
                }
            }
        }
    }

    public ConfigurationDialogViewModel()
    {
        AddMutationOptions();
        AddCrossoverOptions();
    }

    private void AddMutationOptions()
    {
        MutationOptions.Add(
            new AlgorithmOption<MutationOperation>("Swap Mutation", solution => solution.SwapMutation()));
        MutationOptions.Add(new AlgorithmOption<MutationOperation>("Inversion Mutation",
            solution => solution.InversionMutation()));
        MutationOptions.Add(new AlgorithmOption<MutationOperation>("Scramble Mutation",
            solution => solution.ScrambleMutation()));
        MutationOptions.Add(
            new AlgorithmOption<MutationOperation>("Insert Mutation", solution => solution.InsertMutation()));
    }

    private void AddCrossoverOptions()
    {
        CrossoverOptions.Add(new AlgorithmOption<CrossoverOperation>(
            "Partially Mapped Crossover",
            (solution1, solution2) => solution1.PartiallyMatchedCrossover(solution2)));
        CrossoverOptions.Add(new AlgorithmOption<CrossoverOperation>(
            "Cycle Crossover",
            (solution1, solution2) => solution1.CycleCrossover(solution2)));
    }

    public MutationOperation PickRandomMutationOperation()
    {
        var selected = SelectedMutations.ToArray();

        var randomMutation = selected[Random.Shared.Next(selected.Length)];

        return randomMutation;
    }

    public CrossoverOperation PickRandomCrossoverOperation()
    {
        var selected = SelectedCrossovers.ToArray();
        var randomCrossover = selected[Random.Shared.Next(selected.Length)];
        return randomCrossover;
    }

    public void Fill(ConfigurationDialogViewModel sourceViewModel)
    {
        MutationOptions.FillListOfOptions(sourceViewModel.MutationOptions);
        CrossoverOptions.FillListOfOptions(sourceViewModel.CrossoverOptions);
    }


}