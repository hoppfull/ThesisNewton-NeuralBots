using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Xunit;
using FsCheck;
using FsCheck.Xunit;

using NeuralBotLib;

namespace NeuralBotLibTests {
    public class LearningTests {
        #region Learning.Cost
        [Fact]
        public void Cost_With_Testdata() {
            Learning.TrainingExample[] ex0 = new Learning.TrainingExample[] {
                new Learning.TrainingExample(new double[] { }, new double[] { })
            };
            double expected0 = 0;

            Learning.TrainingExample[] ex1 = new Learning.TrainingExample[] {
                new Learning.TrainingExample(new double[] { 4, 3, 5 }, new double[] { 1, 2, 3 })
            };
            double expected1 = Math.Pow(1 - 4, 2) + Math.Pow(2 - 3, 2) + Math.Pow(3 - 5, 2);

            Learning.TrainingExample[] ex2 = new Learning.TrainingExample[] {
                new Learning.TrainingExample(new double[] { 4,3,5,3 }, new double[] { 3,4,4,5 }),
                new Learning.TrainingExample(new double[] { 2,3,4,2 }, new double[] { 8,4,6,4 }),
                new Learning.TrainingExample(new double[] { 1,5,2,3 }, new double[] { 5,4,7,4 })
            };
            double expected2 =
                (Math.Pow(3 - 4, 2) + Math.Pow(4 - 3, 2) + Math.Pow(4 - 5, 2) + Math.Pow(5 - 3, 2)) +
                (Math.Pow(8 - 2, 2) + Math.Pow(4 - 3, 2) + Math.Pow(6 - 4, 2) + Math.Pow(4 - 2, 2)) +
                (Math.Pow(5 - 1, 2) + Math.Pow(4 - 5, 2) + Math.Pow(7 - 2, 2) + Math.Pow(4 - 3, 2));

            Learning.TrainingExample[] ex3 = new Learning.TrainingExample[] {
                new Learning.TrainingExample(new double[] { 9.234 }, new double[] { 4.5443 })
            };
            double expected3 = Math.Pow(4.5443 - 9.234, 2);

            Assert.Equal(expected0, Learning.Cost(ex0, xs => xs));
            Assert.Equal(expected1, Learning.Cost(ex1, xs => xs));
            Assert.Equal(expected2, Learning.Cost(ex2, xs => xs));
            Assert.Equal(expected3, Learning.Cost(ex3, xs => xs));
        }
        #endregion
    }
}
