using System;
using System.Collections.Generic;
using System.Text;

namespace System.GenericArithmetics.Arithmetics
{
    struct Single : IFloatingPointArithmetic<float>
    {
        public float Zero => 0;
        public float One => 1;

        public float Infinity => float.PositiveInfinity;
        public float PositiveInfinity => float.PositiveInfinity;
        public float NegativeInfinity => float.NegativeInfinity;
        public float NaN => float.NaN;
        public float PI => MathF.PI;
        public float E => MathF.E;


        public float Next(float a)
        {
            if (float.IsNaN(a)) return a;
            if (a < 0) return -Previous(-a);
            if (a == float.PositiveInfinity) return a;

            var bits = BitConverter.ToInt32(BitConverter.GetBytes(a), 0);
            return BitConverter.ToSingle(BitConverter.GetBytes(bits + 1), 0);
        }
        
        public float Previous(float a)
        {
            if (float.IsNaN(a)) return a;
            if (a < 0) return -Next(-a);
            if (a == float.PositiveInfinity) return float.MaxValue;
            if (a == 0) return -float.Epsilon;

            var bits = BitConverter.ToInt32(BitConverter.GetBytes(a), 0);
            return BitConverter.ToSingle(BitConverter.GetBytes(bits - 1), 0);
        }


        public bool IsNaN(float a) => float.IsNaN(a);
        public bool IsInfinity(float a) => float.IsInfinity(a);
        
        public float Add(float a, float b) => a + b;
        public float Subtract(float a, float b) => a - b;
        public float Multiply(float a, float b) => a * b;
        public float Divide(float a, float b) => a / b;
        public float Remainder(float a, float b) => a % b;

        public float Negation(float a) => -a;
        public float Abs(float a) => MathF.Abs(a);
        public float Sign(float a) => MathF.Sign(a);

        public bool AreEqual(float a, float b) => a == b;
        public bool IsLessThan(float a, float b) => a < b;
        public bool IsLessOrEqual(float a, float b) => a <= b;
        public bool IsGreaterThan(float a, float b) => a > b;
        public bool IsGreaterOrEqual(float a, float b) => a >= b;
        public int Compare(float a, float b) => a.CompareTo(b);


        public bool AreApproximatelyEqual(float a, float b)
        {
            throw new NotImplementedException();
        }

        public float Max(params float[] list)
        {
            float m = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                m = MathF.Max(m, list[i]);
            }
            return m;
        }

        public float Min(params float[] list)
        {
            float m = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                m = MathF.Min(m, list[i]);
            }
            return m;
        }

        
        public float Floor(float a) => MathF.Floor(a);
        public float Ceiling(float a) => MathF.Ceiling(a);
        public float Round(float a) => MathF.Round(a);
        public float Round(float a, int digits) => MathF.Round(a, digits);
        public float Round(float a, MidpointRounding mode) => MathF.Round(a, mode);
        public float Round(float a, int digits, MidpointRounding mode) => MathF.Round(a, digits, mode);
        
        public float Pow(float a, float b) => MathF.Pow(a, b);
        public float Sqrt(float a) => MathF.Sqrt(a);
        public float Exp(float a) => MathF.Exp(a);
        public float Log(float a) => MathF.Log(a);
        public float Log10(float a) => MathF.Log10(a);
        
        public float Sin(float a) => MathF.Sin(a);
        public float Asin(float a) => MathF.Asin(a);
        public float Cos(float a) => MathF.Cos(a);
        public float Acos(float a) => MathF.Acos(a);
        public float Tan(float a) => MathF.Tan(a);
        public float Atan(float a) => MathF.Atan(a);
        public float Atan2(float x, float y) => MathF.Atan2(x, y);
        public float Sinh(float a) => MathF.Sinh(a);
        public float Cosh(float a) => MathF.Cosh(a);
        public float Tanh(float a) => MathF.Tanh(a);
    }
}
