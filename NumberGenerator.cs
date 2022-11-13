using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2Iskd
{
    internal class NumberGenerator
    {
        public static double GetDoubleRandomValue(double minValue, double maxValue)
        {
            double min = Math.Round(minValue, 3);
            double max = Math.Round(maxValue, 3);

            if (min == max)
            {
                return minValue;
            }

            if (max < min)
            {
                min -= max;
                max += min;
                min = max - min;
            }

            int minInt = Convert.ToInt32(Math.Ceiling(min));
            int maxInt = Convert.ToInt32(Math.Floor(max));
            double value = minInt;
            double valuePlusRand;
            Random rnd = new();

            if (Math.Abs(maxInt - minInt) > 0)
            {
                value = rnd.Next(minInt, maxInt + 1);
            }
            do
            {
                valuePlusRand = value + (rnd.Next(0, 2) == 0 ? -1 : 1) * rnd.NextDouble();
            }
            while (valuePlusRand > max || valuePlusRand < min);

            return valuePlusRand;
        }
    }
}


