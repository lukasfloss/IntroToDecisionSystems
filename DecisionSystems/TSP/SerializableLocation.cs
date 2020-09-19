namespace DecisionSystems.TSP
{
    public class SerializableLocation
    {
        public double X { get; set; }
        public double Y { get; set; }

        public static Location ToDomain(SerializableLocation location)
        {
            return new Location(location.X, location.Y);
        }
    }
}
