using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// http://derekwill.com/2015/03/05/bit-processing-in-c/
namespace NeuralBotLib {
    public static class Genetics {
        private static uint LengthOfGenes(uint[] config) {
            uint result = 0;
            for (int i = 1; i < config.Length; i++) result += (config[i - 1] + 1) * config[i];
            return result;
        }

        public static int[] Genesis(Func<int> gene, uint[] config) {
            return new int[LengthOfGenes(config)].Select(_ => gene()).ToArray();
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

        public static double[] ExpressGenes(Func<int, int, double> expressGene, int[] chrA, int[] chrB) {
            return chrA.Zip(chrB, expressGene).ToArray();
        }

        public static int[] Replicate(int[] chromosome, Func<int, int> mutation) {
            return chromosome.Select(mutation).ToArray();
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
