using System.Windows;

namespace MetaheuristicOptimizationNTP.Structures;

public class Town
{
    public const double Radius = 15;
    public double X { get; init; }
    public double Y { get; init; }
    public required string Name { get; init; }

    public Point Point => new(X, Y);

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
}