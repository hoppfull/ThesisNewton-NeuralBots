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

        public static double NeuronEval(Func<double, double> activation, double[] weights, double[] inputs) {
            if (activation == null || weights == null || inputs == null) throw new ArgumentNullException();
            if (weights.Length == 0) throw new EmptyWeightsException();
            if (inputs.Length + 1 > weights.Length) throw new ExcessiveInputsException();
            if (weights.Length > inputs.Length + 1) throw new ExcessiveWeightsException();
            return activation(zipWeightsAndInputs(weights, inputs));
        }

        public static double[] NeuronLayerEval(Func<double, double> activation, double[][] wss, double[] ins) {
            if (activation == null || wss == null || ins == null) throw new ArgumentNullException();
            double[] output = new double[wss.Length];
            for (int i = 0; i < wss.Length; i++) output[i] = NeuronEval(activation, wss[i], ins);
            return output;
        }

        //private static double[][] multiDimensionalArrayToJaggedArray(double[,] array) {
        //    double[][] result = new double[array.GetLength(0)][];
        //    for (int i = 0; i < array.GetLength(0); i++) {
        //        result[i] = new double[array.GetLength(1)];
        //        for (int j = 0; j < array.GetLength(1); j++) result[i][j] = array[j,i];
        //    }
        //    return result;
        //}

        private static double zipWeightsAndInputs(double[] weights, double[] inputs) {
            double result = 0;
            for (int i = 0; i < inputs.Length; i++) result += weights[i] * inputs[i];
            return result + weights.Last();

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
