using System;
using System.Security.Cryptography;

namespace ORB.DARP
{
    public static class RandomNumber
    {
        private static readonly RNGCryptoServiceProvider Generator = new RNGCryptoServiceProvider();

        public static int IntBetween(int min, int max)
        {
            byte[] randomNumber = new byte[1];

            Generator.GetBytes(randomNumber);

            double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);
            double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);

            int range = max - min + 1;

            double randomInRange = Math.Floor(multiplier * range);

            return (int)(min + randomInRange);
        }

        public static double DoubleBetween(int min, int max)
        {
            byte[] randomNumber = new byte[1];

            Generator.GetBytes(randomNumber);

            double asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);
            double multiplier = Math.Max(0, (asciiValueOfRandomCharacter / 255d) - 0.00000000001d);

            int range = max - min + 1;

            double randomInRange = Math.Floor(multiplier * range);

            return (min + randomInRange);
        }
    }
}