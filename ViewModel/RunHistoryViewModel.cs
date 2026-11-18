using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MetaheuristicOptimizationNTP.Database;
using MetaheuristicOptimizationNTP.Structures;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.IO;
using System.Windows.Media.Imaging;

namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class RunHistoryViewModel : DialogBaseViewModel
{
    private readonly TspDbContext _dbContext;
    private readonly Guid _userId;

    public string[] SortOptions { get; } = ["Newest first", "Oldest first", "Name A–Z", "Name Z–A", "Distance ascending", "Distance descending"];

    [ObservableProperty]
    public partial string SelectedSort { get; set; } = "Newest first";
    [ObservableProperty]
    public partial string NameFilter { get; set; } = string.Empty;
    [ObservableProperty]
    public partial DateTime? FromDate { get; set; }
    [ObservableProperty]
    public partial DateTime? ToDate { get; set; }
    [ObservableProperty]
    public partial string MaxDistanceText { get; set; } = string.Empty;
    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;
    [ObservableProperty]
    public partial List<ExperimentRun> Runs { get; private set; } = [];
    [ObservableProperty]
    public partial List<EvolutionStep> Steps { get; private set; } = [];
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PreviewMessage))]
    public partial BitmapImage? RoutePreview { get; private set; }

    public string PreviewMessage => SelectedRun is null ? "Select a run to view its route."
        : RoutePreview is null ? "No route preview was saved for this run." : string.Empty;
    [ObservableProperty]
    public partial string EditedName { get; set; } = string.Empty;
    [ObservableProperty]
    public partial string EditedNotes { get; set; } = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveChangesCommand))]
    [NotifyCanExecuteChangedFor(nameof(DeleteRunCommand))]
    [NotifyPropertyChangedFor(nameof(PreviewMessage))]
    public partial ExperimentRun? SelectedRun { get; set; }

    public RunHistoryViewModel(TspDbContext dbContext, Guid userId)
    {
        _dbContext = dbContext;
        _userId = userId;
        LoadRuns();
    }

    partial void OnSelectedSortChanged(string value) => Refresh();

    partial void OnSelectedRunChanged(ExperimentRun? value)
    {
        EditedName = value?.Name ?? string.Empty;
        EditedNotes = value?.Notes ?? string.Empty;
        Steps = value is null ? [] : _dbContext.EvolutionSteps.AsNoTracking()
            .Where(step => step.ExperimentRunId == value.Id).OrderBy(step => step.StepNumber).ToList();
        RoutePreview = null;
        if (value?.RoutePreview is { Length: > 0 })
        {
            using var stream = new MemoryStream(value.RoutePreview);
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = stream;
            image.EndInit();
            image.Freeze();
            RoutePreview = image;
        }
    }

    [RelayCommand]
    public void Refresh() => LoadRuns(SelectedRun?.Id);

    [RelayCommand]
    public void ClearFilters()
    {
        NameFilter = string.Empty;
        FromDate = null;
        ToDate = null;
        MaxDistanceText = string.Empty;
        Refresh();
    }

    private void LoadRuns(Guid? selectedId = null)
    {
        ErrorMessage = string.Empty;
        SelectedRun = null;
        Runs = [];

        if (FromDate.HasValue && ToDate.HasValue && FromDate.Value.Date > ToDate.Value.Date)
        {
            ErrorMessage = "The start date must not be after the end date.";
            return;
        }

        var query = _dbContext.ExperimentRuns.AsNoTracking().Include(run => run.User)
            .Where(run => run.UserId == _userId);
        if (!string.IsNullOrWhiteSpace(NameFilter))
        {
            var name = NameFilter.Trim().ToLowerInvariant();
            query = query.Where(run => run.Name.ToLower().Contains(name));
        }
        if (FromDate.HasValue)
        {
            var from = FromDate.Value.Date;
            query = query.Where(run => run.StartedAt >= from);
        }
        if (ToDate.HasValue && ToDate.Value.Date < DateTime.MaxValue.Date)
        {
            var until = ToDate.Value.Date.AddDays(1);
            query = query.Where(run => run.StartedAt < until);
        }
        if (!string.IsNullOrWhiteSpace(MaxDistanceText))
        {
            if (!double.TryParse(MaxDistanceText, out var distance) || !double.IsFinite(distance) || distance < 0)
            {
                ErrorMessage = "Enter a valid, non-negative maximum distance.";
                return;
            }
            query = query.Where(run => run.FinalBestDistance <= distance);
        }

        query = SelectedSort switch
        {
            "Oldest first" => query.OrderBy(run => run.StartedAt).ThenBy(run => run.Id),
            "Name A–Z" => query.OrderBy(run => run.Name).ThenBy(run => run.Id),
            "Name Z–A" => query.OrderByDescending(run => run.Name).ThenBy(run => run.Id),
            "Distance ascending" => query.OrderBy(run => run.FinalBestDistance).ThenBy(run => run.Id),
            "Distance descending" => query.OrderByDescending(run => run.FinalBestDistance).ThenBy(run => run.Id),
            _ => query.OrderByDescending(run => run.StartedAt).ThenBy(run => run.Id)
        };
        Runs = query.ToList();
        SelectedRun = Runs.FirstOrDefault(run => run.Id == selectedId) ?? Runs.FirstOrDefault();
    }

    private bool HasSelectedRun => SelectedRun is not null;

    [RelayCommand(CanExecute = nameof(HasSelectedRun))]
    public void SaveChanges()
    {
        if (SelectedRun is null)
        {
            return;
        }
        if (string.IsNullOrWhiteSpace(EditedName) || EditedName.Trim().Length > 100)
        {
            ErrorMessage = "Enter a run name between 1 and 100 characters.";
            return;
        }

        var run = _dbContext.ExperimentRuns.SingleOrDefault(run => run.Id == SelectedRun.Id && run.UserId == _userId);
        if (run is null)
        {
            Refresh();
            ErrorMessage = "This run no longer exists.";
            return;
        }
        var oldName = run.Name;
        var oldNotes = run.Notes;
        run.Name = EditedName.Trim();
        run.Notes = EditedNotes;
        try
        {
            _dbContext.SaveChanges();
        }
        catch (DbUpdateException)
        {
            run.Name = oldName;
            run.Notes = oldNotes;
            ErrorMessage = "The run could not be saved. Please try again.";
            return;
        }
        LoadRuns(run.Id);
    }

    [RelayCommand(CanExecute = nameof(HasSelectedRun))]
    public void DeleteRun()
    {
        if (SelectedRun is null || MessageBox.Show("Delete this saved run and all its evolution steps?", "Delete run",
            MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) != MessageBoxResult.Yes)
        {
            return;
        }

        var run = _dbContext.ExperimentRuns.SingleOrDefault(run => run.Id == SelectedRun.Id && run.UserId == _userId);
        if (run is not null)
        {
            _dbContext.ExperimentRuns.Remove(run);
            try
            {
                _dbContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                _dbContext.Entry(run).State = EntityState.Unchanged;
                ErrorMessage = "The run could not be deleted. Please try again.";
                return;
            }
        }
        Refresh();
    }
}
