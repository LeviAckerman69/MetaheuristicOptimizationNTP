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
    }

    private void ButtonMutate_Click(object sender, RoutedEventArgs e)
    {
        TownDisplay.MutateSolution();
        TownDisplay.InvalidateVisual();
    }
}