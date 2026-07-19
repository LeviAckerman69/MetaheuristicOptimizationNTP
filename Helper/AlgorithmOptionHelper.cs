using MetaheuristicOptimizationNTP.ViewModel;

namespace MetaheuristicOptimizationNTP.Helper;

public static class AlgorithmOptionHelper
{
    public static void FillListOfOptions<TOperation>(this IEnumerable<AlgorithmOption<TOperation>> destinationOptions,
        IEnumerable<AlgorithmOption<TOperation>> sourceOptions)
    {
        //for (var i =0; i < destinationOptions.Count(); i++)
        //{
        //    var algorithmOption = destinationOptions.ElementAt(i);
        //    var sourceOption = sourceOptions.ElementAt(i);
        //    algorithmOption.Fill(sourceOption);
        //}

        foreach (var (destinationOption, sourceOption) in destinationOptions.Zip(sourceOptions))
        {
            destinationOption.Fill(sourceOption);
        }
    }
}