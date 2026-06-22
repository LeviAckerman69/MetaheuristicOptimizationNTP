using System.Windows;
using MetaheuristicOptimizationNTP.ViewModel;

namespace MetaheuristicOptimizationNTP;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}