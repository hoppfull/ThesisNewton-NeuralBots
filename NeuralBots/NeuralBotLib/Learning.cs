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
            return ((double)rng()) / int.MaxValue;
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

        public static double[] TrainNeuralNetworkSelectiveBreeding(TrainingExample[] trainingSet, uint[] config, Func<int, int> rng, double threshold) {
            Genetics.Individual[] individuals = new int[30].Select(_ => {
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

            Tuple<Genetics.Individual, double>[] currentGen =
                individuals.Select(ind => Tuple.Create(ind, individualCost(ind))).OrderBy(c => c.Item2).ToArray();


            Tuple<Genetics.Individual, double>[] solutions =
                new Tuple<Genetics.Individual, double>[0];

            int runs = 0;
            while (solutions.Length == 0) {
                runs++;

                Tuple<Genetics.Individual, double> uber1 = currentGen[0];
                Tuple<Genetics.Individual, double> uber2 = currentGen[1];

                {
                    int _rn = 9479238;
                    Func<int> _rng = () => {
                        _rn = Genetics.Hash(_rn);
                        return _rn;
                    };
                    Random r = new Random();
                    Func<int> __rng = () => {
                        return r.Next();
                    };
                    currentGen = currentGen.AsParallel().Select(c => {
                        Genetics.Individual child = uber1.Item1.Mate(__rng, uber2.Item1, mutator);
                        return Tuple.Create(child, individualCost(child));
                    }).OrderBy(c => c.Item2).ToArray();
                }

                for (int i = 0; i < Math.Min(currentGen.Length, 10); i++)
                    Console.Write($"{string.Format("{0:0.000}", currentGen[i].Item2).PadRight(10)}");
                Console.WriteLine();

                solutions = currentGen.Where(ind => ind.Item2 <= threshold).ToArray();
            }

            Console.WriteLine($"runs: {runs}");
            return solutions.OrderBy(solution => solution.Item2).FirstOrDefault()?.Item1.Express(Genetics.DefaultGeneExpression);
        }

        public static double[] TrainNeuralNetworkRuleofTwo(TrainingExample[] trainingSet, uint[] config, Func<int, int> rng, double threshold) {
            Genetics.Individual[] individuals = new int[3].Select(_ => {
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


            Tuple<Genetics.Individual, double> daddy = Tuple.Create(individuals[0], individualCost(individuals[0]));
            Tuple<Genetics.Individual, double> mommy = Tuple.Create(individuals[1], individualCost(individuals[1]));
            Tuple<Genetics.Individual, double> child = Tuple.Create(individuals[2], individualCost(individuals[2]));

            int runs = 0;
            while (child.Item2 >= threshold) {
                runs++;
                
                if (daddy.Item2 > mommy.Item2) { // daddy worse than mommy?
                    daddy = daddy.Item2 > child.Item2 ? child : daddy;
                } else { // mommy worse than daddy?
                    mommy = mommy.Item2 > child.Item2 ? child : mommy;
                }

                {
                    int _rn = 9479238;
                    Func<int> _rng = () => {
                        _rn = Genetics.Hash(_rn);
                        return _rn;
                    };
                    Random r = new Random();
                    Func<int> __rng = () => {
                        return r.Next();
                    };


                    Genetics.Individual c = mommy.Item1.Mate(__rng, daddy.Item1, mutator);
                    child = Tuple.Create(c, individualCost(c));
                }
                Console.Write($"mommy: {string.Format("{0:0.000}", mommy.Item2)};   ");
                Console.Write($"daddy: {string.Format("{0:0.000}", daddy.Item2)};   ");
                Console.Write($"child: {string.Format("{0:0.000}", child.Item2)};");
                Console.WriteLine();
            }

            Console.WriteLine($"runs: {runs}");
            return child.Item1.Express(Genetics.DefaultGeneExpression);
        }

        public static double[] TrainNeuralNetworkRoulette(TrainingExample[] trainingSet, uint[] config, Func<int, int> rng, double threshold) {
            Genetics.Individual[] individuals = new int[30].Select(_ => {
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
                individuals.Select(ind => Tuple.Create(ind, individualCost(ind))).ToArray();

            Genetics.Individual solution = null;
            while (solution == null) {
                runs++;
                
                {
                    int _rn = 9479238;
                    Func<int> _rng = () => {
                        _rn = Genetics.Hash(_rn);
                        return _rn;
                    };

                    Random r = new Random();
                    Func<int> __rng = () => {
                        return r.Next();
                    };
                    
                    currentGen = new bool[currentGen.Count()].AsParallel().Select(_ => {
                        Genetics.Individual mommy = Roulette(currentGen, __rng);
                        Genetics.Individual daddy = Roulette(currentGen, __rng);
                        //Genetics.Individual daddy;
                        //for (int i = 0; ; i++) {
                        //    daddy = Roulette(currentGen, __rng);
                        //    if (mommy != daddy) break;
                        //}
                        Genetics.Individual child = mommy.Mate(__rng, daddy, mutator);
                        return Tuple.Create(child, individualCost(child));
                    }).OrderBy(c => c.Item2).ToArray();
                }
                
                for (int i = 0; i < Math.Min(currentGen.Length, 10); i++)
                    Console.Write($"{string.Format("{0:0.000}", currentGen[i].Item2).PadRight(10)}");
                Console.WriteLine();
                


                solution = currentGen[0].Item2 <= threshold ? currentGen[0].Item1 : null;
            }

            Console.WriteLine($"runs: {runs}");
            return solution.Express(Genetics.DefaultGeneExpression);
        }
    }
}
