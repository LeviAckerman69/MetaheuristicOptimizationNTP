using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MetaheuristicOptimizationNTP.Structures;
using MetaheuristicOptimizationNTP.View;

namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class MainViewModel
{
    [NotifyPropertyChangedFor(nameof(IsLoggedIn))]
    [NotifyPropertyChangedFor(nameof(FormatCurrentUser))]
    [NotifyPropertyChangedFor(nameof(LoginText))]
    [ObservableProperty]
    public partial User? CurrentUser { get; set; } = null;

    public string FormatCurrentUser => CurrentUser is not null
        ? $"Current User: {CurrentUser.Name}"
        : "No user logged in.";

    public bool IsLoggedIn => CurrentUser != null;

    public string LoginText => IsLoggedIn ? "_Switch User..." : "_Login...";

    [RelayCommand]
    private void Register()
    {
        var registerViewModel = new RegisterViewModel();
        var registerView = new RegisterView(registerViewModel)
        {
            Owner = Application.Current.MainWindow
        };

        registerView.ShowDialog();

        if (registerView.DialogResult ?? false)
        {
            MessageBox.Show($"User {registerViewModel.Username} registered successfully.", "Registration Successful",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    [RelayCommand]
    private void Login()
    {
        var loginViewModel = new LoginViewModel();
        var loginView = new LoginView(loginViewModel)
        {
            Owner = Application.Current.MainWindow
        };

        loginView.ShowDialog();

        if (loginView.DialogResult ?? false)
        {
            CurrentUser = loginViewModel.SelectedUser;
        }
    }

    [RelayCommand]
    private void Logout()
    {
        CurrentUser = null;
    }
}