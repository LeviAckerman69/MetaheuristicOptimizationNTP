using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class SaveExperimentRunViewModel : DialogBaseViewModel
{
    [ObservableProperty]
    public partial string RunName { get; set; } = $"Run {DateTime.Now:g}";

    [ObservableProperty]
    public partial string Notes { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;

    [RelayCommand]
    public void Save()
    {
        if (string.IsNullOrWhiteSpace(RunName) || RunName.Trim().Length > 100)
        {
            ErrorMessage = "Enter a run name between 1 and 100 characters.";
            return;
        }

        OnCloseRequested(true);
    }
}
