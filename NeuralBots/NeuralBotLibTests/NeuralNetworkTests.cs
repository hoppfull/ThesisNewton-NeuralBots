using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;
using FsCheck.Xunit;

using NeuralBotLib;

namespace NeuralBotLibTests {
    public class NeuralNetworkTests {
        [Property]
        public bool fOfXIsAlwaysGreaterThanX(int x) {
            return NeuralNetwork.f(x) > x;
        }

        [Fact]
        public void fOf4Equals8() {
            Assert.Equal(8, NeuralNetwork.f(4));
        }
    }
}
