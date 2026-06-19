using System.Windows;

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

        ButtonMutate.Click += ButtonMutate_Click;
        ButtonInverse.Click += ButtonInverse_Click;
        ButtonScramble.Click += ButtonScramble_Click;
    }

    private void ButtonMutate_Click(object sender, RoutedEventArgs e)
    {
        TownDisplay.MutateSolution();
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