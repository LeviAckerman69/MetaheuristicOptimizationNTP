using MetaheuristicOptimizationNTP.Structures;
using System.Windows;
namespace MetaheuristicOptimizationNTP.ViewModel;

public partial class MainViewModel
{
    public void AddNewTownAtPosition(Point position)
    {
        if (FindTownAtPosition(position, 4) == null)
        {
            var id = Towns.Count + 1;
            var town = new Town { X = position.X, Y = position.Y, Name = $"Town {id}" };
            Towns.Add(town);

            DbContext.Towns.Add(town);
            DbContext.SaveChanges();
        }
    }

    public void RemoveTownAtPosition(Point position)
    {
        var town = FindTownAtPosition(position);
        if (town is not null)
        {
            Towns.Remove(town);
            DbContext.Towns.Remove(town);
            DbContext.SaveChanges();
        }
    }



    private Town? FindTownAtPosition(Point position, double scalingFactor = 1)
    {
        foreach (var town in Towns.Reverse())
        {
            if (town.ContainsAtScale(position, scalingFactor))
            {
                return town;
            }
        }

        return null;
    }
}

