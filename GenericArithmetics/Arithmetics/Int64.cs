using System;
using System.Collections.Generic;
using System.Text;

namespace System.GenericArithmetics.Arithmetics
{
    public struct Int64
        : ISignedIntegerArithmetic<long>
    {
        public long Zero => 0;
        public long One => 1;
        
        public long Next(long a) => a + 1;
        public long Previous(long a) => a - 1;


        public long Add(long a, long b) => a + b;
        public long Subtract(long a, long b) => a - b;
        public long Multiply(long a, long b) => a * b;
        public long Divide(long a, long b) => a / b;
        public long Remainder(long a, long b) => a % b;

        public long Negation(long a) => -a;
        public long Abs(long a) => Math.Abs(a);
        public long Sign(long a) => Math.Sign(a);

        public bool AreEqual(long a, long b) => a == b;
        public bool IsLessThan(long a, long b) => a < b;
        public bool IsLessOrEqual(long a, long b) => a <= b;
        public bool IsGreaterThan(long a, long b) => a > b;
        public bool IsGreaterOrEqual(long a, long b) => a >= b;
        public int Compare(long a, long b) => a.CompareTo(b);

        public long Max(params long[] list)
        {
            long m = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                m = Math.Max(m, list[i]);
            }
            return m;
        }

        public long Min(params long[] list)
        {
            long m = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                m = Math.Min(m, list[i]);
            }
            return m;
        }


        public long BitwiseAnd(long a, long b) => a & b;
        public long BitwiseOr(long a, long b) => a | b;
        public long ExclusiveOr(long a, long b) => a ^ b;
        public long LeftShift(long a, int b) => a << b;
        public long RightShift(long a, int b) => a >> b;

        public long LeftShift(long a, long b) => a << (int)b;
        public long RightShift(long a, long b) => a >> (int)b;


        public long OnesComplement(long a) => ~a;

        public (long Quotient, long Reminder) DivRem(long a, long b)
        {
            var div = Math.DivRem(a, b, out var rem);
            return (div, rem);
        }
    }
}
