using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuralBotLib;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            uint[] config = new uint[] { 2, 2, 1 };
            Func<int> rng = Genetics.GenHasher(129369, 192379);
            Individual[] individuals = new Individual[3].Select(_ =>
            {
                Genetics.Chromosome cA = new Genetics.Chromosome(() => new Genetics.Gene(rng()), config);
                Genetics.Chromosome cB = new Genetics.Chromosome(() => new Genetics.Gene(rng()), config);
                return new Individual(rng, cA, cB);
            }).ToArray();

            Learning.TrainingExample[] exs_OR = new Learning.TrainingExample[] {
                new Learning.TrainingExample(new double[] { 0, 0 }, new double[] { 0 }),
                new Learning.TrainingExample(new double[] { 0, 1 }, new double[] { 1 }),
                new Learning.TrainingExample(new double[] { 1, 0 }, new double[] { 1 }),
                new Learning.TrainingExample(new double[] { 1, 1 }, new double[] { 1 })
            };

            Func<Individual, double> individualCost = individual =>
            {
                double[] wData = individual.cA.ExpressWith(individual.cB, (gA, gB) => (gA.Data + gB.Data) * 1.0e-5);
                double[][][] wsss = Genetics.FoldExpression(wData, config);
                return Learning.Cost(exs_OR, input => Neural.Network(Neural.Sigmoid, wsss, input).ToArray());
            };


            double[] mockCosts = new double[] { 1,1,1 };

            Individual i1 = individuals[0];
            Individual i2 = individuals[1];
            Individual i3 = individuals[2];

            int ni1 = 0;
            int ni2 = 0;
            int ni3 = 0;


            for (int i = 0; i < 1e4; i++)
            {
                Individual winner = Roulette(individuals, mockCosts, rng);
                if (winner == i1) ni1++;
                if (winner == i2) ni2++;
                if (winner == i3) ni3++;
                //Console.WriteLine(((double)rng())/int.MaxValue);
            }
            //double avg = 0;
            //for (int i = 0; i < 1000; i++)
            //{
            //    avg += (rng()/2d) / int.MaxValue+0.5;
            //}
            //Console.WriteLine(avg/1000);

            Console.WriteLine($"ind1 = {ni1/100}%");
            Console.WriteLine($"ind2 = {ni2/100}%");
            Console.WriteLine($"ind3 = {ni3/100}%");





            Console.ReadLine();
        }

        static double[] CostToRouletteValues(double[] costs)
        {
            double sum = costs.Sum();
            double[] temp = costs.Select(x => sum - x).ToArray();
            return temp.Select(x => x / temp.Sum()).ToArray();
        }

        static Individual Roulette(Individual[] individuals, double[] costs, Func<int> rng)
        {
            double[] rouletteValues = CostToRouletteValues(costs);
            double lottoWinner = (rng() / 2d) / int.MaxValue + 0.5;
            double lottoStack = 0;
            for (int i = 0; i < individuals.Length; i++)
            {
                lottoStack += rouletteValues[i];
                if (lottoStack >= lottoWinner) return individuals[i];
            }
            return individuals.Last();
        }


        class Individual
        {
            public Genetics.Chromosome cA { get; }
            public Genetics.Chromosome cB { get; }
            private Func<int> rng;

            public Individual(Func<int> rng, Genetics.Chromosome cA, Genetics.Chromosome cB)
            {
                this.cA = cA;
                this.cB = cB;
                this.rng = rng;
            }

            public Individual Mate(Individual mate, Func<Genetics.Gene, Genetics.Gene> mutator)
            {
                Genetics.Chromosome new_cA = (rng() % 2 == 0 ? cA : cB).Replicate(mutator);
                Genetics.Chromosome new_cB = (rng() % 2 == 0 ? mate.cA : mate.cB).Replicate(mutator);
                return new Individual(rng, new_cB, new_cA);
            }
        }
    }
}
