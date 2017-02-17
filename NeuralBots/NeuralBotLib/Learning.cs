using System;
using System.Collections.Generic;
using System.Linq;

namespace NeuralBotLib {
    public static class Learning {
        public class TrainingExample {
            public double[] Input { get; }
            public double[] Output { get; }
            public TrainingExample(double[] Input, double[] Output) {
                this.Input = Input;
                this.Output = Output;
            }
        }

        public static double Cost(TrainingExample[] exs, Func<double[], double[]> hypothesis) {
            Func<double, double, double> w = (y, yHat) => Math.Pow(y - yHat, 2);
            return exs.Aggregate(0d, (acc, ex) => acc + ex.Output.Zip(hypothesis(ex.Input), w).Sum());
        }

        private class Gambler<T> {
            public T Candidate { get; }
            public double LotteryNumber { get; }
            public Gambler(T Candidate, double LotteryNumber) {
                this.Candidate = Candidate;
                this.LotteryNumber = LotteryNumber;
            }
        }

        private static IEnumerable<Gambler<T>> CostToRouletteValues<T>(IEnumerable<Tuple<T, double>> candidates) {
            double totalCost = candidates.Aggregate(0d, (acc, candidate) => acc + candidate.Item2);
            double totalWin = totalCost * (candidates.Count() - 1);
            return candidates.Select(candidate => new Gambler<T>(candidate.Item1, (totalCost - candidate.Item2) / totalWin));
        }

        private static double lotto(Func<int> rng) {
            return (rng() / 2d) / int.MaxValue + 0.5;
        }

        private static T Pick<T>(IEnumerable<Gambler<T>> gamblers, double lotteryWinner) {
            double lotteryStack = 0;
            return (gamblers.FirstOrDefault(gambler => {
                lotteryStack += gambler.LotteryNumber;
                return lotteryStack >= lotteryWinner;
            }) ?? gamblers.Last()).Candidate;
        }

        public static T Roulette<T>(IEnumerable<Tuple<T, double>> candidates, Func<int> rng) {
            return Pick<T>(CostToRouletteValues(candidates), lotto(rng));
        }

        public static double[] TrainNeuralNetwork(TrainingExample[] trainingSet, uint[] config, Func<int, int> rng) {
            Genetics.Individual[] individuals = new int[10].Select(_ => {
                int nFeatures = (int)Neural.NWeightsFromConfig(config);
                Genetics.Gene[] genesA = Genetics.Generate(rng, 48334).Select(x => new Genetics.Gene(x)).Take(nFeatures).ToArray();
                Genetics.Gene[] genesB = Genetics.Generate(rng, 94353).Select(x => new Genetics.Gene(x)).Take(nFeatures).ToArray();
                Genetics.Chromosome cA = new Genetics.Chromosome(genesA);
                Genetics.Chromosome cB = new Genetics.Chromosome(genesB);
                return new Genetics.Individual(cA, cB);
            }).ToArray();

            Func<Genetics.Individual, double> individualCost = individual => {
                double[] wData = individual.Express(Genetics.DefaultGeneExpression);
                double[][][] wsss = Neural.FoldExpression(wData, config);
                return Cost(trainingSet, input => Neural.Network(Neural.Sigmoid, wsss, input).ToArray());
            };

            Func<Genetics.Gene, Genetics.Gene> mutator = Genetics.CreateDefaultGeneMutator(546794);
            
            int runs = 0;
            Tuple<Genetics.Individual, double>[] currentGen =
                individuals.Select(ind => Tuple.Create(ind, individualCost(ind))).ToArray();

            Tuple<Genetics.Individual, double>[] solutions =
                new Tuple<Genetics.Individual, double>[0];

            while (solutions.Length == 0) {
                runs++;

                currentGen = new bool[currentGen.Count()].Select(_ => {
                    Genetics.Individual mommy = Roulette(currentGen, () => Genetics.Recur(0, 394583, rng));
                    Genetics.Individual daddy = Roulette(currentGen, () => Genetics.Recur(0, 947393, rng));
                    Genetics.Individual child = mommy.Mate(() => Genetics.Recur(0, 938752, rng), daddy, mutator);
                    return Tuple.Create(child, individualCost(child));
                }).ToArray();
                
                solutions = currentGen.Where(ind => ind.Item2 < 0.1).ToArray();
            }

            //Console.WriteLine($"runs: {runs}");
            return solutions.OrderBy(solution => solution.Item2).First().Item1.Express(Genetics.DefaultGeneExpression);
        }
    }
}
