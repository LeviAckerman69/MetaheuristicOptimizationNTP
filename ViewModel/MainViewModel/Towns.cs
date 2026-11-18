using CommunityToolkit.Mvvm.Input;
using MetaheuristicOptimizationNTP.View;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class MainViewModel
{
    [RelayCommand(CanExecute = nameof(CanEditProblem))]
    public void OpenTownManagement()
    {
        if (IsEvolving)
        {
            return;
        }

        var viewModel = new TownManagementViewModel(Towns);
        var dialog = new TownManagementView(viewModel) { Owner = Application.Current.MainWindow };
        if (dialog.ShowDialog() != true || viewModel.SelectedTown is null)
        {
            return;
        }

        var town = viewModel.SelectedTown;
        var oldName = town.Name;
        var oldX = town.X;
        var oldY = town.Y;
        town.Name = viewModel.TownName.Trim();
        town.X = viewModel.EditedX;
        town.Y = viewModel.EditedY;

        try
        {
            DbContext.SaveChanges();
        }
        catch (DbUpdateException)
        {
            town.Name = oldName;
            town.X = oldX;
            town.Y = oldY;
            MessageBox.Show("The town could not be saved. Please try again.", "Save failed",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        if (town.X != oldX || town.Y != oldY)
        {
            InvalidatePopulation();
        }

        OnPropertyChanged(nameof(Towns));
    }
}
