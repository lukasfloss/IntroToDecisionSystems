using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionSystems.TSP
{
    public static class Utils
    {
        public static double GetDistance(Location a, Location b)
        {
            var dx = a.X - b.X;
            var dy = a.Y - b.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static double GetDistance(IReadOnlyCollection<int> solution, IReadOnlyList<Location> cities)
        {
            var previousCityIndex = solution.Last();
            var distance = 0.0;

            // Iterate through the list.
            foreach (var cityIndex in solution)
            {
                var previousCity = cities[previousCityIndex - 1];
                var city = cities[cityIndex - 1];
                distance += GetDistance(previousCity, city);
                previousCityIndex = cityIndex;
            }
            return distance;
           
            
        }
    }
}