using System;
using System.Collections.Generic;
using System.Linq;

namespace DecisionSystems.DataPrediction.Predictor
{
    public class GeneticAlgorithmDataPredictor : IDataPredictor
    {
        public class Individual
        {
            public Individual(IReadOnlyList<double> factors, double fitness)
            {
                Factors = factors;
                Fitness = fitness;
            }

            public IReadOnlyList<double> Factors { get; }
            public double Fitness { get; }
        }

        private readonly int order;
        private readonly int iterations;

        public GeneticAlgorithmDataPredictor(int order, int iterations)
        {
            if (order < 0)
            {
                throw new ArgumentException("Order must not be negative.");
            }
            if (iterations < 1)
            {
                throw new ArgumentException("Number of iterations must be >= 1.");
            }

            this.order = order;
            this.iterations = iterations;
        }

        public IDataPredictionModel Train(IReadOnlyList<DataPoint> data)
        {
            Random generator = new Random();

            var minY = data.Min(dataPoint => dataPoint.DependentValue);
            var maxY = data.Max(dataPoint => dataPoint.DependentValue);

            double getFitness(IReadOnlyList<double> factors)
            {
                var prediction = data
                    .Select(dataPoint => new PolynomialPredictionModel(factors).Test(dataPoint.IndependentValue));
                return -Utils.MeanSquaredError(data, prediction);
            }

            Individual createIndividual(IReadOnlyList<double> factors)
            {
                return new Individual(factors, getFitness(factors));
            }

            var populationSize = 500;
            var initialPopulation = Enumerable
                .Range(0, populationSize)
                .Select(p =>
                {
                    var factors = Enumerable
                        .Range(0, order + 1)
                        .Select(_ => generator.NextDouble() * 100 - 50)
                        .ToList();
                    return createIndividual(factors);
                })
                .ToList();

            Individual tournamentSelect(IReadOnlyList<Individual> individuals)
            {
                var tournamentSize = 2;
                return Enumerable
                    .Range(0, int.MaxValue)
                    .Select(_ => generator.Next(0, individuals.Count))
                    .Distinct()
                    .Take(tournamentSize)
                    .Select(i => individuals[i])
                    .MaxBy(individual => individual.Fitness);
            }

            Individual averageCrossover(Individual parent1, Individual parent2)
            {
                var factors = parent1.Factors
                    .Zip(parent2.Factors, (a, b) => (a + b) / 2)
                    .ToList();
                return createIndividual(factors);
            }

            Individual mutateAllFactors(Individual individual)
            {
                var factors = individual.Factors
                    .Select(f =>
                    {
                        var mutationFactor = generator.NextDouble() / 10 + 0.95;
                        return f * mutationFactor;
                    })
                    .ToList();
                return createIndividual(factors);
            }

            var population = initialPopulation;
            var bestIndividual = population.MaxBy(individual => individual.Fitness);
            for (int i = 0; i < iterations; i++)
            {
                Console.WriteLine($"Iteration #{i}. Best individual: {string.Join(", ", bestIndividual.Factors)}, {bestIndividual.Fitness}");
                population = Enumerable
                    .Range(0, populationSize)
                    .Select(_ =>
                    {
                        var parent1 = tournamentSelect(population);
                        var parent2 = tournamentSelect(population);
                        var child = averageCrossover(parent1, parent2);
                        var mutatedChild = mutateAllFactors(child);
                        return mutatedChild;
                    })
                    .ToList();
                bestIndividual = population
                    .Append(bestIndividual)
                    .MaxBy(individual => individual.Fitness);
            }
            return new PolynomialPredictionModel(bestIndividual.Factors);
        }
    }
}