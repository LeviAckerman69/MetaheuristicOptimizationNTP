using MetaheuristicOptimizationNTP.Structures;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class MainViewModel
{
    [RelayCommand(CanExecute = nameof(CanEditProblem))]
    public void ClearTowns()
    {
        if (IsEvolving || Towns.Count == 0)
        {
            return;
        }

        var result = MessageBox.Show("Delete all towns and clear the current population?", "Clear towns",
            MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        DbContext.Towns.RemoveRange(Towns);
        DbContext.SaveChanges();
        InvalidatePopulation();
        Towns.Clear();
    }

    public void AddNewTownAtPosition(Point position)
    {
        if (IsEvolving)
        {
            return;
        }

        var id = Towns.Count + 1;
        var town = new Town { X = position.X, Y = position.Y, Name = $"Town {id}" };
        InvalidatePopulation();
        Towns.Add(town);

        DbContext.Towns.Add(town);
        DbContext.SaveChanges();
    }

    public void RemoveTownAtPosition(Point position)
    {
        if (IsEvolving)
        {
            return;
        }

        var town = FindTownAtPosition(position);
        if (town is not null)
        {
            InvalidatePopulation();
            Towns.Remove(town);
            DbContext.Towns.Remove(town);
            DbContext.SaveChanges();
        }
    }

    public void MoveTownToPosition(Town town, Point position)
    {
        if (IsEvolving || !Towns.Contains(town) || town.Point == position ||
            !double.IsFinite(position.X) || !double.IsFinite(position.Y))
        {
            return;
        }

        var oldX = town.X;
        var oldY = town.Y;
        town.X = position.X;
        town.Y = position.Y;

        try
        {
            DbContext.SaveChanges();
        }
        catch (DbUpdateException)
        {
            town.X = oldX;
            town.Y = oldY;
            MessageBox.Show("The town could not be moved. Please try again.", "Save failed",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        InvalidatePopulation();
        OnPropertyChanged(nameof(Towns));
    }

    private void InvalidatePopulation()
    {
        SelectedSolution = null;
        Population.Solutions.Clear();
        _experimentStartedAt = null;
        _evolutionSteps.Clear();
        SaveExperimentRunCommand.NotifyCanExecuteChanged();
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
}

