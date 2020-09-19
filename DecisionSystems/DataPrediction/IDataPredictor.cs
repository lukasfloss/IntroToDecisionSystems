using System.Collections.Generic;

namespace DecisionSystems.DataPrediction
{
    public interface IDataPredictor
    {
        IDataPredictionModel Train(IReadOnlyList<DataPoint> data);
    }
}