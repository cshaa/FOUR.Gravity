using System;
using System.Collections.Generic;
using System.Text;

namespace System.GenericArithmetics.Arithmetics
{
    public struct UInt16
        : IIntegerArithmetic<ushort>
    {
        public ushort Zero => 0;
        public ushort One => 1;
        
        public ushort Next(ushort a) => (ushort)( a + 1 );
        public ushort Previous(ushort a) => (ushort)( a - 1 );


        public ushort Add(ushort a, ushort b) => (ushort)( a + b );
        public ushort Subtract(ushort a, ushort b) => (ushort)( a - b );
        public ushort Multiply(ushort a, ushort b) => (ushort)( a * b );
        public ushort Divide(ushort a, ushort b) => (ushort)( a / b );
        public ushort Remainder(ushort a, ushort b) => (ushort)( a % b );
        
        public ushort Sign(ushort a)
        {
            var sgn = Math.Sign(a);
            return sgn == 0 ? (ushort)0 : (ushort)1;
        }

        public bool AreEqual(ushort a, ushort b) => a == b;
        public bool IsLessThan(ushort a, ushort b) => a < b;
        public bool IsLessOrEqual(ushort a, ushort b) => a <= b;
        public bool IsGreaterThan(ushort a, ushort b) => a > b;
        public bool IsGreaterOrEqual(ushort a, ushort b) => a >= b;
        public int Compare(ushort a, ushort b) => a.CompareTo(b);

        public ushort Max(params ushort[] list)
        {
            ushort m = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                m = Math.Max(m, list[i]);
            }
            return m;
        }

        public ushort Min(params ushort[] list)
        {
            ushort m = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                m = Math.Min(m, list[i]);
            }
            return m;
        }


        public ushort BitwiseAnd(ushort a, ushort b) => (ushort)( a & b );
        public ushort BitwiseOr(ushort a, ushort b) => (ushort)( a | b );
        public ushort ExclusiveOr(ushort a, ushort b) => (ushort)( a ^ b );
        public ushort LeftShift(ushort a, ushort b) => (ushort)( a << b );
        public ushort RightShift(ushort a, ushort b) => (ushort)( a >> b );
        public ushort LeftShift(ushort a, int b) => (ushort)( a << b );
        public ushort RightShift(ushort a, int b) => (ushort)( a >> b );



        public ushort OnesComplement(ushort a)
            => (ushort)( ~a & ushort.MaxValue );

        public (ushort Quotient, ushort Reminder) DivRem(ushort a, ushort b)
        {
            var div = (ushort)Math.DivRem(a, b, out var rem);
            return (div, (ushort)rem);
        }
    }
}
