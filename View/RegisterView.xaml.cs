using System.Windows;
using MetaheuristicOptimizationNTP.ViewModel;

namespace MetaheuristicOptimizationNTP.View;

public partial class RegisterView : Window
{
    public RegisterView(RegisterViewModel viewModel)
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