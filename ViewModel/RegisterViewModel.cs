using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class RegisterViewModel : ObservableValidator
{
    [ObservableProperty]
    public partial string Username { get; set; }
    
    [ObservableProperty]
    public partial string Password { get; set; }

    [ObservableProperty]
    public partial string RepeatPassword { get; set; }

    [RelayCommand]
    private void Register() { }
}