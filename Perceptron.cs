using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2Iskd
{
    internal class Perceptron
    {
        private static readonly string pathToWeight = "C:\\Users\\slava\\source\\repos\\2Iskd\\2Iskd\\letters";
        private readonly Func<double, int> activationFunction;
        private readonly double thresholdValue;
        private readonly double learningSpeed;
        private double[,] weight;
        private readonly int imageWidth;
        private readonly int imageHeight;
        private readonly char letter;

        public Perceptron(char letter, double thresholdValue)
        {
            if (!char.IsLetter(letter))
            {
                throw new ArgumentOutOfRangeException("ArgumentOutOfRangeException: letter не является буквой.");
            }

            string[] dirsPaths;
            char[] availableLetters;
            int lettersNumber;

            imageWidth = 150;
            imageHeight = 120;
            this.letter = letter;
            InitializeWeight();
            try
            {
                weight = GetWeightFromFile();
            }
            catch (FileNotFoundException)
            {
                InitializeWeight();
                weight = GetWeightFromFile();
                return;
            }

            this.thresholdValue = thresholdValue;
            learningSpeed = 1e-1;
            activationFunction = net =>
            {
                return net >= this.thresholdValue ? 1 : 0;
            };

            dirsPaths = Directory.GetDirectories(@"C:\\Users\\slava\\source\\repos\\2Iskd\\2Iskd\\letters");
            lettersNumber = dirsPaths.Length;

            availableLetters = new char[lettersNumber];
            for (int i = 0; i < lettersNumber; i++)
            {
                availableLetters[i] = dirsPaths[i].Last();
            }

            if (!availableLetters.Contains(letter))
            {
                throw new ArgumentOutOfRangeException("ArgumentOutOfRangeException: персептрон не может обучаться letter.");
            }
        }

        public void Learning()
        {
            var dirs = Directory.GetDirectories("C:\\Users\\slava\\source\\repos\\2Iskd\\2Iskd\\letters");
            var imagesForThisLetter = Directory.GetFiles($"C:\\Users\\slava\\source\\repos\\2Iskd\\2Iskd\\letters\\{ letter}");
            List<string> otherImages = new();

            foreach (var dir in dirs)
            {
                if (dir.Last() == letter || !"ПЕТЛ".Contains(dir.Last()))
                {
                    continue;
                }
                otherImages.AddRange(Directory.GetFiles(dir));
            }

            while (true)
            {
                int i;

                for (i = 0; i < imagesForThisLetter.Length; i++)
                {
                    var image = imagesForThisLetter[i];
                    var bitmap = new Bitmap(image);
                    var inputSignals = GetInputSignals(bitmap);

                    if (activationFunction(GetNet(inputSignals)) == 1)
                    {
                        continue;
                    }
                    ChangeWeight(1, 0, inputSignals);
                    i = 0;
                }
                for (i = 0; i < otherImages.Count; i++)
                {
                    var image = otherImages[i];
                    var bitmap = new Bitmap(image);
                    var inputSignals = GetInputSignals(bitmap);

                    if (activationFunction(GetNet(inputSignals)) == 0)
                    {
                        continue;
                    }
                    ChangeWeight(0, 1, inputSignals);
                    break;
                }
                if (i == otherImages.Count)
                {
                    break;
                }
            }

            WriteWeightToFile();
        }

        public void InitializeWeight()
        {
            weight = new double[imageHeight, imageWidth];
            for (int i = 0; i < imageHeight; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    weight[i, j] = NumberGenerator.GetDoubleRandomValue(1e-8, 1);
                }
            }

            WriteWeightToFile();
        }

        public bool Identify(Bitmap bitmap)
        {
            return activationFunction(GetNet(GetInputSignals(bitmap))) == 1;
        }

        public double GetCloseness(Bitmap bitmap)
        {
            return Math.Abs(thresholdValue - GetNet(GetInputSignals(bitmap)));
        }

        private double GetNet(int[,] inputSignals)
        {
            double net = 0;

            for (int i = 0; i < imageHeight; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    net += weight[i, j] * inputSignals[i, j];
                }
            }

            return net;
        }

        private void ChangeWeight(int expectedResult, int outResult, int[,] inputSignals)
        {
            for (int i = 0; i < imageHeight; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    weight[i, j] += learningSpeed * (expectedResult - outResult) * inputSignals[i, j];
                }
            }
        }

        private int[,] GetInputSignals(Bitmap bitmap)
        {
            int[,] inputSignals = new int[imageHeight, imageWidth];

            for (int i = 0; i < imageHeight; i++)
            {
                for (int j = 0; j < imageWidth; j++)
                {
                    inputSignals[i, j] = bitmap.GetPixel(j, i).A == 0 ? 0 : 1;
                }
            }

            return inputSignals;
        }

        private double[,] GetWeightFromFile()
        {
            double[,] weightReturn = new double[imageHeight, imageWidth];
            StreamReader reader = new(@$"C:\\Users\\slava\\source\\repos\\2Iskd\\2Iskd\\letters\\Weight\\{letter}.txt");
            for (int i = 0; i < imageHeight; i++)
            {
                var line = reader.ReadLine();
                var weightStr = line.Split(" ");

                for (int j = 0; j < imageWidth; j++)
                {
                    weightReturn[i, j] = Convert.ToDouble(weightStr[j]);
                }
            }

            reader.Close();

            return weightReturn;
        }

        private void WriteWeightToFile()
        {
            StreamWriter writer = new(@$"{pathToWeight}\Weight\{letter}.txt", false);

            for (int i = 0; i < imageHeight; i++)
            {
                StringBuilder builder = new(weight[i, 0].ToString());

                for (int j = 1; j < imageWidth; j++)
                {
                    builder.Append(" " + weight[i, j]);
                }
                writer.WriteLine(builder);
            }

            writer.Flush();
            writer.Close();
        }

    }
}
