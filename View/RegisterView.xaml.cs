using System.Windows;
using MetaheuristicOptimizationNTP.ViewModel;

namespace MetaheuristicOptimizationNTP.View;

public partial class RegisterView : Window
{
    public RegisterView(LoginViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;

        viewModel.CloseRequested += result =>
        {
            DialogResult = result;
            Close();
        };
    }
}