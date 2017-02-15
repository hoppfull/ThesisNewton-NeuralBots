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
            public IEnumerable<Gene> Genes { get; }
            public Chromosome(IEnumerable<Gene> Genes) {
                this.Genes = Genes;
            }

            public Chromosome Replicate(Func<Gene, Gene> mutate) {
                return new Chromosome(Genes.Select(mutate));
            }

            public double[] ExpressWith(Chromosome chromosome, Func<Gene, Gene, double> expressGene) {
                return Genes.Zip(chromosome.Genes, expressGene).ToArray();
            }
        }
        
        #region rand functions
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
        
        public static IEnumerable<T> NumberGenerator<T>(Func<T, T> rng, T seed) {
            T rn = rng(seed);
            while (true) {
                yield return rn;
                rn = rng(rn);
            }
        }
        #endregion
    }
}
