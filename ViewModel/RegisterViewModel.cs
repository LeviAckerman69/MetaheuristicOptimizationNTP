using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MetaheuristicOptimizationNTP.Services;

namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class RegisterViewModel : DialogBaseViewModel
{
    [ObservableProperty]
    public partial string Username { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string Password { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string RepeatPassword { get; set; } = string.Empty;

    [RelayCommand]
    private void Register()
    {
        var username = Username;
        var password = Password;

        if (!AuthenticationService.Register(username, password))
        {
            MessageBox.Show("Username already exists. Please choose a different username.", "Registration Failed",
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        OnCloseRequested(true);
    }
}