using System;
using System.Collections.Generic;
using System.Linq;

// http://derekwill.com/2015/03/05/bit-processing-in-c/
namespace NeuralBotLib {
    public static class Genetics {
        public class Gene {
            public int Data { get; }
            public Gene(int Data) { this.Data = Data; }
        }

        public class Chromosome {
            public Gene[] Genes { get; }
            public Chromosome(Gene[] Genes) {
                this.Genes = Genes;
            }

            public Chromosome Replicate(Func<Gene, Gene> mutator) {
                return new Chromosome(Genes.Select(mutator).ToArray());
            }

            public double[] ExpressWith(Chromosome chromosome, Func<Gene, Gene, double> expressGene) {
                return Genes.Zip(chromosome.Genes, expressGene).ToArray();
            }
        }

        public class Individual {
            public Chromosome cA { get; }
            public Chromosome cB { get; }

            public Individual(Chromosome cA, Chromosome cB) {
                this.cA = cA;
                this.cB = cB;
            }

            public Individual Mate(Func<int> rng, Individual mate, Func<Gene, Gene> mutator) {
                Chromosome new_cA = (rng() % 2 == 0 ? cA : cB).Replicate(mutator);
                Chromosome new_cB = (rng() % 2 == 0 ? mate.cA : mate.cB).Replicate(mutator);
                return new Individual(new_cA, new_cB);
            }

            public double[] Express(Func<Gene, Gene, double> expressGenes) {
                return cA.ExpressWith(cB, expressGenes);
            }
        }

        #region Utility functions
        public static int Fastrand(int seed) {
            return ((214013 * seed + 2531011) >> 16) & 0x7FFF;
        }

        public static int Hash(int seed) {
            int x = seed;
            x = (x ^ 61) ^ (x >> 16);
            x += x << 3;
            x ^= x >> 4;
            x *= 0x27d4eb2d;
            x ^= x >> 15;
            return x;
        }

        public static IEnumerable<T> Generate<T>(Func<T, T> generate, T seed) {
            T rn = generate(seed);
            while (true) {
                yield return rn;
                rn = generate(rn);
            }
        }

        public static T Recur<T>(int n, T start, Func<T, T> function) {
            T result = start;
            for (int i = 0; i < n; i++)
                result = function(result);
            return result;
        }

        public static Func<Gene, Gene> CreateDefaultGeneMutator(int seed) {
            int rn = seed;
            return gene => {
                rn = Hash(rn);
                return new Gene(gene.Data ^ (1 << rn % 31));
            };
        }

        public static double DefaultGeneExpression(Gene geneA, Gene geneB) {
            return (geneA.Data + geneB.Data) * 1.0e-5;
        }
        #endregion
    }
}
