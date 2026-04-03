using System.Windows;
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;
using Pen = System.Windows.Media.Pen;

namespace MetaheuristicOptimizationNTP.Structures;

public class Town
{
    private const double Radius = 15;
    public double X { get; init; }
    public double Y { get; init; }
    public required string Name { get; init; }

    public bool ContainsAtScale(Point other, double scalingFactor = 1)
    {
        var position = new Point(X, Y);
        var difference = other - position;
        var maxDistance = scalingFactor * Radius;
        return difference.Length <= maxDistance;
    }

    public bool Contains(Point other)
    {
        return ContainsAtScale(other);
    }

    public void Draw(DrawingContext drawingContext)
    {
        var point = new Point(X, Y);
        var brush = Brushes.Red;
        var pen = new Pen(Brushes.Black, 2);
        drawingContext.DrawEllipse(brush, pen, point, Radius, Radius);
    }
}