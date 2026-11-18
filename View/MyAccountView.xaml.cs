using MetaheuristicOptimizationNTP.ViewModel;
using System.Windows;

namespace MetaheuristicOptimizationNTP.View;

public partial class MyAccountView : Window
{
    public MyAccountView(MyAccountViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        viewModel.CloseRequested += result => DialogResult = result;
    }
}
