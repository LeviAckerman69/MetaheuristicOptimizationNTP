using CommunityToolkit.Mvvm.Input;
using MetaheuristicOptimizationNTP.View;
using System.Windows;

namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class MainViewModel
{
    public ConfigurationDialogViewModel ConfigurationDialogViewModel { get; } = new();

    [RelayCommand]
    public void OpenConfigurationDialog()
    {
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

