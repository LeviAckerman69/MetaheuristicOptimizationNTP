using System.Windows;
using System.Windows.Controls;
using MetaheuristicOptimizationNTP.Structures;
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