using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Text;
using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.ViewModel
{
    public partial class ConfigurationDialogViewModel : ObservableValidator
    {
        public ObservableCollection<AlgorithmOption<MutationOperation>> MutationOptions { get; } = new();
        public ObservableCollection<AlgorithmOption<CrossoverOperation>> CrossoverOptions { get; } = new();

        public ConfigurationDialogViewModel()
        {
            AddMutationOptions();
            AddCrossoverOptions();
        }

        void AddMutationOptions()
        {
            MutationOptions.Add(new AlgorithmOption<MutationOperation>("Swap Mutation", solution => solution.SwapMutation()));
            MutationOptions.Add(new AlgorithmOption<MutationOperation>("Inversion Mutation", solution => solution.InversionMutation()));
            MutationOptions.Add(new AlgorithmOption<MutationOperation>("Scramble Mutation", solution => solution.ScrambleMutation()));
            MutationOptions.Add(new AlgorithmOption<MutationOperation>("Insert Mutation", solution => solution.InsertMutation()));
        }

        void AddCrossoverOptions()
        {
            CrossoverOptions.Add(new AlgorithmOption<CrossoverOperation>(
                "Partially Mapped Crossover", (solution1, solution2) => solution1.PartiallyMatchedCrossover(solution2)));
        }

    }
}
