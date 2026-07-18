using CommunityToolkit.Mvvm.ComponentModel;
using MetaheuristicOptimizationNTP.Structures;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MetaheuristicOptimizationNTP.ViewModel
{
    public delegate Solution MutationOperation(Solution solution);
    public delegate Solution CrossoverOperation(Solution solution1, Solution solution2);

    public partial class AlgorithmOption<TOperation> : ObservableValidator
    {
        public string Name { get; }

        [ObservableProperty]
        public partial bool IsSelected {get; set;}

        [ObservableProperty]
        [Range(0.0d, 1.0d, ErrorMessage = "Set probability between 0.0 and 1.0.")]
        public partial double Probability { get; set; } = 0.0d;

        public TOperation Operation { get; }

        public AlgorithmOption(string name, TOperation operation)
        {
            Name = name;
            Operation = operation;
        }

    }
}
    