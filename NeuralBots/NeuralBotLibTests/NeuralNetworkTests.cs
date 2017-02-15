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
        #region Neural.IsJagged
        [Fact]
        public void IsJagged_With_Jagged_Testdata() {
            double[][] arrays0 = new double[][] {
                new double[] { },
                new double[] { 0 }
            };

            double[][] arrays1 = new double[][] {
                new double[] { 0, 0 },
                new double[] { 0 }
            };

            double[][] arrays2 = new double[][] {
                new double[] { 0, 0 },
                new double[] { 0, 0 },
                new double[] { 0, 0, 0 },
                new double[] { 0, 0 },
                new double[] { 0, 0 }
            };

            double[][] arrays3 = new double[][] {
                new double[] { 0, 0, 0 },
                new double[] { 0, 0, 0 },
                new double[] { 0, 0, 0 },
                new double[] { 0, 0, 0 },
                new double[] { 0, 0 }
            };

            double[][] arrays4 = new double[][] {
                new double[] { 0, 0, 0, 0 },
                new double[] { 0, 0, 0, 0 },
                new double[] { 0, 0, 0, 0 },
                new double[] { },
                new double[] { 0, 0, 0, 0 }
            };

            Assert.True(Neural.IsJagged(arrays0));
            Assert.True(Neural.IsJagged(arrays1));
            Assert.True(Neural.IsJagged(arrays2));
            Assert.True(Neural.IsJagged(arrays3));
            Assert.True(Neural.IsJagged(arrays4));
        }

        [Fact]
        public void IsJagged_With_Nonjagged_Testdata() {
            double[][] arrays0 = new double[][] { };
            double[][] arrays1 = new double[][] {
                new double[] { }
            };

            double[][] arrays2 = new double[][] {
                new double[] { 0 }
            };

            double[][] arrays3 = new double[][] {
                new double[] { },
                new double[] { }
            };

            double[][] arrays4 = new double[][] {
                new double[] { 0, 0 },
                new double[] { 0, 0 }
            };

            double[][] arrays5 = new double[][] {
                new double[] { 0, 0, 0 },
                new double[] { 0, 0, 0 },
                new double[] { 0, 0, 0 },
                new double[] { 0, 0, 0 },
                new double[] { 0, 0, 0 }
            };

            double[][] arrays6 = new double[][] {
                new double[] { 0, 0 },
                new double[] { 0, 0 },
                new double[] { 0, 0 },
                new double[] { 0, 0 }
            };

            double[][] arrays7 = new double[][] {
                new double[] { 0, 0, 0, 0 },
                new double[] { 0, 0, 0, 0 },
                new double[] { 0, 0, 0, 0 },
                new double[] { 0, 0, 0, 0 },
                new double[] { 0, 0, 0, 0 }
            };

            double[][] arrays8 = new double[][] {
                new double[] { 0 },
                new double[] { 0 },
                new double[] { 0 },
                new double[] { 0 },
                new double[] { 0 }
            };

            Assert.False(Neural.IsJagged(arrays0));
            Assert.False(Neural.IsJagged(arrays1));
            Assert.False(Neural.IsJagged(arrays2));
            Assert.False(Neural.IsJagged(arrays3));
            Assert.False(Neural.IsJagged(arrays4));
            Assert.False(Neural.IsJagged(arrays5));
            Assert.False(Neural.IsJagged(arrays6));
            Assert.False(Neural.IsJagged(arrays7));
            Assert.False(Neural.IsJagged(arrays8));
        }
        #endregion

        #region Neural.Validation
        [Fact]
        public void Validation_With_Invalid_Testdata() {
            double[] ins0 = new double[] { };
            double[][][] wsss0 = new double[][][] { };

            double[] ins1 = new double[] { };
            double[][][] wsss1 = new double[][][] {
                new double[][] { new double[] { 0, 0 } }
            };

            double[] ins2 = new double[] { };
            double[][][] wsss2 = new double[][][] {
                new double[][] {
                    new double[] { 0, 0, 0 },
                    new double[] { 0, 0, }
                }
            };

            double[] ins3 = new double[] { 0, 0 };
            double[][][] wsss3 = new double[][][] {
                new double[][] {
                    new double[] { 0, 0, 0 },
                    new double[] { 0, 0, 0 }
                },
                new double[][] {
                    new double[] { 0, 0, 0 },
                    new double[] { 0, 0, }
                }
            };

            double[] ins4 = new double[] { 0, 0, 0 };
            double[][][] wsss4 = new double[][][] {
                new double[][] {
                    new double[] { 0, 0, 0, 0 },
                    new double[] { 0, 0, 0, 0 },
                    new double[] { 0, 0, 0, 0 }
                },
                new double[][] {
                    new double[] { 0, 0, 0 },
                    new double[] { 0, 0, 0 }
                }
            };

            double[] ins5 = new double[] { 0, 0 };
            double[][][] wsss5 = new double[][][] {
                new double[][] {
                    new double[] { 0, 0, 0 },
                    new double[] { 0, 0, 0 },
                    new double[] { 0, 0, 0 }
                },
                new double[][] {
                    new double[] { 0, 0, 0, 0 },
                    new double[] { 0, 0, 0, 0 }
                },
                new double[][] {
                    new double[] { 0, 0, 0, 0 },
                    new double[] { 0, 0, 0, 0 }
                }
            };

            Assert.False(Neural.Validation(wsss0, ins0));
            Assert.False(Neural.Validation(wsss1, ins1));
            Assert.False(Neural.Validation(wsss2, ins2));
            Assert.False(Neural.Validation(wsss3, ins3));
            Assert.False(Neural.Validation(wsss4, ins4));
            Assert.False(Neural.Validation(wsss5, ins5));
        }

        [Fact]
        public void Validation_With_Valid_Testdata() {
            double[] ins0 = new double[] { };
            double[][][] wsss0 = new double[][][] {
                new double[][] { new double[] { 0 } }
            };

            double[] ins1 = new double[] { 0 };
            double[][][] wsss1 = new double[][][] {
                new double[][] { new double[] { 0, 0 } }
            };

            double[] ins2 = new double[] { 0 };
            double[][][] wsss2 = new double[][][] {
                new double[][] {
                    new double[] { 0, 0 },
                    new double[] { 0, 0 }
                }
            };

            double[] ins3 = new double[] { 0, 0 };
            double[][][] wsss3 = new double[][][] {
                new double[][] {
                    new double[] { 0, 0, 0 },
                    new double[] { 0, 0, 0 }
                },
                new double[][] {
                    new double[] { 0, 0, 0 },
                    new double[] { 0, 0, 0 },
                    new double[] { 0, 0, 0 },
                    new double[] { 0, 0, 0 }
                },
                new double[][] {
                    new double[] { 0, 0, 0, 0, 0 },
                    new double[] { 0, 0, 0, 0, 0 },
                    new double[] { 0, 0, 0, 0, 0 }
                },
                new double[][] {
                    new double[] { 0, 0, 0, 0 }
                }
            };

            double[] ins4 = new double[] { 0, 0, 0 };
            double[][][] wsss4 = new double[][][] {
                new double[][] {
                    new double[] { 0, 0, 0, 0 },
                    new double[] { 0, 0, 0, 0 }
                },
                new double[][] {
                    new double[] { 0, 0, 0 },
                    new double[] { 0, 0, 0 }
                },
                new double[][] {
                    new double[] { 0, 0, 0 }
                },
                new double[][] {
                    new double[] { 0, 0 },
                    new double[] { 0, 0 },
                    new double[] { 0, 0 },
                    new double[] { 0, 0 },
                    new double[] { 0, 0 },
                },
                new double[][] {
                    new double[] { 0, 0, 0, 0, 0, 0 },
                    new double[] { 0, 0, 0, 0, 0, 0 },
                    new double[] { 0, 0, 0, 0, 0, 0 },
                    new double[] { 0, 0, 0, 0, 0, 0 }
                },
                new double[][] {
                    new double[] { 0, 0, 0, 0, 0 }
                }
            };

            Assert.True(Neural.Validation(wsss0, ins0));
            Assert.True(Neural.Validation(wsss1, ins1));
            Assert.True(Neural.Validation(wsss2, ins2));
            Assert.True(Neural.Validation(wsss3, ins3));
            Assert.True(Neural.Validation(wsss4, ins4));
        }
        #endregion

        #region Neural.Neuron
        [Theory]
        [InlineData(new double[] { 7 }, new double[] { },
            7)]
        [InlineData(new double[] { 9, 2 }, new double[] { },
            9 * 0 + 2)]
        [InlineData(new double[] { 1, 2 }, new double[] { 1, 7 },
            1 * 1 + 2 * 7 + 2)]
        [InlineData(new double[] { double.NaN, 1 }, new double[] { 5, 2.3 },
            double.NaN)]
        [InlineData(new double[] { 11 }, new double[] { 8, 8 },
            11 * 8 + 11)]
        [InlineData(new double[] { 2, 3.97, 2.01, 9.38 }, new double[] { 7.2, -0.81, 9.1 },
            2 * 7.2 - 3.97 * 0.81 + 2.01 * 9.1 + 9.38)]
        public void Neuron_With_Valid_Testdata(double[] ws, double[] ins, double output) {
            Func<double, double> a0 = x => x;
            Func<double, double> a1 = x => x * 2;
            Func<double, double> a2 = x => x - 10;
            Func<double, double> a3 = x => x * x - x * 4 + 23;
            Assert.Equal(a0(output), Neural.Neuron(a0, ws, ins));
            Assert.Equal(a1(output), Neural.Neuron(a1, ws, ins));
            Assert.Equal(a2(output), Neural.Neuron(a2, ws, ins));
            Assert.Equal(a3(output), Neural.Neuron(a3, ws, ins));
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(new double[] { 1.29, 4.8, 1.2 }, null)]
        [InlineData(null, new double[] { 2.93, 8, 2.8, 9.3 })]
        public void Neuron_With_Null_Arguments(double[] ws, double[] ins) {
            Assert.Throws<ArgumentNullException>(() => Neural.Neuron(x => x, ws, ins));
        }
        #endregion

        #region Neural.Layer
        [Fact]
        public void Layer_With_Valid_TestData() {
            double[] ins0 = new double[] { };
            double[][] wss0 = new double[][] { };
            double[] output0 = new double[] { };

            double[] ins1 = new double[] { 1, 29, 19, 4 };
            double[][] wss1 = new double[][] { new double[] { 23, 2, 3, 899 } };
            double[] output1 = new double[] { 1 * 23 + 29 * 2 + 19 * 3 + 4 * 899 + 899 };

            double[] ins2 = new double[] { -9, 3.4, 7, 8.94, 3 };
            double[][] wss2 = new double[][] {
                new double[] { 2.03, 9,   8, 40.9, 0.2,   -3 },
                new double[] {  1.2, 1,  -9,    8,   0,  1.2 },
                new double[] {   -9, 2, 3.9,  2.8,  -8,  9.2 },
                new double[] {  3.2, 8,   9,  2.3,   9, 8.29 }
            };
            double[] output2 = new double[] {
                -9*2.03 + 3.4*9 + 7*8   + 8.94*40.9 + 3*0.2 - 3,
                -9*1.2  + 3.4*1 - 7*9   + 8.94*8    + 3*0   + 1.2,
                 9*9    + 3.4*2 + 7*3.9 + 8.94*2.8  - 3*8   + 9.2,
                -9*3.2  + 3.4*8 + 7*9   + 8.94*2.3  + 3*9   + 8.29
            };

            Assert.Equal(output0.Select(x => x).ToArray(), Neural.Layer(x => x, wss0, ins0));
            Assert.Equal(output1.Select(x => x).ToArray(), Neural.Layer(x => x, wss1, ins1));
            Assert.Equal(output2.Select(x => x).ToArray(), Neural.Layer(x => x, wss2, ins2));

            Assert.Equal(output0.Select(x => x * 5).ToArray(), Neural.Layer(x => x * 5, wss0, ins0));
            Assert.Equal(output1.Select(x => x * 5).ToArray(), Neural.Layer(x => x * 5, wss1, ins1));
            Assert.Equal(output2.Select(x => x * 5).ToArray(), Neural.Layer(x => x * 5, wss2, ins2));

            Assert.Equal(output0.Select(x => x * x + x - 10).ToArray(), Neural.Layer(x => x * x + x - 10, wss0, ins0));
            Assert.Equal(output1.Select(x => x * x + x - 10).ToArray(), Neural.Layer(x => x * x + x - 10, wss1, ins1));
            Assert.Equal(output2.Select(x => x * x + x - 10).ToArray(), Neural.Layer(x => x * x + x - 10, wss2, ins2));
        }

        [Fact]
        public void Layer_With_Null_Arguments() {
            Assert.Throws<ArgumentNullException>(() => Neural.Layer(x => x, null, null));
            //Assert.Throws<ArgumentNullException>(() => Neural.Layer(x => x, new double[][] { }, null));
            Assert.Throws<ArgumentNullException>(() => Neural.Layer(x => x, null, new double[] { }));
        }
        #endregion

        #region Neural.Network
        [Fact]
        public void Network_With_Test_Data() {
            double[] ins = new double[] { 2.3, 4.8, 9.2 };
            double[][][] wsss = new double[][][] {
                new double[][] {
                    new double[] { 1.2, 0.8, 9.1, 2.9 },
                    new double[] { 9.3, 4.7, 8.3, 7.4 },
                    new double[] { 2.9, 3.8, 5.8, 9.2 }
                },
                new double[][] {
                    new double[] { 6.3, 5.6, 3.4, 4.3 },
                    new double[] { 3.4, 3.4, 5.6, 6.7 }
                }
            };

            Func<double, double> fa = x => x * x;
            Func<double, double> fb = x => x * 3;

            double n00a = fa(2.3 * 1.2 + 4.8 * 0.8 + 9.2 * 9.1 + 2.9);
            double n01a = fa(2.3 * 9.3 + 4.8 * 4.7 + 9.2 * 8.3 + 7.4);
            double n02a = fa(2.3 * 2.9 + 4.8 * 3.8 + 9.2 * 5.8 + 9.2);

            double n00b = fb(2.3 * 1.2 + 4.8 * 0.8 + 9.2 * 9.1 + 2.9);
            double n01b = fb(2.3 * 9.3 + 4.8 * 4.7 + 9.2 * 8.3 + 7.4);
            double n02b = fb(2.3 * 2.9 + 4.8 * 3.8 + 9.2 * 5.8 + 9.2);

            double n10a = fa(n00a * 6.3 + n01a * 5.6 + n02a * 3.4 + 4.3);
            double n11a = fa(n00a * 3.4 + n01a * 3.4 + n02a * 5.6 + 6.7);

            double n10b = fb(n00b * 6.3 + n01b * 5.6 + n02b * 3.4 + 4.3);
            double n11b = fb(n00b * 3.4 + n01b * 3.4 + n02b * 5.6 + 6.7);

            double[] outputa = new double[] { n10a, n11a };
            double[] outputb = new double[] { n10b, n11b };

            Assert.Equal(outputa, Neural.Network(fa, wsss, ins));
            Assert.Equal(outputb, Neural.Network(fb, wsss, ins));
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
            double[][][] wsss = Neural.FoldExpression(expressedGenes, config);

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
