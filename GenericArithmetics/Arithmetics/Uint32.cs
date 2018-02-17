using System;
using System.Collections.Generic;
using System.Text;

namespace System.GenericArithmetics.Arithmetics
{
    public struct UInt32
        : IIntegerArithmetic<uint>
    {
        public uint Zero => 0;
        public uint One => 1;
        
        public uint Next(uint a) => a + 1;
        public uint Previous(uint a) => a - 1;


        public uint Add(uint a, uint b) => a + b;
        public uint Subtract(uint a, uint b) => a - b;
        public uint Multiply(uint a, uint b) => a * b;
        public uint Divide(uint a, uint b) => a / b;
        public uint Remainder(uint a, uint b) => a % b;
        
        public uint Sign(uint a)
            => Math.Sign(a) == 0 ? 0u : 1u;

        public bool AreEqual(uint a, uint b) => a == b;
        public bool IsLessThan(uint a, uint b) => a < b;
        public bool IsLessOrEqual(uint a, uint b) => a <= b;
        public bool IsGreaterThan(uint a, uint b) => a > b;
        public bool IsGreaterOrEqual(uint a, uint b) => a >= b;
        public int Compare(uint a, uint b) => a.CompareTo(b);

        public uint Max(params uint[] list)
        {
            uint m = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                m = Math.Max(m, list[i]);
            }
            return m;
        }

        public uint Min(params uint[] list)
        {
            uint m = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                m = Math.Min(m, list[i]);
            }
            return m;
        }


        public uint BitwiseAnd(uint a, uint b) => a & b;
        public uint BitwiseOr(uint a, uint b) => a | b;
        public uint ExclusiveOr(uint a, uint b) => a ^ b;
        public uint LeftShift(uint a, int b) => a << b;
        public uint RightShift(uint a, int b) => a >> b;

        public uint LeftShift(uint a, uint b)
            => a << (ushort)b;

        public uint RightShift(uint a, uint b)
            => a >> (ushort)b;

        public uint OnesComplement(uint a) => ~a;

        public (uint Quotient, uint Reminder) DivRem(uint a, uint b)
        {
            var div = (uint)Math.DivRem(a, b, out long rem);
            return (div, (uint)rem);
        }
    }
}
