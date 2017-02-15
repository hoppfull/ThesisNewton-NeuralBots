using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Xunit;
using FsCheck;
using FsCheck.Xunit;

using static NeuralBotLib.Learning;

namespace NeuralBotLibTests {
    public class LearningTests {
        #region Cost
        [Fact]
        public void Cost_With_Testdata() {
            TrainingExample[] ex0 = new TrainingExample[] {
                new TrainingExample(new double[] { }, new double[] { })
            };
            double expected0 = 0;

            TrainingExample[] ex1 = new TrainingExample[] {
                new TrainingExample(new double[] { 4, 3, 5 }, new double[] { 1, 2, 3 })
            };
            double expected1 = Math.Pow(1 - 4, 2) + Math.Pow(2 - 3, 2) + Math.Pow(3 - 5, 2);

            TrainingExample[] ex2 = new TrainingExample[] {
                new TrainingExample(new double[] { 4,3,5,3 }, new double[] { 3,4,4,5 }),
                new TrainingExample(new double[] { 2,3,4,2 }, new double[] { 8,4,6,4 }),
                new TrainingExample(new double[] { 1,5,2,3 }, new double[] { 5,4,7,4 })
            };
            double expected2 =
                (Math.Pow(3 - 4, 2) + Math.Pow(4 - 3, 2) + Math.Pow(4 - 5, 2) + Math.Pow(5 - 3, 2)) +
                (Math.Pow(8 - 2, 2) + Math.Pow(4 - 3, 2) + Math.Pow(6 - 4, 2) + Math.Pow(4 - 2, 2)) +
                (Math.Pow(5 - 1, 2) + Math.Pow(4 - 5, 2) + Math.Pow(7 - 2, 2) + Math.Pow(4 - 3, 2));

            TrainingExample[] ex3 = new TrainingExample[] {
                new TrainingExample(new double[] { 9.234 }, new double[] { 4.5443 })
            };
            double expected3 = Math.Pow(4.5443 - 9.234, 2);

            Assert.Equal(expected0, Cost(ex0, xs => xs));
            Assert.Equal(expected1, Cost(ex1, xs => xs));
            Assert.Equal(expected2, Cost(ex2, xs => xs));
            Assert.Equal(expected3, Cost(ex3, xs => xs));
        }
        #endregion

        #region Roulette

        [Fact]
        public void Roulette_With_Testdata() {
            Tuple<string, double>[] candidates0 = new Tuple<string, double>[] {
                Tuple.Create("a", 25d),
                Tuple.Create("b", 25d),
                Tuple.Create("c", 25d),
                Tuple.Create("d", 25d)
            };

            string id1 = Roulette(candidates0, () => 0);
            string id2 = Roulette(candidates0, () => int.MaxValue);
            string id3 = Roulette(candidates0, () => int.MinValue);
            string id4 = Roulette(candidates0, () => int.MaxValue / 2);

            Assert.Equal("b", id1);
            Assert.Equal("d", id2);
            Assert.Equal("a", id3);
            Assert.Equal("c", id4);
        }

        [Fact]
        public void Roulette_With_Single_Testdata() {
            Tuple<string, double>[] candidates = new Tuple<string, double>[] {
                Tuple.Create("a", 6544d),
            };

            Assert.Equal("a", Roulette(candidates, () => 0));
        }

        [Fact]
        public void Roulette_With_Empty_Testdata() {
            Assert.Throws<InvalidOperationException>(() => Roulette(new Tuple<string, double>[] { }, () => 0));
        }

        #endregion
    }
}
