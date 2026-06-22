using System.Security.Policy;
using MetaheuristicOptimizationNTP.Structures;
using MetaheuristicOptimizationNTP.ViewModel;

namespace MetaheuristicOptimizationNTP;

public static class Storage
{
    public static TownsList Towns { get; } = new();

    public static Population? Population { get; set; }

    public static PopulationListHelper? PopulationListHelper { get; set; }

}