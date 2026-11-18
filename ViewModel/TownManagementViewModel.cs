using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MetaheuristicOptimizationNTP.Structures;
using System.Collections.ObjectModel;

namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class TownManagementViewModel : DialogBaseViewModel
{
    public ObservableCollection<Town> Towns { get; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial Town? SelectedTown { get; set; }

    [ObservableProperty]
    public partial string TownName { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string XText { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string YText { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;

    public double EditedX { get; private set; }
    public double EditedY { get; private set; }

    public TownManagementViewModel(ObservableCollection<Town> towns)
    {
        Towns = towns;
        SelectedTown = towns.FirstOrDefault();
    }

    partial void OnSelectedTownChanged(Town? value)
    {
        TownName = value?.Name ?? string.Empty;
        XText = value?.X.ToString() ?? string.Empty;
        YText = value?.Y.ToString() ?? string.Empty;
        ErrorMessage = string.Empty;
    }

    private bool CanSave => SelectedTown is not null;

    [RelayCommand(CanExecute = nameof(CanSave))]
    public void Save()
    {
        if (SelectedTown is null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(TownName) || TownName.Trim().Length > 100)
        {
            ErrorMessage = "Enter a town name between 1 and 100 characters.";
            return;
        }

        if (!double.TryParse(XText, out var x) || !double.IsFinite(x) ||
            !double.TryParse(YText, out var y) || !double.IsFinite(y))
        {
            ErrorMessage = "Enter valid, finite numbers for X and Y.";
            return;
        }

        EditedX = x;
        EditedY = y;
        ErrorMessage = string.Empty;
        OnCloseRequested(true);
    }
}
