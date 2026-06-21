using System.Windows;
using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Storage.Towns.Register(TownDisplay);

        ButtonSwap.Click += ButtonSwap_Click;
        ButtonInsert.Click += ButtonInsert_Click;
        ButtonInverse.Click += ButtonInverse_Click;
        ButtonScramble.Click += ButtonScramble_Click;
    }

    private void ButtonSwap_Click(object sender, RoutedEventArgs e)
    {
        TownDisplay.SwapSolution();
        TownDisplay.InvalidateVisual();
    }

    private void ButtonInsert_Click(object sender, RoutedEventArgs e)
    {
        TownDisplay.InsertSolution();
        TownDisplay.InvalidateVisual();
    }

    private void ButtonInverse_Click(object sender, RoutedEventArgs e)
    {
        TownDisplay.InverseSolution();
        TownDisplay.InvalidateVisual();
    }

    private void ButtonScramble_Click(object sender, RoutedEventArgs e)
    {
        TownDisplay.ScrambleSolution();
        TownDisplay.InvalidateVisual();
    }
}