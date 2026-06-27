using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MetaheuristicOptimizationNTP.Structures;


namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class MainViewModel : ObservableObject, IViewModel
{
    private static Random Random { get; } = new();
    public int PopulationSize { get; set; } = 100;
    public ObservableCollection<Town> Towns { get; } = new();
    public Population Population { get; set; } = new();

    [ObservableProperty] 
    public partial Solution? SelectedSolution { get; set; } = null;

    [RelayCommand]
    public void CreatePopulation()
    {
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
        var solutionList = new List<Solution>();

        foreach (var solution in Population.Solutions)
        {
            var mutationId = Random.Next(4);
            Solution mutation = null;

            switch (mutationId)
            {
                case 0:
                    mutation = solution.SwapMutation();
                    mutation.Evaluate(Towns);
                    break;
                case 1:
                    mutation = solution.InsertMutation();
                    mutation.Evaluate(Towns);
                    break;
                case 2:
                    mutation = solution.InversionMutation();
                    mutation.Evaluate(Towns);
                    break;
                case 3:
                    mutation = solution.ScrambleMutation();
                    mutation.Evaluate(Towns);
                    break;
            }

            solutionList.Add(mutation);
        }

        var sortedSolutions = solutionList.Concat(Population.Solutions).OrderBy(solution => solution.Fitness).Take(PopulationSize).ToList();
        
        
        Population.Solutions.Clear();

        foreach (var solution in sortedSolutions)
        {
            Population.Solutions.Add(solution);
        }

        SelectedSolution = Population.Solutions.FirstOrDefault();
    }

}