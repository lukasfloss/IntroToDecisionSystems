using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DecisionSystems.TSP.Solver
{
    public class BacktrackingTSPSolver : ITSPSolver
    {
        public List<int> Solve(IReadOnlyList<Location> cities)
        {
            var minLength = double.MaxValue;
            var baseTour = Enumerable.Range(1, cities.Count).ToArray();
            List<int[]> tours = new List<int[]>();
            CalculatePermutations(baseTour, 1, tours);

            List<int> solution = null;
            foreach (var tour in tours)
            {
                var tourList = tour.ToList();
                var length = Utils.GetDistance(tourList, cities);
                if (length < minLength)
                {
                    minLength = length;
                    solution = tourList;
                }
            }
            Debug.Assert(solution != null);
            return solution;
        }

        private static void CalculatePermutations(int[] data, int startIndex, List<int[]> permutations)
        {
            if (startIndex == data.Length - 1)
            {
                permutations.Add(data.ToArray());
            }
            else
            {
                // Calculate permutations by placing permutation[startIndex] at every possible index
                // and then calculate permutations of elements with index > startIndex
                for (int i = startIndex; i < data.Length; i++)
                {
                    Swap(data, startIndex, i);
                    CalculatePermutations(data, startIndex + 1, permutations);
                    Swap(data, startIndex, i);
                }
            }
        }

        private static void Swap(int[] data, int a, int b)
        {
            int temp = data[a];
            data[a] = data[b];
            data[b] = temp;
        }
    }
}
