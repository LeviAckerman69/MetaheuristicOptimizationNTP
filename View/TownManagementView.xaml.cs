using MetaheuristicOptimizationNTP.ViewModel;
using System.Windows;

namespace MetaheuristicOptimizationNTP.View;

public partial class TownManagementView : Window
{
    public TownManagementView(TownManagementViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        viewModel.CloseRequested += result => DialogResult = result;
    }
}
