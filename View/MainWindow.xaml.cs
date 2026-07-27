using System.Windows;
using MetaheuristicOptimizationNTP.ViewModel;

namespace MetaheuristicOptimizationNTP;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}