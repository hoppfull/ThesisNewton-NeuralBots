using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralBotLib {
    public static class Neural {
        #region NeuralNetwork
        public static double Neuron(Func<double, double> activation, double[] ws, IEnumerable<double> ins) {
            return activation(ws.Zip(ins, (w, i) => w * i).Aggregate(0d, (acc, pair) => acc + pair) + ws.LastOrDefault());
        }

        public static IEnumerable<double> Layer(Func<double, double> activation, double[][] wss, IEnumerable<double> ins) {
            return wss.Select(ws => Neuron(activation, ws, ins));
        }

        public static IEnumerable<double> Network(Func<double, double> activation, double[][][] wsss, IEnumerable<double> ins) {
            return wsss.Aggregate(ins, (input, wss) => Layer(activation, wss, input));
        }
        #endregion

        #region Activation functions
        public static double Sigmoid(double x) {
            if (x > 10) return 1;
            if (x < -10) return 0;
            return 1d / (1d + Math.Pow(Math.E, -x));
        }
        #endregion

        #region SupportFunctions
        public static bool IsJagged(double[][] arrays) {
            if (arrays.Length == 0 || arrays.Length == 1) return false;
            return !arrays.All(array => array.Length == arrays[0].Length);
        }

        public static bool Validation(double[][][] wsss, IEnumerable<double> ins) {
            if (wsss.Length == 0) return false; // no neurons in network?
            int prevWidth = ins.Count();
            foreach (double[][] wss in wsss) {
                if (wss[0].Length != prevWidth + 1) return false;
                if (IsJagged(wss)) return false;
                prevWidth = wss.Length;
            }
            return true;
        }

        public static double[][][] FoldExpression(double[] geneExpression, uint[] config) {
            double[][][] result = new double[Math.Max(config.Length - 1, 0)][][];
            int gene = 0;
            for (int i = 0; i < result.Length; i++) {
                result[i] = new double[config[i + 1]][];
                for (int j = 0; j < result[i].Length; j++) {
                    int nWeights = (int)config[i] + 1;
                    result[i][j] = geneExpression.Skip(gene).Take(nWeights).ToArray();
                    gene += nWeights;
                }
            }
            return result;
        }

        #endregion
    }
}
