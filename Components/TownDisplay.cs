using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.Components;

public class TownDisplay : FrameworkElement
{
    private ITownDisplayViewModel ViewModel => DataContext as ITownDisplayViewModel;

    public TownDisplay()
    {
        Loaded += (sender, args) =>
        {
            ViewModel.Towns.CollectionChanged += (sender, args) => InvalidateVisual();
        };
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

        if (ViewModel.FindTownAtPosition(position, 4) == null)
        {
            ViewModel.AddTownAt(position);
        }
    }
}