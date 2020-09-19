namespace DecisionSystems.DataPrediction
{
    public interface IDataPredictionModel
    {
        double Test(double independentValue);
    }
}