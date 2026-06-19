using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.Components;

public class TownDisplay : FrameworkElement, ITownsListener
{
    private static Solution solution = new Solution(Storage.Towns);
    public void OnTownsChanged()
    {
        solution = new Solution(Storage.Towns);
        InvalidateVisual();
    }

    public void MutateSolution()
    {
        solution = solution.SwapMutation();
    }

    public void InverseSolution()
    {
        solution = solution.InversionMutation();
    }

    public void ScrambleSolution()
    {
        solution = solution.ScrambleMutation();
    }

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);
        drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));
        foreach (var town in Storage.Towns.Get())
        {
            town.Draw(drawingContext);
        }

        var towns = Storage.Towns.Get();

       
        var permutation = solution.Permutation;

        for (var i = 0; i < permutation.Count; i++)
        {   
            var townID1 = permutation[i];
            var town1 = towns[townID1];
            var point1 = new Point(town1.X, town1.Y);

            var townID2 = permutation[(i + 1) % permutation.Count];
            var town2 = towns[townID2];
            var point2 = new Point(town2.X, town2.Y);

            drawingContext.DrawLine(new Pen(Brushes.Black, 2), point1, point2);
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