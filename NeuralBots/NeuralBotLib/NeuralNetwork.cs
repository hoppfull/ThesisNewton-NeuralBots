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

        public static double NEval(double[] weights, double[] inputs) {
            if (weights.Length != inputs.Length+1) throw new IncorrectWeightInputException(weights.Length, inputs.Length);
            return Sigmoid(weights[0]);
        }

        public static double Sigmoid(double x) {
            if (x.Equals(double.NaN)) throw new ArgumentOutOfRangeException();
            if (x > 10) return 1;
            if (x < -10) return 0;
            return 1d / (1d + Math.Pow(Math.E, -x));
        }

        public class IncorrectWeightInputException : Exception {
            public enum ErrorTypes {
                ExcessiveInputs,
                ExcessiveWeights
            }

            public int WeightsLength { get; }
            public int InputsLength { get; }
            public ErrorTypes ErrorType { get; }

            public IncorrectWeightInputException(int weightLength, int inputsLength) : base() {
                WeightsLength = weightLength;
                InputsLength = inputsLength;
                if (WeightsLength > InputsLength)
                    ErrorType = ErrorTypes.ExcessiveWeights;
                else
                    ErrorType = ErrorTypes.ExcessiveInputs;
            }
        }
    }
}
