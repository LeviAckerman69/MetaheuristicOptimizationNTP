using CommunityToolkit.Mvvm.Input;
using MetaheuristicOptimizationNTP.Structures;
using MetaheuristicOptimizationNTP.View;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class MainViewModel
{
    private DateTime? _experimentStartedAt;
    private double _initialBestDistance;

    private bool CanSaveExperimentRun => CurrentUser is not null && !IsEvolving &&
        _experimentStartedAt.HasValue && Population.Count > 0 && Towns.Count >= 3;

    [RelayCommand(CanExecute = nameof(CanSaveExperimentRun))]
    public void SaveExperimentRun()
    {
        if (!CanSaveExperimentRun)
        {
            return;
        }

        var finalBestDistance = Population.Solutions.Min(solution => solution.Fitness);
        if (!double.IsFinite(_initialBestDistance) || !double.IsFinite(finalBestDistance))
        {
            MessageBox.Show("The run has an invalid distance and cannot be saved.", "Cannot save run");
            return;
        }

        var viewModel = new SaveExperimentRunViewModel();
        var dialog = new SaveExperimentRunView(viewModel) { Owner = Application.Current.MainWindow };
        if (dialog.ShowDialog() != true)
        {
            return;
        }

        var run = new ExperimentRun
        {
            UserId = CurrentUser!.Id,
            Name = viewModel.RunName.Trim(),
            StartedAt = _experimentStartedAt!.Value,
            InitialBestDistance = _initialBestDistance,
            FinalBestDistance = finalBestDistance,
            Notes = viewModel.Notes
        };
        DbContext.ExperimentRuns.Add(run);
        try
        {
            DbContext.SaveChanges();
            MessageBox.Show("The experiment run was saved.", "Run saved");
        }
        catch (DbUpdateException)
        {
            DbContext.Entry(run).State = EntityState.Detached;
            MessageBox.Show("The experiment run could not be saved. Please try again.", "Save failed",
                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
