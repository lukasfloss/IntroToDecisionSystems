using System.Collections.Generic;

namespace DecisionSystems.TSP.Solver
{
    public interface ITSPSolver
    {
        List<int> Solve(IReadOnlyList<Location> cities);
    }
}