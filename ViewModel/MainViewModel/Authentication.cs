using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MetaheuristicOptimizationNTP.Structures;
using MetaheuristicOptimizationNTP.View;

namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class MainViewModel
{
    [NotifyPropertyChangedFor(nameof(FormatCurrentUser))]
    [NotifyPropertyChangedFor(nameof(LoginText))]
    [NotifyCanExecuteChangedFor(nameof(OpenMyAccountCommand))]
    [NotifyCanExecuteChangedFor(nameof(SaveExperimentRunCommand))]
    [NotifyCanExecuteChangedFor(nameof(OpenRunHistoryCommand))]
    [ObservableProperty]
    public partial User? CurrentUser { get; set; }

    public string FormatCurrentUser
    {
        get
        {
            if (CurrentUser == null)
            {
                return "Not logged in.";
            }
            return $"Logged in as {CurrentUser.Name}.";
        }
    }

    public string LoginText => CurrentUser == null ? "Login" : "Logout";

    private bool CanOpenMyAccount => CurrentUser is not null;

    [RelayCommand(CanExecute = nameof(CanOpenMyAccount))]
    public void OpenMyAccount()
    {
        if (CurrentUser is null)
        {
            return;
        }

        var viewModel = new MyAccountViewModel(CurrentUser);
        var dialog = new MyAccountView(viewModel) { Owner = System.Windows.Application.Current.MainWindow };
        if (dialog.ShowDialog() == true)
        {
            if (viewModel.AccountDeleted)
            {
                CurrentUser = null;
            }
            else
            {
                OnPropertyChanged(nameof(FormatCurrentUser));
            }
        }
    }

    [RelayCommand]
    public void Register()
    {
        var registerViewModel = new RegisterViewModel();
        var registerView = new RegisterView(registerViewModel)
        {
            Owner = System.Windows.Application.Current.MainWindow
        };
        registerView.ShowDialog();
    }

    [RelayCommand]
    public void Login()
    {
        if (CurrentUser == null)
        {
            var loginViewModel = new LoginViewModel();
            var loginView = new LoginView(loginViewModel)
            {
                Owner = System.Windows.Application.Current.MainWindow
            };
            loginView.ShowDialog();

            if (loginView.DialogResult == true)
            {
                CurrentUser = loginViewModel.SelectedUser;
            }
        }
        else
        {
            CurrentUser = null;
        }
    }



}

