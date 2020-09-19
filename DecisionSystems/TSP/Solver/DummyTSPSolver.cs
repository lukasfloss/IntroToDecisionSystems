using System.Linq;
using System.Collections.Generic;

namespace DecisionSystems.TSP.Solver
{
    public class DummyTSPSolver : ITSPSolver
    {
        public List<int> Solve(IReadOnlyList<Location> cities)
        {
            return Enumerable.Range(1, cities.Count).ToList();
        }
    }
}