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
        
        [Fact]
        public void NeuronLayerEval_Null_Handling() {
            Assert.Throws<ArgumentNullException>(() => NeuralNetwork.NeuronLayerEval(null, null));
            Assert.Throws<ArgumentNullException>(() => NeuralNetwork.NeuronLayerEval(null, new double[] { }));
            Assert.Throws<ArgumentNullException>(() => NeuralNetwork.NeuronLayerEval(new double[,] { }, null));
        }

        [Property]
        public Property NeuronLayerEval_Error_Handling() {
            return Prop.ForAll<double[,], double[]>((wss, ins) => {
                Action lazy = () => NeuralNetwork.NeuronLayerEval(wss, ins);
                if (wss.GetLength(0) == 0) Assert.Throws<NeuralNetwork.EmptyWeightsException>(lazy);
                else if (wss.GetLength(0) > ins.Length + 1) Assert.Throws<NeuralNetwork.ExcessiveWeightsException>(lazy);
                else if (ins.Length + 1 > wss.GetLength(0)) Assert.Throws<NeuralNetwork.ExcessiveInputsException>(lazy);
            });
        }

        #region NeuronTests
        [Theory]
        [InlineData(null, null)]
        [InlineData(new double[] { }, null)]
        [InlineData(null, new double[] { })]
        public void NeuronEval_Throws_ArgumentNullException_On_Null_Arguments(double[] weights, double[] inputs) {
            Assert.Throws<ArgumentNullException>(() => NeuralNetwork.NeuronEval(weights, inputs));
        }

        [Property]
        public Property NeuronEval_Error_Handling() {
            return Prop.ForAll<double[], double[]>((weights, inputs) => {
                Action lazy = () => NeuralNetwork.NeuronEval(weights, inputs);
                if (weights.Length == 0) Assert.Throws<NeuralNetwork.EmptyWeightsException>(lazy);
                else if (weights.Length > inputs.Length + 1) Assert.Throws<NeuralNetwork.ExcessiveWeightsException>(lazy);
                else if (inputs.Length + 1 > weights.Length) Assert.Throws<NeuralNetwork.ExcessiveInputsException>(lazy);
            });
        }

        [Theory]
        [InlineData(new double[] { 7 }, new double[] { }, 7)]
        [InlineData(new double[] { -8 }, new double[] { }, -8)]
        [InlineData(new double[] { 4.01, 2.005 }, new double[] { 9.5 },
            4.01 * 9.5 + 2.005)]
        [InlineData(new double[] { 2, 2, 3 }, new double[] { 5, 2 },
            2 * 5 + 2 * 2 + 3)]
        [InlineData(new double[] { 83, -2, 74 }, new double[] { 19, 21 },
            83 * 19 - 2 * 21 + 74)]
        [InlineData(new double[] { 0.12, 0.32, 4, -3.2 }, new double[] { 19, 21, -0.8 },
            0.12 * 19 + 0.32 * 21 - 4 * 0.8 - 3.2)]
        [InlineData(new double[] { 0.989, 0.343, 9, -2 }, new double[] { 12, -9, -0.81 },
            0.989 * 12 - 0.343 * 9 - 9 * 0.81 - 2)]
        public void NeuronEval_Test(double[] weights, double[] inputs, double inputsdotweights) {
            double output = NeuralNetwork.NeuronEval(weights, inputs);
            Assert.Equal(NeuralNetwork.Sigmoid(inputsdotweights), output);
        }
        #endregion
        #region SigmoidTests
        [Property]
        public Property Sigmoid_PropTest() {
            return Prop.ForAll<double>(x => {
                if (x.Equals(double.NaN))
                    Assert.Throws<ArgumentOutOfRangeException>(() => NeuralNetwork.Sigmoid(double.NaN));
                else {
                    double h = 0.001;
                    double y0 = NeuralNetwork.Sigmoid(x);
                    double y1 = NeuralNetwork.Sigmoid(x >= 0 ? x + h : x - h);
                    double y2 = NeuralNetwork.Sigmoid(x >= 0 ? x + 2 * h : x - 2 * h);
                    double d1 = y1 - y0;
                    double d2 = y2 - y1;
                    if (x >= 0 && x <= 10)
                        Assert.True(y0 >= 0.5 && y0 <= 1 && d1 > d2 && d1 > 0,
                            $"if(x >= 0 && x <= 10): y0={y0}, d1={d1}, d2={d2}");
                    else if (x < 0 && x >= -10)
                        Assert.True(y0 < 0.5 && y0 >= 0 && d1 < d2 && d1 < 0,
                            $"if(x < 0 && x >= -10): y0={y0}, d1={d1}, d2={d2}");
                    else if (x > 10)
                        Assert.True(y0 == 1 && d1 == 0 && d2 == 0,
                            $"if(x > 10): y0={y0}, d1={d1}, d2={d2}");
                    else if (x < -10)
                        Assert.True(y0 == 0 && d1 == 0 && d2 == 0,
                            $"if(x < -10): y0={y0}, d1={d1}, d2={d2}");
                    else Assert.True(false);
                }
            });
        }
        #endregion
    }
}
