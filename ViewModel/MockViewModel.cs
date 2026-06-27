using MetaheuristicOptimizationNTP.Structures;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace MetaheuristicOptimizationNTP.ViewModel
{
    public class MockViewModel : IViewModel
    {
        public int PopulationSize { get; set; } = 0;
        public ObservableCollection<Town> Towns { get; } = new ObservableCollection<Town>();
        public Population Population { get; set; } = new Population();
        public Solution? SelectedSolution { get; set; }

        public void AddNewTownAtPosition(Point position)
        {
            throw new NotImplementedException();
        }

        public void RemoveTownAtPosition(Point position)
        {
            throw new NotImplementedException();
        }

        public void CreatePopulation()
        {
            throw new NotImplementedException();
        }
    }
}
