using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MetaheuristicOptimizationNTP.Services;
using MetaheuristicOptimizationNTP.Structures;
using System.Windows;

namespace MetaheuristicOptimizationNTP.ViewModel
{
    public partial class LoginViewModel : DialogBaseViewModel
    {
        [ObservableProperty]
        public string Username { get; set; } = string.Empty;
        [ObservableProperty]
        public string Password { get; set; } = string.Empty;
        public User SelectedUser { get; private set; }

        [RelayCommand]
        public void Login()
        {
            var user = AuthenticationService.Authenticate(Username, Password);
            if (user == null)
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SelectedUser = user;

            OnCloseRequested(true);


        }
    }
}
