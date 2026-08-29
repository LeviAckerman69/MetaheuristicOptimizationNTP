using CommunityToolkit.Mvvm.ComponentModel;
using MetaheuristicOptimizationNTP.Structures;
using System.ComponentModel.DataAnnotations;

namespace MetaheuristicOptimizationNTP.ViewModel;

public delegate Solution MutationOperation(Solution solution);

public delegate Solution CrossoverOperation(Solution solution1, Solution solution2);

public partial class AlgorithmOption<TOperation> : ObservableValidator
{
    public string Name { get; }

    [ObservableProperty]
    public partial bool IsSelected { get; set; } = true;

    [ObservableProperty]
    [Range(0.0d, 1.0d, ErrorMessage = "Set probability between 0.0 and 1.0.")]
    public partial double Probability { get; set; } = 0.0d;

    public TOperation Operation { get; }

    public AlgorithmOption(string name, TOperation operation)
    {
        Name = name;
        Operation = operation;
    }

    public AlgorithmOption(AlgorithmOption<TOperation> algorithmOption)
    {
        Name = algorithmOption.Name;
        IsSelected = algorithmOption.IsSelected;
        Probability = algorithmOption.Probability;
        Operation = algorithmOption.Operation;
    }

    public void Fill(AlgorithmOption<TOperation> sourceOption)
    {
        if (Name != sourceOption.Name)
        {
            throw new ArgumentException("Option names do not match.");
        }

        IsSelected = sourceOption.IsSelected;
        Probability = sourceOption.Probability;
    }
}