using System;
using System.Collections.Generic;
using System.Text;

namespace System.GenericArithmetics.Arithmetics
{
    struct Double : IFloatingPointArithmetic<double>
    {
        public double Zero => 0;
        public double One => 1;

        public double Infinity => double.PositiveInfinity;
        public double PositiveInfinity => double.PositiveInfinity;
        public double NegativeInfinity => double.NegativeInfinity;
        public double NaN => double.NaN;
        public double PI => Math.PI;
        public double E => Math.E;


        public double Next(double a)
        {
            if (double.IsNaN(a)) return a;
            if (a < 0) return -Previous(-a);
            if (a == double.PositiveInfinity) return a;

            var bits = BitConverter.DoubleToInt64Bits(a);
            return BitConverter.Int64BitsToDouble(bits + 1);
        }
        
        public double Previous(double a)
        {
            if (double.IsNaN(a)) return a;
            if (a < 0) return -Next(-a);
            if (a == double.PositiveInfinity) return double.MaxValue;
            if (a == 0) return -double.Epsilon;

            var bits = BitConverter.DoubleToInt64Bits(a);
            return BitConverter.Int64BitsToDouble(bits - 1);
        }


        public bool IsNaN(double a) => double.IsNaN(a);
        public bool IsInfinity(double a) => double.IsInfinity(a);

        public double Add(double a, double b) => a + b;
        public double Subtract(double a, double b) => a - b;
        public double Multiply(double a, double b) => a * b;
        public double Divide(double a, double b) => a / b;
        public double Remainder(double a, double b) => a % b;

        public double Negation(double a) => -a;
        public double Abs(double a) => Math.Abs(a);
        public double Sign(double a) => Math.Sign(a);

        public bool AreEqual(double a, double b) => a == b;
        public bool IsLessThan(double a, double b) => a < b;
        public bool IsLessOrEqual(double a, double b) => a <= b;
        public bool IsGreaterThan(double a, double b) => a > b;
        public bool IsGreaterOrEqual(double a, double b) => a >= b;
        public int Compare(double a, double b) => a.CompareTo(b);


        public bool AreApproximatelyEqual(double a, double b)
        {
            throw new NotImplementedException();
        }

        public double Max(params double[] list)
        {
            double m = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                m = Math.Max(m, list[i]);
            }
            return m;
        }

        public double Min(params double[] list)
        {
            double m = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                m = Math.Min(m, list[i]);
            }
            return m;
        }

        
        public double Floor(double a) => Math.Floor(a);
        public double Ceiling(double a) => Math.Ceiling(a);
        public double Round(double a) => Math.Round(a);
        public double Round(double a, int digits) => Math.Round(a, digits);
        public double Round(double a, MidpointRounding mode) => Math.Round(a, mode);
        public double Round(double a, int digits, MidpointRounding mode) => Math.Round(a, digits, mode);
        
        public double Pow(double a, double b) => Math.Pow(a, b);
        public double Sqrt(double a) => Math.Sqrt(a);
        public double Exp(double a) => Math.Exp(a);
        public double Log(double a) => Math.Log(a);
        public double Log10(double a) => Math.Log10(a);
        
        public double Sin(double a) => Math.Sin(a);
        public double Asin(double a) => Math.Asin(a);
        public double Cos(double a) => Math.Cos(a);
        public double Acos(double a) => Math.Acos(a);
        public double Tan(double a) => Math.Tan(a);
        public double Atan(double a) => Math.Atan(a);
        public double Atan2(double x, double y) => Math.Atan2(x, y);
        public double Sinh(double a) => Math.Sinh(a);
        public double Cosh(double a) => Math.Cosh(a);
        public double Tanh(double a) => Math.Tanh(a);
    }
}
