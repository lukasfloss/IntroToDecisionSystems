namespace DecisionSystems.DataPrediction.Predictor
{
    public class LinearPredictionModel : IDataPredictionModel
    {
        private readonly double k;
        private readonly double d;

        public LinearPredictionModel(double k, double d)
        {
            this.k = k;
            this.d = d;
        }

        public double Test(double independentValue)
        {
            return k * independentValue + d;
        }
    }
}