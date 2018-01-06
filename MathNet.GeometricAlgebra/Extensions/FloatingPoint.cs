using System;
using System.Collections.Generic;
using System.Text;

namespace MathNet.Extensions
{

    public static class FloatingPoint
    {

        public static double Next(this double d)
        {
            if (double.IsNaN(d)) return d;
            if (d < 0) return -(-d).Previous();
            if (d == double.PositiveInfinity) return d;

            return Binary.DoubleFromBits(d.ToBits() + 1);
        }

        public static float Next(this float d)
        {
            if (float.IsNaN(d)) return d;
            if (d < 0) return -(-d).Previous();
            if (d == float.PositiveInfinity) return d;

            return Binary.SingleFromBits(d.ToBits() + 1);
        }


        public static double Previous(this double d)
        {
            if (double.IsNaN(d)) return d;
            if (d < 0) return -(-d).Next();
            if (d == double.PositiveInfinity) return double.MaxValue;
            if (d == 0) return -double.Epsilon;

            return Binary.DoubleFromBits(d.ToBits() - 1);
        }

        public static float Previous(this float d)
        {
            if (float.IsNaN(d)) return d;
            if (d < 0) return -(-d).Next();
            if (d == double.PositiveInfinity) return float.MaxValue;
            if (d == 0) return -float.Epsilon;

            return Binary.SingleFromBits(d.ToBits() - 1);
        }


        public static double Ulp(this double d)
        {
            if (double.IsNaN(d)) return d;
            if (d < 0) d = -d; // ulp(d) == ulp(-d)
            if (d == double.PositiveInfinity) return d;

            var upperUlp = Math.Abs(d.Next() - d);
            var lowerUlp = Math.Abs(d.Previous() - d);

            return Math.Max(upperUlp, lowerUlp);
        }

        public static float Ulp(this float d)
        {
            if (float.IsNaN(d)) return d;
            if (d < 0) d = -d; // ulp(d) == ulp(-d)
            if (d == float.PositiveInfinity) return d;

            var upperUlp = Math.Abs(d.Next() - d);
            var lowerUlp = Math.Abs(d.Previous() - d);

            return Math.Max(upperUlp, lowerUlp);
        }


        public static long UlpsApart(this double self, double other)
        {
            // algorithm from https://stackoverflow.com/a/10425732/1137334

            var selfBits = self.ToBits();
            if (selfBits < 0) selfBits = (long)(0x8000000000000000 - (ulong)selfBits);

            var otherBits = other.ToBits();
            if (otherBits < 0) otherBits = (long)(0x8000000000000000 - (ulong)otherBits);

            return otherBits - selfBits;
        }

        public static int UlpsApart(this float self, float other)
        {
            // algorithm from https://stackoverflow.com/a/10425732/1137334

            var selfBits = self.ToBits();
            if (selfBits < 0) selfBits = (int)(0x80000000 - (uint)selfBits);

            var otherBits = other.ToBits();
            if (otherBits < 0) otherBits = (int)(0x80000000 - (uint)otherBits);

            return otherBits - selfBits;
        }

    }
}
