using MetaheuristicOptimizationNTP.Components;
using MetaheuristicOptimizationNTP.Structures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace MetaheuristicOptimizationNTP.ViewModel
{
    public class MainViewModel : IViewModel

    {
        public int PopulationSize { get; set; } = 100;
        public ObservableCollection<Town> Towns { get; } = new();
        public Population Population { get; set; } = new Population();
        public Solution? SelectedSolution { get; set; } = null;

        public void CreatePopulation()
        {
            Population.Populate(Towns, PopulationSize);
        }

        public void AddNewTownAtPosition(Point position)
        {
            if (FindTownAtPosition(position, 4) == null)
            {
                var id = Towns.Count + 1;
                var town = new Town { X = position.X, Y = position.Y, Name = $"Town {id}" };
                Towns.Add(town);
            }
        }

        public void RemoveTownAtPosition(Point position)
        {
            var town = FindTownAtPosition(position);
            if (town is not null)
            {
                Towns.Remove(town);
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
}