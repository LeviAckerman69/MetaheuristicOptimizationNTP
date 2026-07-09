using System.Windows;
using MetaheuristicOptimizationNTP.ViewModel;

namespace MetaheuristicOptimizationNTP.View
{
    public partial class ConfigurationDialog : Window
    {
        public ConfigurationDialog(ConfigurationDialogViewModel configurationDialogViewModel)
        {
            InitializeComponent();
            DataContext = configurationDialogViewModel;
        }   
    }
}