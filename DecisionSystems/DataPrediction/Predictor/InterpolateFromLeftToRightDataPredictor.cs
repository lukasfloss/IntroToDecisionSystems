using System.Collections.Generic;
using System.Linq;

namespace DecisionSystems.DataPrediction.Predictor
{
    public class InterpolateFromLeftToRightDataPredictor : IDataPredictor
    {
        public IDataPredictionModel Train(IReadOnlyList<DataPoint> data)
        {
            var left = data.MinBy(dataPoint => dataPoint.IndependentValue);
            var right = data.MaxBy(dataPoint => dataPoint.IndependentValue);
            var dx = right.IndependentValue - left.IndependentValue;
            var dy = right.DependentValue - left.DependentValue;
            var k = dy / dx;
            var d = left.DependentValue - k * left.IndependentValue;
            return new LinearPredictionModel(k, d);
        }
    }
}