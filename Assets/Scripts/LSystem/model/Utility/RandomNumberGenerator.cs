using System;

namespace LSystem.utility
{
    public class RandomNumberGenerator
    {
        private static Random instance = null;

        public static double GenerateDouble(double from, double to)
        {
            if (instance == null)
            {
                instance = new Random();
            }

            return instance.NextDouble() * (to - from) + from;
        }
    }
}
