using MetaheuristicOptimizationNTP.ViewModel;
using System.Windows;

namespace MetaheuristicOptimizationNTP.View;

public partial class SaveExperimentRunView : Window
{
    public SaveExperimentRunView(SaveExperimentRunViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        viewModel.CloseRequested += result => DialogResult = result;
    }
}
