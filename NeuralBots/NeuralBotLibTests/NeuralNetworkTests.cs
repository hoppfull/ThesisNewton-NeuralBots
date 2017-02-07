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
            Assert.Throws<ArgumentNullException>(() => NeuralNetwork.NeuronLayerEval(x => x, null, null));
            Assert.Throws<ArgumentNullException>(() => NeuralNetwork.NeuronLayerEval(x => x, null, new double[] { }));
            Assert.Throws<ArgumentNullException>(() => NeuralNetwork.NeuronLayerEval(x => x, new double[][] { }, null));
            Assert.Throws<ArgumentNullException>(() => NeuralNetwork.NeuronLayerEval(null, new double[][] { }, new double[] { }));
        }

        //[Property]
        //public Property NeuronLayerEval_Error_Handling() {
        //    return Prop.ForAll<double[][], double[]>((wss, ins) => {
        //        Action lazy = () => NeuralNetwork.NeuronLayerEval(x => x, wss, ins);
        //        if (wss[0].Length == 0) Assert.Throws<NeuralNetwork.EmptyWeightsException>(lazy);
        //        else if (wss[0].Length > ins.Length + 1) Assert.Throws<NeuralNetwork.ExcessiveWeightsException>(lazy);
        //        else if (ins.Length + 1 > wss.GetLength(1)) Assert.Throws<NeuralNetwork.ExcessiveInputsException>(lazy);
        //        else {
        //            double[] output = NeuralNetwork.NeuronLayerEval(x => x, wss, ins);
        //            Assert.True(output.Length == wss.GetLength(0),
        //                $"output.Length={output.Length}, wss.GetLength(0)={wss.GetLength(0)}");
        //        }
        //    });
        //}

        [Fact]
        public void NeuronLayerEval_With_TestData() {
            double[][] wss0 = new double[][] { new double[] { 1 } };
            double[] ins0 = new double[] { };
            List<double> out0 = new List<double> { 1 };

            double[][] wss1 = new double[][] { new double[] { -10, 2.01 } };
            double[] ins1 = new double[] { 0.57 };
            List<double> out1 = new List<double> { -10 * 0.57 + 2.01 };

            double[][] wss2 = new double[][] {
                new double[] { 17, 0.112, -81 },
                new double[] { 10, -2.9, 3.1 }
            };
            double[] ins2 = new double[] { 0.22, 5.01 };
            List<double> out2 = new List<double> {
                17 * 0.22 + 0.112 * 5.01 - 81,
                10 * 0.22 - 2.9 * 5.01 + 3.1
            };

            double[][] wss3 = new double[][] {
                new double[] { 8, 2.3, 4,   9,   9,   3 },
                new double[] { 1, 2,   9.8, 1,   2,   1.1 },
                new double[] { 4, 3,   8,   3,   4,   9 },
                new double[] { 3, 2,   3,   5,   2,   0 },
                new double[] { 8, 2,   7.8, 4.2, 3.8, 9 }
            };
            double[] ins3 = new double[] { 7, 28, 2, 8, 2 };
            List<double> out3 = new List<double> {
                8 * 7 + 2.3 * 28 + 4   * 2 + 9   * 8 + 9 *   2 + 3,
                1 * 7 + 2   * 28 + 9.8 * 2 + 1   * 8 + 2 *   2 + 1.1,
                4 * 7 + 3   * 28 + 8   * 2 + 3   * 8 + 4 *   2 + 9,
                3 * 7 + 2   * 28 + 3   * 2 + 5   * 8 + 2 *   2 + 0,
                8 * 7 + 2   * 28 + 7.8 * 2 + 4.2 * 8 + 3.8 * 2 + 9
            };

            Func<double, double> fa = x => x;
            Func<double, double> fb = x => x * 3;
            Func<double, double> fc = x => x - 10;

            Assert.Equal(out0.Select(fa).ToArray(), NeuralNetwork.NeuronLayerEval(fa, wss0, ins0));
            Assert.Equal(out0.Select(fb).ToArray(), NeuralNetwork.NeuronLayerEval(fb, wss0, ins0));
            Assert.Equal(out0.Select(fc).ToArray(), NeuralNetwork.NeuronLayerEval(fc, wss0, ins0));

            Assert.Equal(out1.Select(fa).ToArray(), NeuralNetwork.NeuronLayerEval(fa, wss1, ins1));
            Assert.Equal(out1.Select(fb).ToArray(), NeuralNetwork.NeuronLayerEval(fb, wss1, ins1));
            Assert.Equal(out1.Select(fc).ToArray(), NeuralNetwork.NeuronLayerEval(fc, wss1, ins1));

            Assert.Equal(out2.Select(fa).ToArray(), NeuralNetwork.NeuronLayerEval(fa, wss2, ins2));
            Assert.Equal(out2.Select(fb).ToArray(), NeuralNetwork.NeuronLayerEval(fb, wss2, ins2));
            Assert.Equal(out2.Select(fc).ToArray(), NeuralNetwork.NeuronLayerEval(fc, wss2, ins2));

            Assert.Equal(out3.Select(fa).ToArray(), NeuralNetwork.NeuronLayerEval(fa, wss3, ins3));
            Assert.Equal(out3.Select(fb).ToArray(), NeuralNetwork.NeuronLayerEval(fb, wss3, ins3));
            Assert.Equal(out3.Select(fc).ToArray(), NeuralNetwork.NeuronLayerEval(fc, wss3, ins3));
        }

        #region NeuronTests
        [Theory]
        [InlineData(null, null)]
        [InlineData(new double[] { }, null)]
        [InlineData(null, new double[] { })]
        public void NeuronEval_Throws_ArgumentNullException_On_Null_Arguments(double[] weights, double[] inputs) {
            Assert.Throws<ArgumentNullException>(() => NeuralNetwork.NeuronEval(x => x, weights, inputs));
        }

        [Fact]
        public void NeuronEval_Throws_ArgumentNullException_On_Null_Activation() {
            Assert.Throws<ArgumentNullException>(() => NeuralNetwork.NeuronEval(null, new double[] { }, new double[] { }));
        }

        [Property]
        public Property NeuronEval_Error_Handling() {
            return Prop.ForAll<double[], double[]>((weights, inputs) => {
                Action lazy = () => NeuralNetwork.NeuronEval(NeuralNetwork.Sigmoid, weights, inputs);
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
        public void NeuronEval__With_TestData(double[] weights, double[] inputs, double inputsdotweights) {
            Func<double, double> a0 = x => x;
            Func<double, double> a1 = x => x * 2;
            Func<double, double> a2 = x => x * x;
            Func<double, double> a3 = x => x * x + 3 * x - 7;
            double output0 = NeuralNetwork.NeuronEval(a0, weights, inputs);
            double output1 = NeuralNetwork.NeuronEval(a1, weights, inputs);
            double output2 = NeuralNetwork.NeuronEval(a2, weights, inputs);
            double output3 = NeuralNetwork.NeuronEval(a3, weights, inputs);
            Assert.Equal(a0(inputsdotweights), output0);
            Assert.Equal(a1(inputsdotweights), output1);
            Assert.Equal(a2(inputsdotweights), output2);
            Assert.Equal(a3(inputsdotweights), output3);
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
