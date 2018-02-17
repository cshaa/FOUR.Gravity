using System;
using System.Collections.Generic;
using System.Text;

namespace System.GenericArithmetics.Arithmetics
{
    public struct Int32
        : ISignedIntegerArithmetic<int>
    {
        public int Zero => 0;
        public int One => 1;
        
        public int Next(int a) => a + 1;
        public int Previous(int a) => a - 1;


        public int Add(int a, int b) => a + b;
        public int Subtract(int a, int b) => a - b;
        public int Multiply(int a, int b) => a * b;
        public int Divide(int a, int b) => a / b;
        public int Remainder(int a, int b) => a % b;

        public int Negation(int a) => -a;
        public int Abs(int a) => Math.Abs(a);
        public int Sign(int a) => Math.Sign(a);

        public bool AreEqual(int a, int b) => a == b;
        public bool IsLessThan(int a, int b) => a < b;
        public bool IsLessOrEqual(int a, int b) => a <= b;
        public bool IsGreaterThan(int a, int b) => a > b;
        public bool IsGreaterOrEqual(int a, int b) => a >= b;
        public int Compare(int a, int b) => a.CompareTo(b);

        public int Max(params int[] list)
        {
            int m = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                m = Math.Max(m, list[i]);
            }
            return m;
        }

        public int Min(params int[] list)
        {
            int m = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                m = Math.Min(m, list[i]);
            }
            return m;
        }


        public int BitwiseAnd(int a, int b) => a & b;
        public int BitwiseOr(int a, int b) => a | b;
        public int ExclusiveOr(int a, int b) => a ^ b;
        public int LeftShift(int a, int b ) => a << b;
        public int RightShift(int a, int b) => a >> b;

        public int OnesComplement(int a) => ~a;

        public (int Quotient, int Reminder) DivRem(int a, int b)
        {
            var div = Math.DivRem(a, b, out var rem);
            return (div, rem);
        }
    }
}
