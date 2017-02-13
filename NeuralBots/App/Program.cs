using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuralBotLib;

namespace App {
    class Program {
        static void Main(string[] args) {
            uint[] config = new uint[] { 2, 2, 1 };
            Func<int> rng = Genetics.GenHasher(129369, 192379);
            Individual[] individuals = new Individual[10].Select(_ => {
                Genetics.Chromosome cA = new Genetics.Chromosome(() => new Genetics.Gene(0), config);
                Genetics.Chromosome cB = new Genetics.Chromosome(() => new Genetics.Gene(0), config);
                return new Individual(rng, cA, cB);
            }).ToArray();
            
            Learning.TrainingExample[] exs_OR = new Learning.TrainingExample[] {
                new Learning.TrainingExample(new double[] { 0, 0 }, new double[] { 0 }),
                new Learning.TrainingExample(new double[] { 0, 1 }, new double[] { 1 }),
                new Learning.TrainingExample(new double[] { 1, 0 }, new double[] { 1 }),
                new Learning.TrainingExample(new double[] { 1, 1 }, new double[] { 1 })
            };

            Func<Individual, double> individualCost = individual => {
                double[] wData = individual.cA.ExpressWith(individual.cB, (gA, gB) => (gA.Data + gB.Data) * 1.0e-5);
                double[][][] wsss = Genetics.FoldExpression(wData, config);
                return Learning.Cost(exs_OR, input => Neural.Network(Neural.Sigmoid, wsss, input).ToArray());
            };

            //Func<Individual[], Individual[]> filterFittest = inds => {
            //    Individual[] 
            //    for (int i = 0; i < length; i++) {

            //    }
            //};

            double[] costs = individuals.Select(individualCost).ToArray();

            double totalCost = costs.Sum();

            // loop:
            //double chance = costs[i] / totalCost;
            double lottery = rng() % totalCost;

            //foreach (double cost in costs) {
            //    Console.WriteLine(cost);
            //}
        }


        class Individual {
            public Genetics.Chromosome cA { get; }
            public Genetics.Chromosome cB { get; }
            private Func<int> rng;

            public Individual(Func<int> rng, Genetics.Chromosome cA, Genetics.Chromosome cB) {
                this.cA = cA;
                this.cB = cB;
                this.rng = rng;
            }

            public Individual Mate(Individual mate, Func<Genetics.Gene, Genetics.Gene> mutator) {
                Genetics.Chromosome new_cA = (rng() % 2 == 0 ? cA : cB).Replicate(mutator);
                Genetics.Chromosome new_cB = (rng() % 2 == 0 ? mate.cA : mate.cB).Replicate(mutator);
                return new Individual(rng, new_cB, new_cA);
            }
        }
    }
}
