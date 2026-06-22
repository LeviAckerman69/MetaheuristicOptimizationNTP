using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.Components;

public class TownDisplay : FrameworkElement, ITownsListener, IPopulationListHelperListener
{
    // private static Solution solution = new Solution(Storage.Towns);
    //public void OnTownsChanged()
    //{
    //    solution = new Solution(Storage.Towns);
    //    InvalidateVisual();
    //}

    public void OnTownsChanged()
    {
        InvalidateVisual();
    }

    public void OnSelectionChanged()
    {
        InvalidateVisual();
    }

    //public void SwapSolution()
    //{
    //    solution = solution.SwapMutation();
    //}

    //public void InsertSolution()
    //{
    //    solution = solution.InsertMutation();
    //}

    //public void InverseSolution()
    //{
    //    solution = solution.InversionMutation();
    //}

    //public void ScrambleSolution()
    //{
    //    solution = solution.ScrambleMutation();
    //}

    protected override void OnRender(DrawingContext drawingContext)
    {
        base.OnRender(drawingContext);
        drawingContext.DrawRectangle(Brushes.Transparent, null, new Rect(RenderSize));

        var towns = Storage.Towns.TownsView;

        foreach (var town in towns)
        {
            town.Draw(drawingContext);
        }


        if (Storage.PopulationListHelper is not null)
        {
            var solution = Storage.PopulationListHelper.SelectedSolution;

            if (solution is not null)
            {
                var permutation = solution.PermutationView;

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
        foreach (var town in Storage.Towns.TownsView.Reverse())
        {
            if (town.ContainsAtScale(position, scalingFactor))
            {
                return town;
            }
        }

        return null;
    }
}