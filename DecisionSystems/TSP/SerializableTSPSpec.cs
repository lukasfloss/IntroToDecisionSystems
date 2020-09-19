using System.Linq;

namespace DecisionSystems.TSP
{
    public class SerializableTSPSpec
    {
        public string Name { get; set; }
        public int[] OptimalTour { get; set; }
        public SerializableLocation[] Cities { get; set; }

        public static TSPSpec ToDomain(SerializableTSPSpec spec)
        {
            return new TSPSpec(
                spec.Name,
                spec.OptimalTour.ToList(),
                spec.Cities.Select(SerializableLocation.ToDomain).ToList()
            );
        }
    }
}
