using System.Collections.Generic;

namespace DecisionSystems.TSP
{
    public class TSPSpec
    {
        public TSPSpec(string name, IReadOnlyList<int> optimalTour, IReadOnlyList<Location> cities)
        {
            Name = name;
            OptimalTour = optimalTour;
            Cities = cities;
        }

        public string Name { get; }
        public IReadOnlyList<int> OptimalTour { get; }
        public IReadOnlyList<Location> Cities { get; }
    }
}