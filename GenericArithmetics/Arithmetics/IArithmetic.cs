using System;
using System.Collections.Generic;
using System.Text;

namespace System.GenericArithmetics.Arithmetics
{
    public interface IArithmetic<T>
    {
        T Zero { get; }
        T One { get; }

        bool AreEqual(T a, T b);

        T Add(T a, T b);
        T Subtract(T a, T b);
        T Multiply(T a, T b);
        T Divide(T a, T b);

        T Sign(T a);
    }

    public interface IOrderedArithmetic<T> : IArithmetic<T>
    {
        T Next(T a);
        T Previous(T a);

        bool IsLessThan(T a, T b);
        bool IsLessOrEqual(T a, T b);
        bool IsGreaterThan(T a, T b);
        bool IsGreaterOrEqual(T a, T b);
        int Compare(T a, T b);
        
        T Remainder(T a, T b);

        T Max(params T[] a);
        T Min(params T[] a);
    }

    public interface ISignedArithmetic<T>
        : IArithmetic<T>
    {
        T Negation(T a);
        T Abs(T a);
    }


    public interface IIntegerArithmetic<T>
        : IOrderedArithmetic<T>
    {
        T BitwiseAnd(T a, T b);
        T BitwiseOr(T a, T b);
        T ExclusiveOr(T a, T b);
        T LeftShift(T a, T b);
        T RightShift(T a, T b);

        T LeftShift(T a, int b);
        T RightShift(T a, int b);

        T OnesComplement(T a);

        (T Quotient, T Reminder) DivRem(T a, T b);
    }

    public interface ISignedIntegerArithmetic<T>
        : IIntegerArithmetic<T>,
          ISignedArithmetic<T>
    { }
    

    public interface IUnorderedRealArithmetic<T>
        : ISignedArithmetic<T>
    {
        T PI { get; }
        T E { get; }

        bool AreApproximatelyEqual(T a, T b);

        T Pow(T a, T b);
        T Sqrt(T a);
        T Exp(T a);
        T Log(T a);
        T Log10(T a);

        T Sin(T a);
        T Asin(T a);
        T Cos(T a);
        T Acos(T a);
        T Tan(T a);
        T Atan(T a);
        T Atan2(T x, T y);
        T Cosh(T a);
        T Sinh(T a);
        T Tanh(T a);
    }

    public interface IRealArithmetic<T>
        : IUnorderedRealArithmetic<T>,
          IOrderedArithmetic<T>
    {
        T Floor(T a);
        T Ceiling(T a);
        T Round(T a);
        T Round(T a, int digits);
        T Round(T a, MidpointRounding mode);
        T Round(T a, int digits, MidpointRounding mode);
    }


    public interface IUnorderedFloatingPointArithmetic<T>
        : IUnorderedRealArithmetic<T>
    {
        T Infinity { get; }
        T NaN { get; }

        bool IsNaN(T a);
        bool IsInfinity(T a);
    }

    public interface IFloatingPointArithmetic<T>
        : IRealArithmetic<T>,
          IUnorderedFloatingPointArithmetic<T>
    {
        T PositiveInfinity { get; }
        T NegativeInfinity { get; }
    }

    public interface IDecimalArithmetic<T>
        : IRealArithmetic<T>
    {
        T Truncate(T a);
    }


    public interface IComplexArithmetic<T>
        : IUnorderedFloatingPointArithmetic<T>
    {
        T ImaginaryUnit { get; }

        T RealPart(T a);
        T ImaginaryPart(T a);
    }



    public interface IFloatingPointBitsArithmetic<T, BitsType>
        : IFloatingPointArithmetic<T>
    {
        IIntegerArithmetic<BitsType> BitsArithmetic { get; }

        BitsType ToBits(T a);
        T FromBits(BitsType a);
    }
}
