using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using MetaheuristicOptimizationNTP.Structures;
using MetaheuristicOptimizationNTP.ViewModel;

namespace MetaheuristicOptimizationNTP.Components;

public class TownDisplay : FrameworkElement
{
    private MockViewModel MockViewModel { get; } = new();
    private ITownDisplayViewModel ViewModel => DataContext as ITownDisplayViewModel ?? MockViewModel;

    public TownDisplay()
    {
        Loaded += (sender, args) =>
        {
            ViewModel.Towns.CollectionChanged += (sender, args) => InvalidateVisual();
            ViewModel.Population.Solutions.CollectionChanged += (sender, args) => InvalidateVisual();
            (ViewModel as ObservableObject)?.PropertyChanged += (sender, args) => InvalidateVisual();
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
        var towns = ViewModel.Towns;
        var solution = ViewModel.SelectedSolution;
        if (solution is not null)
        {
            for (var step = 0; step < towns.Count; step++)
            {
                var townId1 = solution.PermutationView[step];
                var town1 = towns[townId1];
                var point1 = town1.Point;

                var townId2 = solution.PermutationView[(step + 1) % towns.Count];
                var town2 = towns[townId2];
                var point2 = town2.Point;

                var pen = new Pen(Brushes.Black, 2);
                drawingContext.DrawLine(pen, point1, point2);
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

        ViewModel.AddTownAtPointCommand.Execute(position);
    }

    protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseRightButtonDown(e);
        var position = e.GetPosition(this);

        ViewModel.RemoveTownAtPointCommand.Execute(position);
    }
}