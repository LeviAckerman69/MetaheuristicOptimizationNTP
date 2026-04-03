using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.Components;

public class TownDisplay : FrameworkElement, ITownsListener
{
    public void OnTownsChanged()
    {
        InvalidateVisual();
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);
        drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));
        foreach (var town in Storage.Towns.Get())
        {
            town.Draw(drawingContext);
        }
    }


    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        var position = e.GetPosition(this);
        if (FindTownAtPosition(position, 4) == null)
        {
            AddNewTown(position);
        }
    }

    private static void AddNewTown(Point position)
    {
        var id = Storage.Towns.Count + 1;
        var town = new Town { X = position.X, Y = position.Y, Name = $"Town {id}" };
        Storage.Towns.Add(town);
    }

    protected override void OnMouseRightButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseRightButtonDown(e);
        var position = e.GetPosition(this);
        var town = FindTownAtPosition(position);
        if (town is not null)
        {
            Storage.Towns.Remove(town);
        }
    }

    private static Town? FindTownAtPosition(Point position, double scalingFactor = 1)
    {
        foreach (var town in Storage.Towns.Get().Reverse())
        {
            if (town.ContainsAtScale(position, scalingFactor))
            {
                return town;
            }
        }

        return null;
    }
}