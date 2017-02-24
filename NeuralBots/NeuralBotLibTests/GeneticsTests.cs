using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Xunit;
using FsCheck;
using FsCheck.Xunit;

using NeuralBotLib;
using static NeuralBotLib.Genetics;

namespace NeuralBotLibTests {
    public class GeneticsTests {
        #region Individual
        [Fact]
        public void IndividualMate_With_Testdata() {
            Func<int> fa = () => 0;
            Individual ind1a = new Individual(
                new Chromosome(Generate(gene => gene, new Gene(1)).Take(3).ToArray()),
                new Chromosome(Generate(gene => gene, new Gene(2)).Take(3).ToArray()));

            Individual ind1b = new Individual(
                new Chromosome(Generate(gene => gene, new Gene(3)).Take(3).ToArray()),
                new Chromosome(Generate(gene => gene, new Gene(4)).Take(3).ToArray()));

            Func<int> fb = () => 1;
            Individual ind2a = new Individual(
                new Chromosome(Generate(gene => gene, new Gene(1)).Take(3).ToArray()),
                new Chromosome(Generate(gene => gene, new Gene(2)).Take(3).ToArray()));

            Individual ind2b = new Individual(
                new Chromosome(Generate(gene => gene, new Gene(3)).Take(3).ToArray()),
                new Chromosome(Generate(gene => gene, new Gene(4)).Take(3).ToArray()));

            Individual ind1ab = ind1a.Mate(fa, ind1b, gene => new Gene((short)(gene.Data * 10)));
            Individual ind2ab = ind2a.Mate(fb, ind2b, gene => new Gene((short)(gene.Data * 10)));

            Assert.True(ind1ab.cA.Genes.All(gene => gene.Data == 10), "gene.Data = 10");
            Assert.True(ind1ab.cB.Genes.All(gene => gene.Data == 30), "gene.Data = 30");

            Assert.True(ind2ab.cA.Genes.All(gene => gene.Data == 20), "gene.Data = 20");
            Assert.True(ind2ab.cB.Genes.All(gene => gene.Data == 40), "gene.Data = 40");
        }

        [Fact]
        public void IndividualExpress_With_Testdata() {
            Individual ind1a = new Individual(
                new Chromosome(Generate(gene => gene, new Gene(6)).Take(3).ToArray()),
                new Chromosome(Generate(gene => gene, new Gene(4)).Take(3).ToArray()));

            Func<Gene, Gene, double> fa = (gA, gB) => gA.Data + gB.Data;
            Func<Gene, Gene, double> fb = (gA, gB) => gA.Data * gB.Data;
            Func<Gene, Gene, double> fc = (gA, gB) => gA.Data / gB.Data;

            Assert.True(ind1a.Express(fa).All(d => d == 6 + 4), "d = 6 + 4");
            Assert.True(ind1a.Express(fb).All(d => d == 6 * 4), "d = 6 * 4");
            Assert.True(ind1a.Express(fc).All(d => d == 6 / 4), "d = 6 / 4");
        }

        #endregion
        #region Chromosome
        [Fact]
        public void ChromosomeReplicate_Replicates_As_Intended() {
            Chromosome c1 = new Chromosome(Generate(gene => gene, new Gene(0)).Take(5).ToArray());
            Chromosome c2 = c1.Replicate(gene => new Gene((short)(gene.Data + 1)));
            Gene[] g1 = c1.Genes.ToArray();
            Gene[] g2 = c2.Genes.ToArray();
            Assert.Equal(g1.Length, g2.Length);
            for (int i = 0; i < g1.Length; i++) {
                Assert.Equal(g1[i].Data + 1, g2[i].Data);
            }
        }

        [Fact]
        public void ChromosomeExpressWith_Testdata() {
            Gene[] gs1 = new Gene[] {
                new Gene(1),
                new Gene(5),
                new Gene(2),
                new Gene(4),
                new Gene(3)
            };
            Gene[] gs2 = new Gene[] {
                new Gene(8),
                new Gene(7),
                new Gene(9),
                new Gene(6),
                new Gene(0)
            };
            Chromosome c1 = new Chromosome(gs1);
            Chromosome c2 = new Chromosome(gs2);

            Func<Gene, Gene, double> f1 = (g1, g2) => g1.Data + g2.Data;
            Func<Gene, Gene, double> f2 = (g1, g2) => g1.Data / 2d + g2.Data;
            Func<Gene, Gene, double> f3 = (g1, g2) => g1.Data * g2.Data;

            double[] result1 = c1.ExpressWith(c2, f1);
            double[] result2 = c1.ExpressWith(c2, f2);
            double[] result3 = c1.ExpressWith(c2, f3);

            Assert.Equal(gs1.Length, result1.Length);
            Assert.Equal(gs1.Length, result2.Length);
            Assert.Equal(gs1.Length, result3.Length);

            Assert.Equal(gs2.Length, result1.Length);
            Assert.Equal(gs2.Length, result2.Length);
            Assert.Equal(gs2.Length, result3.Length);

            Assert.Equal(new double[] { 9, 12, 11, 10, 3 }, result1);
            Assert.Equal(new double[] { 8.5, 9.5, 10, 8, 1.5 }, result2);
            Assert.Equal(new double[] { 8, 35, 18, 24, 0 }, result3);
        }
        #endregion

        #region RandomFunctions

        #region Hash
        [Property]
        public bool Hash_Is_Determenistic(int seed) {
            return Hash(seed) == Hash(seed);
        }

        [Property]
        public bool Hash_Has_No_Immediate_Repitition(int seed) {
            int rn1 = Hash(seed);
            int rn2 = Hash(rn1);
            return rn1 != rn2;
        }
        #endregion

        #region FastRand
        [Property]
        public bool FastRand_Is_Determenistic(int seed) {
            return Fastrand(seed) == Fastrand(seed);
        }

        [Property]
        public bool FastRand_Has_No_Immediate_Repitition(int seed) {
            int rn1 = Fastrand(seed);
            int rn2 = Fastrand(rn1);
            return rn1 != rn2;
        }
        #endregion

        #endregion

        #region NumberGenerator
        [Property]
        public Property NumberGenerator_Is_Deterministic() {
            return Prop.ForAll<int, int>((length, seed) => {
                Assert.Equal(
                    Generate(x => x, seed).Take(length),
                    Generate(x => x, seed).Take(length));
            });
        }

        [Property]
        public Property NumberGenerator_Is_Pure() {
            return Prop.ForAll<int, int>((length, seed) => {
                IEnumerable<int> list = Generate(Fastrand, seed).Take(length);
                Assert.Equal(list.ToArray(), list.ToArray());
            });
        }

        [Fact]
        public void NumberGenerator_With_Testdata() {
            Func<int, int> f1 = x => x + 1;
            Func<bool, bool> f2 = x => !x;
            Func<double, double> f3 = x => 2 * x;
            Func<Tuple<int, bool>, Tuple<int, bool>> f4 = x => Tuple.Create(x.Item1 - 1, !x.Item2);

            int[] result1 = Generate(f1, 0).Take(5).ToArray();
            bool[] result2 = Generate(f2, true).Take(5).ToArray();
            double[] result3 = Generate(f3, 0.1).Take(5).ToArray();
            Tuple<int, bool>[] result4 = Generate(f4, Tuple.Create(10, false)).Take(5).ToArray();

            Assert.Equal(new int[] { 1, 2, 3, 4, 5 }, result1);
            Assert.Equal(new bool[] { false, true, false, true, false }, result2);
            Assert.Equal(new double[] { 0.2, 0.4, 0.8, 1.6, 3.2 }, result3);
            Assert.Equal(new Tuple<int, bool>[] {
                Tuple.Create(9,true),
                Tuple.Create(8,false),
                Tuple.Create(7,true),
                Tuple.Create(6,false),
                Tuple.Create(5,true)
            }, result4);
        }
        #endregion
    }
}
