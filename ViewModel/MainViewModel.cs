using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MetaheuristicOptimizationNTP.Components;
using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class MainViewModel : ObservableValidator, ITownDisplayViewModel
{
    public MainViewModel()
    {
        Towns.CollectionChanged += (sender, args) =>
        {
            Population.Solutions.Clear();
        };
    }

    private CancellationTokenSource? Cts { get; set; }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Range(5, int.MaxValue, ErrorMessage = "Enter population size >= 5.")]
    public partial int PopulationSize { get; set; } = 10;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(EvolveText))]
    public partial bool IsEvolving { get; set; } = false;

    private int NextTownId { get; set; }

    public ObservableCollection<Solution> Solution { get; } = [];

    [ObservableProperty]
    public partial Solution? SelectedSolution { get; set; }

    public ObservableCollection<Town> Towns { get; } = [];

    public Population Population { get; } = new();

    public void AddTownAt(Point point)
    {
        NextTownId++;
        var town = new Town
        {
            Name = $"Town #{NextTownId}",
            X = point.X,
            Y = point.Y
        };
        Towns.Add(town);
    }

    public Town? FindTownAtPosition(Point position, double scalingFactor = 1)
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

    public bool RemoveTown(Town town)
    {
        return Towns.Remove(town);
    }

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

        Population.Populate(Towns.ToList(), PopulationSize);
        SelectedSolution = Population.Solutions[0];
    }

    [RelayCommand]
    public void Step()
    {
        var idA = Population.RankPick();
        var idB = Population.RankPick();

        while (idA == idB)
        {
            idB = Population.RankPick();
        }

        var solutionA = Population.Solutions[idA];
        var solutionB = Population.Solutions[idB];

        var crossover = solutionA.OrderCrossover(solutionB);
        var mutated = crossover.SwapMutation();

        var rank = Population.Solutions.Select(solution => solution.Fitness)
            .TakeWhile(fitness => fitness < mutated.Fitness).Count();

        Population.Solutions.Insert(rank, mutated);
        Population.Solutions.RemoveAt(Population.Solutions.Count - 1);

        SelectedSolution = Population.Solutions[0];
    }

    public string EvolveText => IsEvolving ? "Stop" : "Evolve";

    [RelayCommand]
    public async void Evolve()
    {
        if (IsEvolving)
        {
            Cts?.Cancel();
            return;
        }

        IsEvolving = true;
        Cts = new CancellationTokenSource();
        var token = Cts.Token;

        try
        {
            while (!token.IsCancellationRequested)
            {
                // await Task.Run(Step, Cts.Token);
                Step();
                await Task.Delay(1, token);
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            Cts.Dispose();
            Cts = null;

            IsEvolving = false;
        }
    }
}