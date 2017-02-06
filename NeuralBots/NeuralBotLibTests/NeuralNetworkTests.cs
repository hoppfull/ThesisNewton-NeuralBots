using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using FsCheck;
using FsCheck.Xunit;

using NeuralBotLib;

namespace NeuralBotLibTests {
    public class NeuralNetworkTests {
        [Fact]
        public void NNEvalOnEmptyWeightArray() {
            double[][][] ws = new double[][][] {

            };
            double[] inputs = new double[] { 1, 2, 3 };
            double[] output = NeuralNetwork.NNEval(ws, inputs);

            Assert.Equal(0, output.Length);
        }

        [Theory]
        [InlineData(new double[] { }, new double[] { 1, 2, 3 })]
        [InlineData(new double[] { 1 }, new double[] { 1, 2, 3, 8 })]
        [InlineData(new double[] { 48, 1, 2 }, new double[] { 8, 12, 0 })]
        public void NEval_Throws_Exception_On_ExcessiveInputs(double[] ws, double[] inputs) {

            NeuralNetwork.IncorrectWeightInputException ex =
                Assert.Throws<NeuralNetwork.IncorrectWeightInputException>(() => NeuralNetwork.NEval(ws, inputs));

            Assert.True(ws.Length == ex.WeightsLength, $"ws.Length: {ws.Length} != ex.WeightsLength: {ex.WeightsLength}");
            Assert.True(inputs.Length == ex.InputsLength, $"inputs.Length: {inputs.Length} != ex.InputsLength: {ex.InputsLength}");
            Assert.Equal(NeuralNetwork.IncorrectWeightInputException.ErrorTypes.ExcessiveInputs, ex.ErrorType);
        }

        [Theory]
        [InlineData(new double[] { 8, 3, 10 }, new double[] { 19 })]
        public void NEval_Throws_Exception_On_ExcessiveWeights(double[] ws, double[] inputs) {

            NeuralNetwork.IncorrectWeightInputException ex =
                Assert.Throws<NeuralNetwork.IncorrectWeightInputException>(() => NeuralNetwork.NEval(ws, inputs));

            Assert.True(ws.Length == ex.WeightsLength, $"ws.Length: {ws.Length} != ex.WeightsLength: {ex.WeightsLength}");
            Assert.True(inputs.Length == ex.InputsLength, $"inputs.Length: {inputs.Length} != ex.InputsLength: {ex.InputsLength}");
            Assert.Equal(NeuralNetwork.IncorrectWeightInputException.ErrorTypes.ExcessiveWeights, ex.ErrorType);
        }

        [Theory]
        [InlineData(new double[] { 7 }, new double[] { })]
        public void NEval_With_No_Inputs(double[] weights, double[] inputs) {
            double output = NeuralNetwork.NEval(weights, inputs);
            Assert.Equal(NeuralNetwork.Sigmoid(7), output);
        }


        [Fact]
        public void Sigmoid_With_NaN() {
            Assert.Throws<ArgumentOutOfRangeException>(() => NeuralNetwork.Sigmoid(double.NaN));
        }

        [Property]
        public Property Sigmoid_With_RealNumbers() {
            Gen<double> allDoublesExceptNaN = from x in Arb.Generate<double>()
                                              where !x.Equals(double.NaN)
                                              select x;
            return Prop.ForAll(allDoublesExceptNaN.ToArbitrary(), x => {
                double y = NeuralNetwork.Sigmoid(x);
                if (x >= 0) Assert.True(y >= 0.5 && y <= 1);
                else if (x < 0) Assert.True(y < 0.5 && y >= 0);
                else Assert.True(false);
            });
        }

        [Property]
        public Property Sigmoid_With_Positive_And_Not_Zero_Numbers() {
            Gen<double> allGreaterThanZero = from x in Arb.Generate<double>()
                                             where x > 0 && !x.Equals(double.NaN)
                                             select x;
            return Prop.ForAll(allGreaterThanZero.ToArbitrary(), x => {
                double h = 0.001;
                double y0 = NeuralNetwork.Sigmoid(x);
                double y1 = NeuralNetwork.Sigmoid(x + h);
                double y2 = NeuralNetwork.Sigmoid(x + 2 * h);
                double derivative1 = y1 - y0;
                double derivative2 = y2 - y1;
                if (x > 10) {
                    Assert.Equal(0, derivative1);
                    Assert.Equal(0, derivative2);
                } else {
                    Assert.True(derivative1 > 0, $"derivative1 = {derivative1}");
                    Assert.True(derivative2 < derivative1);
                }
            });
        }

        [Property]
        public Property Sigmoid_With_Negative_And_Not_Zero_Numbers() {
            Gen<double> allGreaterThanZero = from x in Arb.Generate<double>()
                                             where x < 0 && !x.Equals(double.NaN)
                                             select x;
            return Prop.ForAll(allGreaterThanZero.ToArbitrary(), x => {
                double h = 0.001;
                double y0 = NeuralNetwork.Sigmoid(x);
                double y1 = NeuralNetwork.Sigmoid(x - h);
                double y2 = NeuralNetwork.Sigmoid(x - 2 * h);
                double derivative1 = y1 - y0;
                double derivative2 = y2 - y1;
                if (x < -10) {
                    Assert.Equal(0, derivative1);
                    Assert.Equal(0, derivative2);
                } else {
                    Assert.True(derivative1 < 0, $"derivative1 = {derivative1}");
                    Assert.True(derivative2 > derivative1);
                }
            });
        }
    }
}
