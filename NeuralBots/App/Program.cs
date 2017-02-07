using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuralBotLib;

namespace App {
    class Program {
        static void Main(string[] args) {
            //Console.WriteLine(NeuralNetwork.NeuronLayerEval(x=>x,new double[,] { {0 }, {5 } }, new double[] {0 }));
            var arr = new int[,] { { 1, 2, 10 },
                                   { 3, 4, 20 } };

            Console.WriteLine(arr[0, 0]);
            Console.WriteLine(arr[0, 1]);
            Console.WriteLine(arr[1, 0]);
            Console.WriteLine(arr[1, 1]);
            Console.WriteLine(arr.GetLength(0)); // 3
            Console.WriteLine(arr.GetLength(1)); // 2
        }
    }
}
