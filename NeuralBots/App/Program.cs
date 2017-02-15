using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuralBotLib;

namespace App {
    class Program {
        static void Main(string[] args) {
            //uint[] config = new uint[] { 2, 2, 1 };
            //Func<int> rng = Genetics.GenHasher(12346, 192379);
            //Individual[] currentGen = new Individual[10].Select(_ => {
            //    Genetics.Chromosome cA = new Genetics.Chromosome(() => new Genetics.Gene(rng()), config);
            //    Genetics.Chromosome cB = new Genetics.Chromosome(() => new Genetics.Gene(rng()), config);
            //    return new Individual(rng, cA, cB);
            //}).ToArray();

            //Learning.TrainingExample[] exs_OR = new Learning.TrainingExample[] {
            //    new Learning.TrainingExample(new double[] { 0, 0 }, new double[] { 0 }),
            //    new Learning.TrainingExample(new double[] { 0, 1 }, new double[] { 1 }),
            //    new Learning.TrainingExample(new double[] { 1, 0 }, new double[] { 1 }),
            //    new Learning.TrainingExample(new double[] { 1, 1 }, new double[] { 1 })
            //};

            //Func<Individual, double> individualCost = individual => {
            //    double[] wData = individual.cA.ExpressWith(individual.cB, (gA, gB) => (gA.Data + gB.Data) * 1.0e-5);
            //    double[][][] wsss = Genetics.FoldExpression(wData, config);
            //    return Learning.Cost(exs_OR, input => Neural.Network(Neural.Sigmoid, wsss, input).ToArray());
            //};
            //var mutator = Genetics.GenMutator(48648468);
            //Individual[] finest = new Individual[0];
            //int runs = 0;
            //while (finest.Length == 0) {
            //    runs++;
            //    double[] costs = currentGen.Select(individualCost).ToArray();

            //    foreach (var item in costs) {
            //        Console.Write($"{item} ");
            //    }
            //    Console.WriteLine();


            //    Individual[] nextGen = new Individual[currentGen.Length];
            //    for (int i = 0; i < nextGen.Length; i++) {
            //        nextGen[i] = Roulette(currentGen, costs, rng).Mate(Roulette(currentGen, costs, rng), mutator);
            //    }
            //    currentGen = nextGen;

            //    finest = currentGen.Where(x => individualCost(x) < 0.1).ToArray();


            //}

            //Console.WriteLine($"\nFinished after {runs} runs\n");

            //foreach (var item in finest) {
            //    double[][][] wsss = Genetics.FoldExpression(item.Express(), config);
            //    double result00 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 0, 0 }).ToArray()[0];
            //    double result10 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 1, 0 }).ToArray()[0];
            //    double result01 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 0, 1 }).ToArray()[0];
            //    double result11 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 1, 1 }).ToArray()[0];
            //    Console.WriteLine($"false OR false = {result00 == 1}");
            //    Console.WriteLine($"true OR false = {result10 == 1}");
            //    Console.WriteLine($"false OR true = {result01 == 1}");
            //    Console.WriteLine($"true OR true = {result11 == 1}");

            //}
            //Console.ReadKey();
        }
    }
}
