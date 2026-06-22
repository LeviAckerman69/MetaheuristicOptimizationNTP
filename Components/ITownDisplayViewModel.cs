using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using MetaheuristicOptimizationNTP.Structures;

namespace MetaheuristicOptimizationNTP.Components
{
    public interface ITownDisplayViewModel
    {
        ObservableCollection<Town> Towns { get; }

        void AddTownAt(Point point);
        Town? FindTownAtPosition(Point position, double distance);
    }
}
