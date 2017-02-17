using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuralBotLib;

namespace App {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("OR:");
            TestOr();
            Console.WriteLine("\nAND:");
            TestAND();
            Console.WriteLine("\nXOR:");
            TestXOR();
        }

        static void Test() {
            Learning.TrainingExample[] exs = new Learning.TrainingExample[] {
                new Learning.TrainingExample(new double[] { 0, 0 }, new double[] { 0 }),
                new Learning.TrainingExample(new double[] { 0, 1 }, new double[] { 1 }),
                new Learning.TrainingExample(new double[] { 1, 0 }, new double[] { 1 }),
                new Learning.TrainingExample(new double[] { 1, 1 }, new double[] { 1 })
            };
        }

        static void TestOr() {
            uint[] config = new uint[] { 2, 2, 1 };

            Learning.TrainingExample[] exs_OR = new Learning.TrainingExample[] {
                new Learning.TrainingExample(new double[] { 0, 0 }, new double[] { 0 }),
                new Learning.TrainingExample(new double[] { 0, 1 }, new double[] { 1 }),
                new Learning.TrainingExample(new double[] { 1, 0 }, new double[] { 1 }),
                new Learning.TrainingExample(new double[] { 1, 1 }, new double[] { 1 })
            };

            double[] solution = Learning.TrainNeuralNetwork(exs_OR, config, Genetics.Hash);

            double[][][] wsss = Neural.FoldExpression(solution, config);

            double result0 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 0, 0 }).ToArray()[0];
            double result1 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 1, 0 }).ToArray()[0];
            double result2 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 0, 1 }).ToArray()[0];
            double result3 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 1, 1 }).ToArray()[0];

            Console.WriteLine($"false Or false = {result0}");
            Console.WriteLine($"true Or false = {result1}");
            Console.WriteLine($"false Or true = {result2}");
            Console.WriteLine($"true Or true = {result3}");
        }

        static void TestAND() {
            uint[] config = new uint[] { 2, 2, 1 };

            Learning.TrainingExample[] exs_AND = new Learning.TrainingExample[] {
                new Learning.TrainingExample(new double[] { 0, 0 }, new double[] { 0 }),
                new Learning.TrainingExample(new double[] { 0, 1 }, new double[] { 0 }),
                new Learning.TrainingExample(new double[] { 1, 0 }, new double[] { 0 }),
                new Learning.TrainingExample(new double[] { 1, 1 }, new double[] { 1 })
            };

            double[] solution = Learning.TrainNeuralNetwork(exs_AND, config, Genetics.Hash);

            double[][][] wsss = Neural.FoldExpression(solution, config);

            double result0 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 0, 0 }).ToArray()[0];
            double result1 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 1, 0 }).ToArray()[0];
            double result2 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 0, 1 }).ToArray()[0];
            double result3 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 1, 1 }).ToArray()[0];

            Console.WriteLine($"false Or false = {result0}");
            Console.WriteLine($"true Or false = {result1}");
            Console.WriteLine($"false Or true = {result2}");
            Console.WriteLine($"true Or true = {result3}");
        }

        static void TestXOR() {
            uint[] config = new uint[] { 2, 2, 1 };

            Learning.TrainingExample[] exs_XOR = new Learning.TrainingExample[] {
                new Learning.TrainingExample(new double[] { 0, 0 }, new double[] { 0 }),
                new Learning.TrainingExample(new double[] { 0, 1 }, new double[] { 1 }),
                new Learning.TrainingExample(new double[] { 1, 0 }, new double[] { 1 }),
                new Learning.TrainingExample(new double[] { 1, 1 }, new double[] { 0 })
            };

            double[] solution = Learning.TrainNeuralNetwork(exs_XOR, config, Genetics.Hash);

            double[][][] wsss = Neural.FoldExpression(solution, config);

            double result0 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 0, 0 }).ToArray()[0];
            double result1 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 1, 0 }).ToArray()[0];
            double result2 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 0, 1 }).ToArray()[0];
            double result3 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 1, 1 }).ToArray()[0];

            Console.WriteLine($"false Or false = {result0}");
            Console.WriteLine($"true Or false = {result1}");
            Console.WriteLine($"false Or true = {result2}");
            Console.WriteLine($"true Or true = {result3}");
        }
    }
}
