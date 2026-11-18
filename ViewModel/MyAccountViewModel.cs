using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MetaheuristicOptimizationNTP.Services;
using MetaheuristicOptimizationNTP.Structures;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class MyAccountViewModel : DialogBaseViewModel
{
    private readonly User _user;

    [ObservableProperty]
    public partial string Username { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string NewPassword { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string RepeatPassword { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;

    public bool AccountDeleted { get; private set; }

    public MyAccountViewModel(User user)
    {
        _user = user;
        Username = user.Name;
    }

    [RelayCommand]
    public void Save()
    {
        if (string.IsNullOrWhiteSpace(Username) || Username.Trim().Length < 5 || Username.Trim().Length > 60)
        {
            ErrorMessage = "Enter a username between 5 and 60 characters.";
            return;
        }

        if (NewPassword != RepeatPassword)
        {
            ErrorMessage = "Passwords do not match.";
            return;
        }

        try
        {
            if (!AuthenticationService.UpdateAccount(_user, Username, NewPassword))
            {
                ErrorMessage = "That username is already in use.";
                return;
            }
        }
        catch (DbUpdateException)
        {
            ErrorMessage = "Your account could not be saved. Please try again.";
            return;
        }

        OnCloseRequested(true);
    }

    [RelayCommand]
    public void DeleteAccount()
    {
        var result = MessageBox.Show("Permanently delete your account? You will be logged out.",
            "Delete account", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
        if (result != MessageBoxResult.Yes)
        {
            return;
        }

        try
        {
            AuthenticationService.DeleteAccount(_user);
        }
        catch (DbUpdateException)
        {
            ErrorMessage = "Your account could not be deleted. Please try again.";
            return;
        }

        AccountDeleted = true;
        OnCloseRequested(true);
    }
}
