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
            Learning.TrainingExample[] exs_pics = new Learning.TrainingExample[] {
                new Learning.TrainingExample(readImage(new Bitmap("1.bmp")).Select(b => (double)b).ToArray(), new double[] { 0, 1 }),
                new Learning.TrainingExample(readImage(new Bitmap("2.bmp")).Select(b => (double)b).ToArray(), new double[] { 0, 1 }),
                new Learning.TrainingExample(readImage(new Bitmap("3.bmp")).Select(b => (double)b).ToArray(), new double[] { 0, 1 }),
                new Learning.TrainingExample(readImage(new Bitmap("4.bmp")).Select(b => (double)b).ToArray(), new double[] { 0, 1 }),
                new Learning.TrainingExample(readImage(new Bitmap("5.bmp")).Select(b => (double)b).ToArray(), new double[] { 0, 1 }),
                new Learning.TrainingExample(readImage(new Bitmap("6.bmp")).Select(b => (double)b).ToArray(), new double[] { 0, 1 }),
                new Learning.TrainingExample(readImage(new Bitmap("7.bmp")).Select(b => (double)b).ToArray(), new double[] { 0, 1 }),
                new Learning.TrainingExample(readImage(new Bitmap("8.bmp")).Select(b => (double)b).ToArray(), new double[] { 0, 1 }),
                new Learning.TrainingExample(readImage(new Bitmap("9.bmp")).Select(b => (double)b).ToArray(), new double[] { 0, 1 }),
                new Learning.TrainingExample(readImage(new Bitmap("10.bmp")).Select(b => (double)b).ToArray(), new double[] { 0, 1 }),
                new Learning.TrainingExample(readImage(new Bitmap("11.bmp")).Select(b => (double)b).ToArray(), new double[] { 1, 0 }),
                new Learning.TrainingExample(readImage(new Bitmap("12.bmp")).Select(b => (double)b).ToArray(), new double[] { 1, 0 }),
                new Learning.TrainingExample(readImage(new Bitmap("13.bmp")).Select(b => (double)b).ToArray(), new double[] { 1, 0 }),
                new Learning.TrainingExample(readImage(new Bitmap("14.bmp")).Select(b => (double)b).ToArray(), new double[] { 1, 0 }),
                new Learning.TrainingExample(readImage(new Bitmap("15.bmp")).Select(b => (double)b).ToArray(), new double[] { 1, 0 }),
                new Learning.TrainingExample(readImage(new Bitmap("16.bmp")).Select(b => (double)b).ToArray(), new double[] { 1, 0 }),
                new Learning.TrainingExample(readImage(new Bitmap("17.bmp")).Select(b => (double)b).ToArray(), new double[] { 1, 0 }),
                new Learning.TrainingExample(readImage(new Bitmap("18.bmp")).Select(b => (double)b).ToArray(), new double[] { 1, 0 }),
                new Learning.TrainingExample(readImage(new Bitmap("19.bmp")).Select(b => (double)b).ToArray(), new double[] { 1, 0 }),
                new Learning.TrainingExample(readImage(new Bitmap("20.bmp")).Select(b => (double)b).ToArray(), new double[] { 1, 0 })
            };

            uint[] config = new uint[] { 16 * 16, 20, 10, 2 };

            double[] solution = Learning.TrainNeuralNetwork(exs_pics, config, Genetics.Hash);
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

        static void TestOR() {
            uint[] config = new uint[] { 2, 2, 1 };

            Learning.TrainingExample[] exs_OR = new Learning.TrainingExample[] {
                new Learning.TrainingExample(new double[] { 0, 0 }, new double[] { 0 }),
                new Learning.TrainingExample(new double[] { 0, 1 }, new double[] { 1 }),
                new Learning.TrainingExample(new double[] { 1, 0 }, new double[] { 1 }),
                new Learning.TrainingExample(new double[] { 1, 1 }, new double[] { 1 })
            };

            double[] solution = Learning.TrainNeuralNetwork(exs_OR, config, Genetics.Hash);

            double[][][] wsss = Neural.FoldExpression(solution, config);

            double result0 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 0, 0 }).ToArray()[0];
            double result1 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 1, 0 }).ToArray()[0];
            double result2 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 0, 1 }).ToArray()[0];
            double result3 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 1, 1 }).ToArray()[0];

            Console.WriteLine("OR:");
            Console.WriteLine($"false Or false = {result0}");
            Console.WriteLine($"true Or false = {result1}");
            Console.WriteLine($"false Or true = {result2}");
            Console.WriteLine($"true Or true = {result3}");
        }

        static void TestAND() {
            uint[] config = new uint[] { 2, 2, 1 };

            Learning.TrainingExample[] exs_AND = new Learning.TrainingExample[] {
                new Learning.TrainingExample(new double[] { 0, 0 }, new double[] { 0 }),
                new Learning.TrainingExample(new double[] { 0, 1 }, new double[] { 0 }),
                new Learning.TrainingExample(new double[] { 1, 0 }, new double[] { 0 }),
                new Learning.TrainingExample(new double[] { 1, 1 }, new double[] { 1 })
            };

            double[] solution = Learning.TrainNeuralNetwork(exs_AND, config, Genetics.Hash);

            double[][][] wsss = Neural.FoldExpression(solution, config);

            double result0 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 0, 0 }).ToArray()[0];
            double result1 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 1, 0 }).ToArray()[0];
            double result2 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 0, 1 }).ToArray()[0];
            double result3 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 1, 1 }).ToArray()[0];

            Console.WriteLine("AND:");
            Console.WriteLine($"false And false = {result0}");
            Console.WriteLine($"true And false = {result1}");
            Console.WriteLine($"false And true = {result2}");
            Console.WriteLine($"true And true = {result3}");
        }

        static void TestXOR() {
            uint[] config = new uint[] { 2, 2, 1 };

            Learning.TrainingExample[] exs_XOR = new Learning.TrainingExample[] {
                new Learning.TrainingExample(new double[] { 0, 0 }, new double[] { 0 }),
                new Learning.TrainingExample(new double[] { 0, 1 }, new double[] { 1 }),
                new Learning.TrainingExample(new double[] { 1, 0 }, new double[] { 1 }),
                new Learning.TrainingExample(new double[] { 1, 1 }, new double[] { 0 })
            };

            double[] solution = Learning.TrainNeuralNetwork(exs_XOR, config, Genetics.Hash);

            double[][][] wsss = Neural.FoldExpression(solution, config);

            double result0 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 0, 0 }).ToArray()[0];
            double result1 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 1, 0 }).ToArray()[0];
            double result2 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 0, 1 }).ToArray()[0];
            double result3 = Neural.Network(Neural.Sigmoid, wsss, new double[] { 1, 1 }).ToArray()[0];

            Console.WriteLine("XOR:");
            Console.WriteLine($"false Xor false = {result0}");
            Console.WriteLine($"true Xor false = {result1}");
            Console.WriteLine($"false Xor true = {result2}");
            Console.WriteLine($"true Xor true = {result3}");
        }
    }
}
