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

    public Point Point => new Point(X, Y);

    public bool ContainsAtScale(Point other, double scalingFactor = 1)
    {
        var position = Point;
        var difference = other - position;
        var maxDistance = scalingFactor * Radius;
        return difference.Length <= maxDistance;
    }

    public double DistanceTo(Town other)
    {
        var distance = Point - other.Point;

        return distance.Length;
    }

    public bool Contains(Point other)
    {
        return ContainsAtScale(other);
    }

    public void Draw(DrawingContext drawingContext)
    {
        var point = this.Point;
        var brush = Brushes.Red;
        var pen = new Pen(Brushes.Black, 2);
        drawingContext.DrawEllipse(brush, pen, point, Radius, Radius);
    }
}