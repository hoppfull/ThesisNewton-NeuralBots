using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
