using System;
using System.Collections.Generic;
using System.Text;

namespace System.GenericArithmetics.Arithmetics
{
    struct Decimal : IDecimalArithmetic<decimal>
    {
        public decimal Zero => 0;
        public decimal One => 1;

        public decimal PI => PI_CONST;
        public decimal E => E_CONST;

        private const decimal E_CONST  = 2.7182818284590452353602874713M;
        private const decimal PI_CONST = 3.1415926535897932384626433833M;
        private const decimal PI_HALF  = 1.5707963267948966192313216916M;
        private const decimal PI_TWO   = 6.2831853071795864769252867666M;



        public decimal Next(decimal a)
        {
            var delta = Pow10(LowestExponent(a));
            if (a != a + delta) return a + delta;
            return a + 10 * delta;
        }

        public decimal Previous(decimal a) => -Next(-a);


        /*
         * To test the Next and Previous methods, use these lambdas:
         * (x) => (x < Next(x)    ) && (x + Math.Abs(x - Next(x)    )/2 == x || x + Math.Abs(x - Next(x)    )/2 == Next(x)    );
         * (x) => (x > Previous(x)) && (x - Math.Abs(x - Previous(x))/2 == x || x - Math.Abs(x - Previous(x))/2 == Previous(x));
         */

        // Helper methods

        private static int LowestExponent(decimal a)
        {
            if (a < 0) return LowestExponent(-a);
            var i = (int)Math.Floor(Math.Log10((double)a));
            if (i <= 0) return -28;
            return i - 28;
        }

        private static decimal Pow10(int n)
        {
            if (n < -28) return 0;
            if (n < 0) return 1 / Pow10(-n);
            if (n > 28) throw new OverflowException("Value was either too large or too small for a Decimal.");

            decimal[] hash = {
                1m, 1_0000m, 1_0000_0000m, 1_0000_0000_0000m,
                1_0000_0000_0000_0000m, 1_0000_0000_0000_0000_0000m,
                1_0000_0000_0000_0000_0000_0000m, 1_0000_0000_0000_0000_0000_0000_0000m
            };

            var x = hash[n / 4];
            switch (n % 4)
            {
                case 0: return x;
                case 1: return x * 10;
                case 2: return x * 100;
                case 3: return x * 1000;
            }

            throw new Exception("Unknown error in decimal Pow10(int).");
        }

        // End of helper methods

        
        
        public decimal Add(decimal a, decimal b) => a + b;
        public decimal Subtract(decimal a, decimal b) => a - b;
        public decimal Multiply(decimal a, decimal b) => a * b;
        public decimal Divide(decimal a, decimal b) => a / b;
        public decimal Remainder(decimal a, decimal b) => a % b;

        public decimal Negation(decimal a) => -a;
        public decimal Abs(decimal a) => Math.Abs(a);
        public decimal Sign(decimal a) => Math.Sign(a);

        public bool AreEqual(decimal a, decimal b) => a == b;
        public bool IsLessThan(decimal a, decimal b) => a < b;
        public bool IsLessOrEqual(decimal a, decimal b) => a <= b;
        public bool IsGreaterThan(decimal a, decimal b) => a > b;
        public bool IsGreaterOrEqual(decimal a, decimal b) => a >= b;
        public int Compare(decimal a, decimal b) => a.CompareTo(b);


        public bool AreApproximatelyEqual(decimal a, decimal b)
        {
            throw new NotImplementedException();
        }

        public decimal Max(params decimal[] list)
        {
            decimal m = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                m = Math.Max(m, list[i]);
            }
            return m;
        }

        public decimal Min(params decimal[] list)
        {
            decimal m = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                m = Math.Min(m, list[i]);
            }
            return m;
        }


        public decimal Floor(decimal a) => Math.Floor(a);
        public decimal Ceiling(decimal a) => Math.Ceiling(a);
        public decimal Round(decimal a) => Math.Round(a);
        public decimal Round(decimal a, int digits) => Math.Round(a, digits);
        public decimal Round(decimal a, MidpointRounding mode) => Math.Round(a, mode);
        public decimal Round(decimal a, int digits, MidpointRounding mode) => Math.Round(a, digits, mode);

        /// <summary>
        ///     Computes the exponential function. Approximately 2000x slower than Math.Exp(double).
        /// </summary>
        public decimal Exp(decimal a)
        {
            if (a <= -66) return 0;
            if (a < 0) return 1 / Exp(-a);

            decimal next = a;
            decimal x = 0, y = 1;

            for (int i = 2; x != y; i++)
            {
                x = y;
                y += next;
                next *= a / i;
            }

            return x;
        }

        /// <summary>
        ///     Computes the natural logarithm of a. Approximately 250x slower than Math.Log(double).
        /// </summary>
        public decimal Log(decimal a)
        {
            switch (a)
            {
                case decimal x when x <= 0:
                    throw new ArgumentException("The logarithm can only take positive numbers.");

                case 1: return 0;
                case 2: return 0.69314718055994530941723212145M;
                case 3: return 1.09861228866810969139524523692M;
                case 4: return 1.38629436111989061883446424292M;
                case 5: return 1.60943791243410037460075933322M;
                case 6: return 1.79175946922805500081247735838M;
                case 7: return 1.94591014905531330510535274344M;
                case 8: return 2.07944154167983592825169636437M;
                case 9: return 2.19722457733621938279049047385M;
                case 10: return 2.30258509299404568401799145468M;

                case decimal x when x > 2:
                    ulong i = 2, j = 1;
                    decimal d;

                    while (true)
                    {
                        d = x / i;
                        if (d < 2) break;
                        if (d <= 10 && (int)d == d) break;
                        if (j == 62) break;
                        i <<= 1;
                        j++;
                    }
                    return j * Log(2) + Log(d);

                default:
                    decimal y1, y2 = 0;
                    do
                    {
                        y1 = y2;
                        y2 = y1 + a / Exp(y1) - 1;
                    }
                    while (y1 - y2 > 1e-28m);
                    return y1;
            }
        }


        public decimal Log10(decimal a) => Log(a) / Log(10);
        public decimal Pow(decimal a, decimal b) => Exp(b * Log(a));

        public decimal Sqrt(decimal a)
        {
            switch (a)
            {
                case decimal a_ when a_ < 0:
                    throw new ArgumentException("Taking a square root of a negative number is not allowed.");

                case 1: return 1;
                case 2: return 1.4142135623730950488016887242M;
                case 3: return 1.7320508075688772935274463415M;
                case 4: return 2;
                case 5: return 2.2360679774997896964091736688M;
            }
            
            decimal bigPart = 1;

            while (a >= 4294967296m)
            {
                a /= 4294967296m;
                bigPart *= 65536m;
            }

            while (a >= 65536m)
            {
                a /= 65536m;
                bigPart *= 256m;
            }
            
            while (a >= 256m)
            {
                a /= 256m;
                bigPart *= 16m;
            }
            
            while (a >= 4m)
            {
                a /= 4m;
                bigPart *= 2m;
            }

            decimal x = 0, y = 1;
            while (x != y)
            {
                y = 0.5m * (y + a / y);
                x = y;
            }

            return x * bigPart;
        }


        public decimal Sin(decimal a)
        {
            if (a >= PI_TWO) a -= PI_TWO * Math.Truncate(a / (PI_TWO));
            if (a > PI_CONST) return -Sin(a - PI_CONST);
            if (a > PI_HALF) return Sin(PI_HALF - a);

            decimal numerator = a;
            decimal denominator = 1;
            decimal x = 1, y = 0;
            a *= -a;

            for (int i = 2; x != y; i+=2)
            {
                x = y;
                y += numerator/denominator;
                numerator *= a;
                denominator *= i * (i + 1);
            }

            return x;
        }

        //TODO Implement a precise algorithm
        public decimal Asin(decimal a) => (decimal)Math.Asin((double)a);

        public decimal Cos(decimal a) => Sin(a + PI_HALF);
        public decimal Acos(decimal a) => PI_HALF - Asin(a);
        public decimal Tan(decimal a) => Sin(a) / Cos(a);

        //TODO Implement precise algorithms
        public decimal Atan(decimal a) => (decimal)Math.Atan((double)a);
        public decimal Atan2(decimal x, decimal y)
            => (decimal)Math.Atan2((double)x, (double)y);

        public decimal Sinh(decimal a)
        {
            var exp = Exp(a);
            return 0.5m * (exp - 1/exp);
        }

        public decimal Cosh(decimal a)
        {
            var exp = Exp(a);
            return 0.5m * (exp + 1 / exp);
        }
        
        public decimal Tanh(decimal a)
        {
            var exp = Exp(a);
            var inv = 1 / exp;

            return (exp - inv) / (exp + inv);
        }

        public decimal Truncate(decimal a) => Math.Truncate(a);
    }
}
