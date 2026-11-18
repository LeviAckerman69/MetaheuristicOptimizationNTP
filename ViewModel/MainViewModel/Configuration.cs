using CommunityToolkit.Mvvm.Input;
using MetaheuristicOptimizationNTP.View;
using System.Windows;

namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class MainViewModel
{
    public ConfigurationDialogViewModel ConfigurationDialogViewModel { get; } = new();

    [RelayCommand(CanExecute = nameof(CanEditProblem))]
    public void OpenConfigurationDialog()
    {
        if (IsEvolving)
        {
            return;
        }

        var configurationDialogViewModelCopy = new ConfigurationDialogViewModel();
        configurationDialogViewModelCopy.Fill(ConfigurationDialogViewModel);

        var dialog = new ConfigurationDialog(configurationDialogViewModelCopy)
        {
            Owner = Application.Current.MainWindow
        };
        dialog.ShowDialog();

        if (dialog.DialogResult == true)
        {
            ConfigurationDialogViewModel.Fill(configurationDialogViewModelCopy);
        }
    }

}

