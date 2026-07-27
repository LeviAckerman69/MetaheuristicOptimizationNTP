using MetaheuristicOptimizationNTP.ViewModel;

namespace MetaheuristicOptimizationNTP.Helper;

public static class AlgorithmOptionHelper
{
    public static void FillListOfOptions<TOperation>(
        this IEnumerable<AlgorithmOption<TOperation>> destinationOptions,
        IEnumerable<AlgorithmOption<TOperation>> sourceOptions
    )
    {
        foreach (var (destinationOption, sourceOption) in destinationOptions.Zip(sourceOptions))
        {
            destinationOption.Fill(sourceOption);
        }
    }
}