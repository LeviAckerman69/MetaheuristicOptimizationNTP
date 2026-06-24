using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using MetaheuristicOptimizationNTP.Structures;
using MetaheuristicOptimizationNTP.ViewModel;

namespace MetaheuristicOptimizationNTP.Components;

public class TownDisplay : FrameworkElement
{
    private MockViewModel MockViewModel { get; } = new();
    private ITownDisplayViewModel ViewModel => DataContext as ITownDisplayViewModel ?? MockViewModel;

    public TownDisplay()
    {
        Loaded += (sender, args) => { ViewModel.Towns.CollectionChanged += (sender, args) => InvalidateVisual(); };
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);
        drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));

        foreach (var town in ViewModel.Towns)
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

        if (ViewModel.FindTownAtPosition(position, 4.0) is not null)
        {
            var toolTip = new ToolTip
            {
                Content = "Town already exists here.",
                Placement = PlacementMode.MousePoint,
                PlacementTarget = Mouse.DirectlyOver as UIElement,
                StaysOpen = false,
                IsOpen = true
            };

            return;
        }

        ViewModel.AddTownAt(position);
    }
}