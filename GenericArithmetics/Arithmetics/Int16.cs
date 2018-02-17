using System;
using System.Collections.Generic;
using System.Text;

namespace System.GenericArithmetics.Arithmetics
{
    public struct Int16
        : ISignedIntegerArithmetic<short>
    {
        public short Zero => 0;
        public short One => 1;
        
        public short Next(short a) => (short)( a + 1 );
        public short Previous(short a) => (short)( a - 1 );


        public short Add(short a, short b) => (short)( a + b );
        public short Subtract(short a, short b) => (short)( a - b );
        public short Multiply(short a, short b) => (short)( a * b );
        public short Divide(short a, short b) => (short)( a / b );
        public short Remainder(short a, short b) => (short)( a % b );

        public short Negation(short a) => (short)( -a );
        public short Abs(short a) => Math.Abs(a);
        public short Sign(short a)
        {
            var sgn = Math.Sign(a);
            return sgn < 0 ? (short)-1
                : sgn == 0 ? (short)0
                : (short)1;
        }

        public bool AreEqual(short a, short b) => a == b;
        public bool IsLessThan(short a, short b) => a < b;
        public bool IsLessOrEqual(short a, short b) => a <= b;
        public bool IsGreaterThan(short a, short b) => a > b;
        public bool IsGreaterOrEqual(short a, short b) => a >= b;
        public int Compare(short a, short b) => a.CompareTo(b);

        public short Max(params short[] list)
        {
            short m = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                m = Math.Max(m, list[i]);
            }
            return m;
        }

        public short Min(params short[] list)
        {
            short m = list[0];
            for (int i = 1; i < list.Length; i++)
            {
                m = Math.Min(m, list[i]);
            }
            return m;
        }


        public short BitwiseAnd(short a, short b) => (short)( a & b );
        public short BitwiseOr(short a, short b) => (short)( a | b );
        public short ExclusiveOr(short a, short b) => (short)( a ^ b );
        public short LeftShift(short a, short b) => (short)( a << b );
        public short RightShift(short a, short b) => (short)( a >> b );
        public short LeftShift(short a, int b) => (short)( a << b );
        public short RightShift(short a, int b) => (short)( a >> b );



        public short OnesComplement(short a)
            => (short)( ~a & short.MaxValue );

        public (short Quotient, short Reminder) DivRem(short a, short b)
        {
            var div = (short)Math.DivRem(a, b, out var rem);
            return (div, (short)rem);
        }
    }
}
