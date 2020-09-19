using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionSystems.DataPrediction.Predictor
{
    public class FastTreeDataPredictor : IDataPredictor
    {
        public IDataPredictionModel Train(IReadOnlyList<DataPoint> data)
        {
            MLContext mlContext = new MLContext(seed: 0);

            var dataPoints = data.Select(MLNetDataPoint.FromDomain);

            var dataView = mlContext.Data.LoadFromEnumerable(dataPoints);
            var pipeline = mlContext.Transforms
                .CopyColumns(outputColumnName: "Label", inputColumnName: nameof(MLNetDataPoint.DependentValue))
                .Append(mlContext.Transforms.Concatenate("Features", nameof(MLNetDataPoint.IndependentValue)))
                .Append(mlContext.Regression.Trainers.FastTree());
            var model = pipeline.Fit(dataView);

            Evaluate(mlContext, model, dataPoints);

            var predictionFunction = mlContext.Model.CreatePredictionEngine<MLNetDataPoint, MLNetDataPointPrediction>(model);
            return new MLNetDataPredictionModel(predictionFunction);
        }

        private static void Evaluate(MLContext mlContext, ITransformer model, IEnumerable<MLNetDataPoint> data)
        {
            var dataView = mlContext.Data.LoadFromEnumerable(data);
            var predictions = model.Transform(dataView);
            var metrics = mlContext.Regression.Evaluate(predictions, "Label");
            System.Diagnostics.Debug.WriteLine("");
            System.Diagnostics.Debug.WriteLine($"*************************************************");
            System.Diagnostics.Debug.WriteLine($"*       Model quality metrics evaluation         ");
            System.Diagnostics.Debug.WriteLine($"*------------------------------------------------");
            System.Diagnostics.Debug.WriteLine($"*       RSquared Score:      {metrics.RSquared:0.##}");
            System.Diagnostics.Debug.WriteLine($"*       Root Mean Squared Error:      {metrics.RootMeanSquaredError:#.##}");
        }

        private class MLNetDataPredictionModel : IDataPredictionModel
        {
            private readonly PredictionEngine<MLNetDataPoint, MLNetDataPointPrediction> predictionEngine;

            public MLNetDataPredictionModel(PredictionEngine<MLNetDataPoint, MLNetDataPointPrediction> predictionEngine)
            {
                this.predictionEngine = predictionEngine;
            }

            public double Test(double independentValue)
            {
                return predictionEngine.Predict(new MLNetDataPoint { IndependentValue = (float)independentValue, DependentValue = 0 }).Value;
            }
        }

        private class MLNetDataPoint
        {
            public float IndependentValue;
            public float DependentValue;

            public static MLNetDataPoint FromDomain(DataPoint dataPoint)
            {
                return new MLNetDataPoint
                {
                    IndependentValue = (float)dataPoint.IndependentValue,
                    DependentValue = (float)dataPoint.DependentValue
                };
            }
        }

        private class MLNetDataPointPrediction
        {
            [ColumnName("Score")]
            public float Value;
        }
    }
}