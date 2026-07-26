using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MetaheuristicOptimizationNTP.Services;
using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class LoginViewModel : DialogBaseViewModel
{
    private AuthenticationService AuthenticationService { get; } = new();

    [ObservableProperty]
    public partial string Username { get; set; }

    [ObservableProperty]
    public partial string Password { get; set; }

    public User? SelectedUser { get; private set; }

    [RelayCommand]
    private void Login()
    {
        var user = AuthenticationService.Authenticate(Username, Password);

        if (user != null)
        {
            SelectedUser = user;
            OnCloseRequested(true);
        }
        else
        {
            MessageBox.Show("Invalid username or password.");
        }
    }
}