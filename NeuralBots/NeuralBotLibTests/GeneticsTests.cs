using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Xunit;
using FsCheck;
using FsCheck.Xunit;

using NeuralBotLib;

namespace NeuralBotLibTests {
    public class GeneticsTests {
        #region Genetics.Genesis
        [Theory]
        [InlineData(new uint[] { }, 0)]
        [InlineData(new uint[] { 74 }, 0)]
        [InlineData(new uint[] { 3, 4 }, (3 + 1) * 4)]
        [InlineData(new uint[] { 2, 3, 8, 7, 4, 6 },
            (2 + 1) * 3 + (3 + 1) * 8 + (8 + 1) * 7 + (7 + 1) * 4 + (4 + 1) * 6)]
        public void Genesis_With_Testdata(uint[] config, int length) {
            Func<int> fa = () => 0;
            Func<int> fb = () => 1;
            Func<int> fc = Genetics.GenHasher(234232, 123121);
            Func<int> fd = Genetics.GenHasher(234232, 123121);

            int[] chromosomeA = Genetics.Genesis(fa, config);
            int[] chromosomeB = Genetics.Genesis(fb, config);
            int[] chromosomeC = Genetics.Genesis(fc, config);

            Assert.Equal(length, chromosomeA.Length);
            Assert.Equal(length, chromosomeB.Length);
            Assert.Equal(length, chromosomeC.Length);

            Assert.True(chromosomeA.All(gene => gene == 0), "All zero");
            Assert.True(chromosomeB.All(gene => gene == 1), "All one");
            Assert.True(chromosomeC.All(gene => gene == fd()), "All random");
        }
        #endregion

        #region Genetics.FoldExpression
        [Theory]
        [InlineData(new double[] { }, new uint[] { }, 0)]
        [InlineData(new double[] { }, new uint[] { 3 }, 0)]
        [InlineData(new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 },
            new uint[] { 2, 3 }, 1)]
        [InlineData(new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 },
            new uint[] { 3, 3, 1, 2, 4 }, 4)]
        public void FoldExpression_With_Testdata(double[] expressedGenes, uint[] config, int wsssLength) {
            double[][][] wsss = Genetics.FoldExpression(expressedGenes, config);

            Assert.Equal(wsssLength, wsss.Length);
            for (int i = 0; i < wsssLength; i++) {
                Assert.Equal((int)config[i + 1], wsss[i].Length);
                for (int j = 0; j < wsss[i].Length; j++)
                    Assert.Equal((int)config[i] + 1, wsss[i][j].Length);
            }
        }
        #endregion
    }
}
