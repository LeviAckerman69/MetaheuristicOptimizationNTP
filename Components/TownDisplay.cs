using CommunityToolkit.Mvvm.ComponentModel;
using MetaheuristicOptimizationNTP.Structures;
using MetaheuristicOptimizationNTP.ViewModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MetaheuristicOptimizationNTP.Components;

public class TownDisplay : FrameworkElement
{
    private static MockViewModel MockViewModel { get; } = new();

    private IViewModel ViewModel => DataContext as IViewModel ?? MockViewModel;

    public TownDisplay()
    {
        Loaded += (s, e) =>
        {
            ViewModel.Towns.CollectionChanged += (s, e) => InvalidateVisual();
            (ViewModel as ObservableObject)?.PropertyChanged += (s, e) => InvalidateVisual();
        };
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);
        drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));

        DrawPaths(drawingContext);

        DrawTowns(drawingContext);
    }

    private void DrawPaths(DrawingContext drawingContext)
    {
        var selectedSolution = ViewModel.SelectedSolution;

        var towns = ViewModel.Towns;

        if (selectedSolution is not null)
        {
            var permutation = selectedSolution.PermutationView;

            for (var i = 0; i < permutation.Count; i++)
            {
                var townId1 = permutation[i];
                var town1 = towns[townId1];
                var point1 = town1.Point;

                var townId2 = permutation[(i + 1) % permutation.Count];
                var town2 = towns[townId2];
                var point2 = town2.Point;

                drawingContext.DrawLine(new Pen(Brushes.Black, 2), point1, point2);
            }
        }
    }

    private void DrawTowns(DrawingContext drawingContext)
    {
        var towns = ViewModel.Towns;

        foreach (var town in towns)
        {
            var point = town.Point;
            var brush = Brushes.Red;
            var pen = new Pen(Brushes.Black, 2);
            drawingContext.DrawEllipse(brush, pen, point, Town.Radius, Town.Radius);
        }
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        var position = e.GetPosition(this);
        ViewModel.AddNewTownAtPosition(position);
    }


    protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseRightButtonDown(e);
        var position = e.GetPosition(this);
        ViewModel.RemoveTownAtPosition(position);
    }
}