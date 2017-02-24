using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
                Genetics.Gene[] genesA = Genetics.Generate(rng, 48334).Select(x => new Genetics.Gene((short)x)).Take(nFeatures).ToArray();
                Genetics.Gene[] genesB = Genetics.Generate(rng, 94353).Select(x => new Genetics.Gene((short)x)).Take(nFeatures).ToArray();
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
                individuals.Select(ind => Tuple.Create(ind, individualCost(ind))).OrderBy(c => c.Item2).ToArray();


            Tuple<Genetics.Individual, double>[] solutions =
                new Tuple<Genetics.Individual, double>[0];

            while (solutions.Length == 0) {
                runs++;

                Tuple<Genetics.Individual, double> uber1 = currentGen[0];
                Tuple<Genetics.Individual, double> uber2 = currentGen[1];

                //Tuple<Genetics.Individual, double>[] nextGen = new bool[currentGen.Count()].AsParallel().Select(_ => {
                //    Genetics.Individual mommy = Roulette(currentGen, () => Genetics.Recur(0, 394583, rng));
                //    Genetics.Individual daddy;
                //    for (int i = 0; ; i++) {
                //        daddy = Roulette(currentGen, () => Genetics.Recur(i, 947393, rng));
                //        if (mommy != daddy) break;
                //    }
                //    Genetics.Individual child = mommy.Mate(() => Genetics.Recur(0, 938752, rng), daddy, mutator);
                //    return Tuple.Create(child, individualCost(child));
                //}).OrderBy(c => c.Item2).ToArray();

                //for (int i = 0; i < currentGen.Length; i++) {
                //    Genetics.Individual child = uber1.Item1.Mate(() => Genetics.Recur(i, 948539, rng), uber2.Item1, mutator);
                //    currentGen[i] = Tuple.Create(child, individualCost(child));
                //}
                {
                    int _rn = 9479238;
                    Func<int> _rng = () => {
                        _rn = Genetics.Hash(_rn);
                        return _rn;
                    };
                    currentGen = currentGen.AsParallel().Select(c => {
                        Genetics.Individual child = uber1.Item1.Mate(_rng, uber2.Item1, mutator);
                        return Tuple.Create(child, individualCost(child));
                    }).OrderBy(c => c.Item2).ToArray();
                }
                
                //currentGen[0] = currentGen[0].Item2 < uber1.Item2 ? currentGen[0] : uber1;
                //currentGen[1] = currentGen[1].Item2 < uber2.Item2 ? currentGen[1] : uber2;

                //nextGen[nextGen.Length - 1] = currentGen[0];
                //nextGen[nextGen.Length - 2] = currentGen[1];
                //nextGen[nextGen.Length - 3] = currentGen[2];
                //nextGen[nextGen.Length - 4] = currentGen[3];
                //nextGen[nextGen.Length - 5] = currentGen[4];


                for (int i = 0; i < Math.Min(currentGen.Length, 10); i++)
                    Console.Write($"{string.Format("{0:0.000}", currentGen[i].Item2).PadRight(10)}");
                Console.WriteLine();

                //currentGen = nextGen.OrderBy(c => c.Item2).ToArray();
                solutions = currentGen.Where(ind => ind.Item2 < 0.1).ToArray();
            }

            Console.WriteLine($"runs: {runs}");
            return solutions.OrderBy(solution => solution.Item2).FirstOrDefault()?.Item1.Express(Genetics.DefaultGeneExpression);
        }
    }
}
