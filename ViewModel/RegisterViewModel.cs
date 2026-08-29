using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MetaheuristicOptimizationNTP.Services;
using System.Windows;

namespace MetaheuristicOptimizationNTP.ViewModel
{
    public partial class RegisterViewModel : DialogBaseViewModel
    {
        [ObservableProperty]
        public string Username { get; set; } = string.Empty;
        [ObservableProperty]
        public string Password { get; set; } = string.Empty;
        [ObservableProperty]
        public string RepeatPassword { get; set; } = string.Empty;

        [RelayCommand]
        public void Register()
        {
            if (Password != RepeatPassword)
            {
                MessageBox.Show("Passwords do not match.", "Registration Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var success = AuthenticationService.Register(Username, Password);
            if (!success)
            {
                MessageBox.Show("Registration failed.", "Registration Failed", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            MessageBox.Show("Registration successful.", "Registration Successful", MessageBoxButton.OK, MessageBoxImage.Information);

            OnCloseRequested(true);
        }
    }
}
