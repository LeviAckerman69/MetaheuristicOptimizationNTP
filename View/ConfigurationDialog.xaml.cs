using MetaheuristicOptimizationNTP.ViewModel;
using System.Windows;

namespace MetaheuristicOptimizationNTP.View;

public partial class ConfigurationDialog : Window
{
    public ConfigurationDialog(ConfigurationDialogViewModel configurationDialogViewModel)
    {
        InitializeComponent();
        DataContext = configurationDialogViewModel;

        configurationDialogViewModel.CloseRequested += result =>
        {
            DialogResult = result;
            Close();
        };
    }
}