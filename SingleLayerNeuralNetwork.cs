using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2Iskd
{
    internal class SingleLayerNeuralNetwork
    {
        public SingleLayerNeuralNetwork(char[] letters, double[] thresholdValues)
        {
            perceptrons = new();

            for (int i = 0; i < letters.Length; i++)
            {
                char letter = letters[i];

                try
                {
                    perceptrons.Add(letter, new Perceptron(letter, thresholdValues[i]));
                }
                catch (ArgumentOutOfRangeException e)
                {
                    throw e;
                }
            }
        }

        public (char letter, bool isSure) Identify(Bitmap bitmap)
        {
            try
            {
                return (perceptrons.First(p => p.Value.Identify(bitmap)).Key, true);
            }
            catch (InvalidOperationException)
            {

            }

            int numberOfPerceptrons = perceptrons.Count;
            double[] closeness = new double[numberOfPerceptrons];
            char[] letters = new char[numberOfPerceptrons];

            int i = 0;
            foreach (var perceptron in perceptrons)
            {
                letters[i] = perceptron.Key;
                closeness[i++] = perceptron.Value.GetCloseness(bitmap);
            }

            return (letters[Array.IndexOf(closeness, closeness.Min())], false);
        }

        public void Learning()
        {
            foreach (var perceptron in perceptrons.Values)
            {
                perceptron.Learning();
            }
        }

        private readonly Dictionary<char, Perceptron> perceptrons;
    }


}
