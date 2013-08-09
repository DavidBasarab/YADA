using System;

namespace YADA.Acceptance.Extensions
{
    public static class NumberExtensions
    {
        private static Random _random;

        public static Random Random
        {
            get { return _random ?? (_random = new Random()); }
        }

        public static int NextRandom(int min = 0, int max = int.MaxValue)
        {
            if (max < min)
            {
                max = min;
            }

            return Random.Next(min, max);
        }

        public static decimal Round(this decimal? value, int digits = 2)
        {
            return !value.HasValue ? default(decimal) : value.Value.Round();
        }

        public static decimal Round(this decimal value, int digits = 2)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }
    }
}