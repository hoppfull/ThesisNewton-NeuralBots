using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralBotLib {
    public static class Learning {
        public class TrainingExample {
            public double[] Input { get; }
            public double[] Output { get; }
            public TrainingExample(double[] Input, double[] Output) {
                this.Input = Input;
                this.Output = Output;
            }
        }

        public static double Cost(TrainingExample[] exs, Func<double[], double[]> hypothesis) {
            Func<double, double, double> w = (y, yHat) => Math.Pow(y - yHat, 2);
            return exs.Aggregate(0d, (acc, ex) => acc + ex.Output.Zip(hypothesis(ex.Input), w).Sum());
        }
    }
}
