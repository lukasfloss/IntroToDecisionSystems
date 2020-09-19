using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionSystems.DataPrediction.Predictor
{
    public class PolynomialPredictionModel : IDataPredictionModel
    {
        private readonly IReadOnlyCollection<double> factors;

        public PolynomialPredictionModel(IReadOnlyCollection<double> factors)
        {
            this.factors = factors;
        }

        public double Test(double independentValue)
        {
            return factors
                .Select((factor, exponent) => factor * Math.Pow(independentValue, exponent))
                .Sum();
        }
    }
}