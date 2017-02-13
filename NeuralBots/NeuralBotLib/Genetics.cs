using System;
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
            public uint[] Config { get; }
            public Chromosome(Func<Gene> gene, uint[] Config) {
                this.Config = Config;
                Genes = new int[LengthOfGenes(Config)].Select(_ => gene()).ToArray();
            }

            public Chromosome(Gene[] Genes, uint[] Config) {
                this.Config = Config;
                this.Genes = Genes;
            }

            public Chromosome Replicate(Func<Gene, Gene> mutate) {
                return new Chromosome(Genes.Select(mutate).ToArray(), Config);
            }

            public double[] ExpressWith(Chromosome chromosome, Func<Gene, Gene, double> expressGene) {
                double[] result = new double[Math.Min(Genes.Length, chromosome.Genes.Length)];
                for (int i = 0; i < result.Length; i++) result[i] = expressGene(Genes[i], chromosome.Genes[i]);
                return result;
            }
        }

        private static uint LengthOfGenes(uint[] config) {
            uint result = 0;
            for (int i = 1; i < config.Length; i++) result += (config[i - 1] + 1) * config[i];
            return result;
        }

        public static double[][][] FoldExpression(double[] geneExpression, uint[] config) {
            double[][][] result = new double[Math.Max(config.Length - 1, 0)][][];
            int gene = 0;
            for (int i = 0; i < result.Length; i++) {
                result[i] = new double[config[i + 1]][];
                for (int j = 0; j < result[i].Length; j++) {
                    int nWeights = (int)config[i] + 1;
                    result[i][j] = geneExpression.Skip(gene).Take(nWeights).ToArray();
                    gene += nWeights;
                }
            }
            return result;
        }

        #region rand functions
        public static int fastrand(int seed) {
            return ((214013 * seed + 2531011) >> 16) & 0x7FFF;
        }

        public static int hash(int seed) {
            int x = seed;
            x = (x ^ 61) ^ (x >> 16);
            x += x << 3;
            x ^= x >> 4;
            x *= 0x27d4eb2d;
            x ^= x >> 15;
            return x;
        }

        public static Func<int> GenHasher(int seed1, int seed2) {
            int rnd1 = hash(seed1);
            int rnd2 = hash(seed2);
            return () => {
                rnd1 = hash(rnd1);
                rnd2 = hash(rnd2);
                return rnd1 * (rnd2 % 2 == 0 ? 1 : -1);
            };
        }

        public static Func<int, int> GenMutator(int seed) {
            int rnd = hash(seed);
            return x => {
                rnd = hash(rnd);
                return x ^ (1 << (rnd % 32));
            };
        }
        #endregion
    }
}
