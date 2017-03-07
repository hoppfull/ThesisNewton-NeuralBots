using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NeuralBotLib;

namespace App {
    class Program {
        static void Main(string[] args) {
            Learning.TrainingExample[] exs_pics = new Learning.TrainingExample[100];

            for (int i = 1; i <= 50; i++)
                exs_pics[i - 1] = new Learning.TrainingExample(readImage(new Bitmap($"{string.Format("o{00:00}.bmp", i)}")).Select(b => (double)b).ToArray(), new double[] { 0, 1 });

            for (int i = 1; i <= 50; i++)
                exs_pics[i + 49] = new Learning.TrainingExample(readImage(new Bitmap($"{string.Format("x{00:00}.bmp", i)}")).Select(b => (double)b).ToArray(), new double[] { 1, 0 });

            uint[] config = new uint[] { 16 * 16, 30, 4, 2 };

            //double[] solution = Learning.TrainNeuralNetworkRuleofTwo(exs_pics, config, Genetics.Hash, 1);
            double[] solution = Learning.TrainNeuralNetworkSelectiveBreeding(exs_pics, config, Genetics.Hash, 50);
            //double[] solution = Learning.TrainNeuralNetworkRoulette(exs_pics, config, Genetics.Hash, 1);

            double[][][] wsss = Neural.FoldExpression(solution, config);
            double[] cross = Neural.Network(Neural.Sigmoid, wsss, readImage(new Bitmap("test01o.bmp")).Select(b => (double)b)).ToArray();
            double[] circle = Neural.Network(Neural.Sigmoid, wsss, readImage(new Bitmap("test11x.bmp")).Select(b => (double)b)).ToArray();

            int nCorrects = 0;
            for (int i = 1; i <= 10; i++) {
                double[] outs= Neural.Network(Neural.Sigmoid, wsss, readImage(new Bitmap(string.Format("test{00:00}o.bmp", i))).Select(b => (double)b)).ToArray();
                Console.WriteLine($"TESTING CROSS\n\tProbability of cross: {outs[0]}, Probability of circle: {outs[1]}");
                if (outs[0] < outs[1]) nCorrects++;
            }
            for (int i = 11; i <= 20; i++) {
                double[] outs = Neural.Network(Neural.Sigmoid, wsss, readImage(new Bitmap(string.Format("test{00:00}x.bmp", i))).Select(b => (double)b)).ToArray();
                Console.WriteLine($"TESTING CIRCLE\n\tProbability of cross: {outs[0]}, Probability of circle: {outs[1]}");
                if (outs[0] > outs[1]) nCorrects++;
            }
            Console.WriteLine($"nCorrects={nCorrects}");
        }

        public static void printPic(Bitmap image) {
            byte[] pic = readImage(image);
            for (int x = 0; x < image.Width; x++) {
                for (int y = 0; y < image.Height; y++)
                    Console.Write(pic[y * image.Width + x] == 0 ? 'X' : '.');
                Console.WriteLine();
            }
        }

        public static byte[] readImage(Bitmap image) {
            byte[] result = new byte[image.Width * image.Height];
            for (int x = 0; x < image.Width; x++)
                for (int y = 0; y < image.Height; y++)
                    result[y * image.Width + x] = image.GetPixel(x, y).R;
            return result;
        }

        //static void TestOR() {
        //    uint[] config = new uint[] { 2, 2, 1 };

        //    Learning.TrainingExample[] exs_OR = new Learning.TrainingExample[] {
        //        new Learning.TrainingExample(new double[] { 0, 0 }, new double[] { 0 }),
        //        new Learning.TrainingExample(new double[] { 0, 1 }, new double[] { 1 }),
        //        new Learning.TrainingExample(new double[] { 1, 0 }, new double[] { 1 }),
        //        new Learning.TrainingExample(new double[] { 1, 1 }, new double[] { 1 })
        //    };

        //    double[] solution = Learning.TrainNeuralNetworkRoulette(exs_OR, config, Genetics.Hash);

        //    double[][][] wsss = Neural.FoldExpression(solution, config);

        //    double result0 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 0, 0 }).ToArray()[0];
        //    double result1 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 1, 0 }).ToArray()[0];
        //    double result2 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 0, 1 }).ToArray()[0];
        //    double result3 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 1, 1 }).ToArray()[0];

        //    Console.WriteLine("OR:");
        //    Console.WriteLine($"false Or false = {result0}");
        //    Console.WriteLine($"true Or false = {result1}");
        //    Console.WriteLine($"false Or true = {result2}");
        //    Console.WriteLine($"true Or true = {result3}");
        //}

        //static void TestAND() {
        //    uint[] config = new uint[] { 2, 2, 1 };

        //    Learning.TrainingExample[] exs_AND = new Learning.TrainingExample[] {
        //        new Learning.TrainingExample(new double[] { 0, 0 }, new double[] { 0 }),
        //        new Learning.TrainingExample(new double[] { 0, 1 }, new double[] { 0 }),
        //        new Learning.TrainingExample(new double[] { 1, 0 }, new double[] { 0 }),
        //        new Learning.TrainingExample(new double[] { 1, 1 }, new double[] { 1 })
        //    };

        //    double[] solution = Learning.TrainNeuralNetwork(exs_AND, config, Genetics.Hash);

        //    double[][][] wsss = Neural.FoldExpression(solution, config);

        //    double result0 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 0, 0 }).ToArray()[0];
        //    double result1 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 1, 0 }).ToArray()[0];
        //    double result2 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 0, 1 }).ToArray()[0];
        //    double result3 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 1, 1 }).ToArray()[0];

        //    Console.WriteLine("AND:");
        //    Console.WriteLine($"false And false = {result0}");
        //    Console.WriteLine($"true And false = {result1}");
        //    Console.WriteLine($"false And true = {result2}");
        //    Console.WriteLine($"true And true = {result3}");
        //}

        //static void TestXOR() {
        //    uint[] config = new uint[] { 2, 5, 1 };

        //    Learning.TrainingExample[] exs_XOR = new Learning.TrainingExample[] {
        //        new Learning.TrainingExample(new double[] { 0, 0 }, new double[] { 0 }),
        //        new Learning.TrainingExample(new double[] { 0, 1 }, new double[] { 1 }),
        //        new Learning.TrainingExample(new double[] { 1, 0 }, new double[] { 1 }),
        //        new Learning.TrainingExample(new double[] { 1, 1 }, new double[] { 0 })
        //    };

        //    //double[] solution = Learning.TrainNeuralNetworkRuleofTwo(exs_XOR, config, Genetics.Hash, 0.01);
        //    //double[] solution = Learning.TrainNeuralNetworkSelectiveBreeding(exs_XOR, config, Genetics.Hash, 0.01);
        //    double[] solution = Learning.TrainNeuralNetworkRoulette(exs_XOR, config, Genetics.Hash, 0.01);

        //    double[][][] wsss = Neural.FoldExpression(solution, config);

        //    double result0 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 0, 0 }).ToArray()[0];
        //    double result1 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 1, 0 }).ToArray()[0];
        //    double result2 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 0, 1 }).ToArray()[0];
        //    double result3 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 1, 1 }).ToArray()[0];

        //    Console.WriteLine("XOR:");
        //    Console.WriteLine($"false Xor false = {result0}");
        //    Console.WriteLine($"true Xor false = {result1}");
        //    Console.WriteLine($"false Xor true = {result2}");
        //    Console.WriteLine($"true Xor true = {result3}");
        //}
    }
}
