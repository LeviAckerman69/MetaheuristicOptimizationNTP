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
        Storage.Towns.Register(TownDisplay);

        ListPopulation.SelectionChanged += ListPopulationOnSelectionChanged;

        //ButtonSwap.Click += ButtonSwap_Click;
        //ButtonInsert.Click += ButtonInsert_Click;
        //ButtonInverse.Click += ButtonInverse_Click;
        //ButtonScramble.Click += ButtonScramble_Click;
    }

    private void ListPopulationOnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        Storage.PopulationListHelper.SelectedIndex = ListPopulation.SelectedIndex;
    }

    //private void ButtonSwap_Click(object sender, RoutedEventArgs e)
    //{
    //    TownDisplay.SwapSolution();
    //    TownDisplay.InvalidateVisual();
    //}

    //private void ButtonInsert_Click(object sender, RoutedEventArgs e)
    //{
    //    TownDisplay.InsertSolution();
    //    TownDisplay.InvalidateVisual();
    //}

    //private void ButtonInverse_Click(object sender, RoutedEventArgs e)
    //{
    //    TownDisplay.InverseSolution();
    //    TownDisplay.InvalidateVisual();
    //}

    //private void ButtonScramble_Click(object sender, RoutedEventArgs e)
    //{
    //    TownDisplay.ScrambleSolution();
    //    TownDisplay.InvalidateVisual();
    //}

    private void ButtonCreatePopulation_Click(object sender, RoutedEventArgs e)
    {
        Storage.Population = new Population(Storage.Towns, 1000);
        Storage.PopulationListHelper = new PopulationListHelper(Storage.Population, ListPopulation);
        Storage.PopulationListHelper.Register(TownDisplay);
        TownDisplay.InvalidateVisual();

        ListPopulation.ItemsSource = Storage.PopulationListHelper.SolutionsFormatted;
    }
}