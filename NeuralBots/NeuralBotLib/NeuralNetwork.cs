using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralBotLib {
    public static class NeuralNetwork {
        public static double[] NNEval(double[][][] weights, double[] inputs) {
            return new double[0];
        }

        public static double NeuronEval(double[] weights, double[] inputs) {
            if (weights == null || inputs == null) throw new ArgumentNullException();
            if (weights.Length == 0) throw new EmptyWeightsException();
            if (inputs.Length + 1 > weights.Length) throw new ExcessiveInputsException();
            if (weights.Length > inputs.Length + 1) throw new ExcessiveWeightsException();
            double result = 0;
            for (int i = 0; i < inputs.Length; i++) result += weights[i] * inputs[i];
            result += weights.Last();
            return Sigmoid(result);
        }

        public static void NeuronLayerEval(double[,] wss, double[] ins) {
            if (wss == null || ins == null) throw new ArgumentNullException();
            if (wss.GetLength(0) == 0) throw new EmptyWeightsException();

        }

        public static double Sigmoid(double x) {
            if (x.Equals(double.NaN)) throw new ArgumentOutOfRangeException();
            if (x > 10) return 1;
            if (x < -10) return 0;
            return 1d / (1d + Math.Pow(Math.E, -x));
        }

        public class NeuralException : Exception {
            public NeuralException() : base() { }
            public NeuralException(string message) : base(message) { }
        }
        public class ExcessiveInputsException : NeuralException {
            public ExcessiveInputsException() : base() { }
            public ExcessiveInputsException(string message) : base(message) { }
        }
        public class ExcessiveWeightsException : NeuralException {
            public ExcessiveWeightsException() : base() { }
            public ExcessiveWeightsException(string message) : base(message) { }
        }
        public class EmptyWeightsException : NeuralException {
            public EmptyWeightsException() : base() { }
            public EmptyWeightsException(string message) : base(message) { }
        }
    }
}
