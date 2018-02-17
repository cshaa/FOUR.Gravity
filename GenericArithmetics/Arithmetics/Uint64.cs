using System;
using System.Collections.Generic;
using System.Text;

namespace System.GenericArithmetics.Arithmetics
{
    public struct UInt64
        : IIntegerArithmetic<ulong>
    {
        public ulong Zero => 0;
        public ulong One => 1;
        
        public ulong Next(ulong a) => a + 1;
        public ulong Previous(ulong a) => a - 1;


        public ulong Add(ulong a, ulong b) => a + b;
        public ulong Subtract(ulong a, ulong b) => a - b;
        public ulong Multiply(ulong a, ulong b) => a * b;
        public ulong Divide(ulong a, ulong b) => a / b;
        public ulong Remainder(ulong a, ulong b) => a % b;

        public ulong Sign(ulong a)
            => Math.Sign((float)a) == 0 ? 0u : 1u;

        public bool AreEqual(ulong a, ulong b) => a == b;
        public bool IsLessThan(ulong a, ulong b) => a < b;
        public bool IsLessOrEqual(ulong a, ulong b) => a <= b;
        public bool IsGreaterThan(ulong a, ulong b) => a > b;
        public bool IsGreaterOrEqual(ulong a, ulong b) => a >= b;
        public int Compare(ulong a, ulong b) => a.CompareTo(b);

        public ulong Max(params ulong[] list)
        {
            ulong m = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                m = Math.Max(m, list[i]);
            }
            return m;
        }

        public ulong Min(params ulong[] list)
        {
            ulong m = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                m = Math.Min(m, list[i]);
            }
            return m;
        }


        public ulong BitwiseAnd(ulong a, ulong b) => a & b;
        public ulong BitwiseOr(ulong a, ulong b) => a | b;
        public ulong ExclusiveOr(ulong a, ulong b) => a ^ b;
        public ulong LeftShift(ulong a, int b) => a << b;
        public ulong RightShift(ulong a, int b) => a >> b;

        public ulong LeftShift(ulong a, ulong b)
            => a << (ushort)b;

        public ulong RightShift(ulong a, ulong b)
            => a >> (ushort)b;
        
        public ulong OnesComplement(ulong a) => ~a;

        public (ulong Quotient, ulong Reminder) DivRem(ulong a, ulong b)
            => (a / b, a % b);
    }
}
