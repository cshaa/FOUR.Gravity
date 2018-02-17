using System;
using System.Collections.Generic;
using System.Text;

namespace MathNet.GeometricAlgebra.Extensions
{
    public interface IArithmetics<T>
    {
        T Zero { get; }
        T One { get; }

        T Add(T a, T b);
        T Subtract(T a, T b);
        T Multiply(T a, T b);
        T Divide(T a, T b);

        T Negation(T a);
    }

    public interface IIntegerArithmetics<T> : IArithmetics<T>
    {
        T BitwiseAnd(T a, T b);
        T BitwiseOr(T a, T b);
        T ExclusiveOr(T a, T b);

        T OnesComplement(T a);
    }


    public interface INumeric<T>
        : IEquatable<T>, IComparable<T>
    {
        IArithmetics<T> Arithmetics { get; }
        T Value { get; }

        T Next { get; }
        T Previous { get; }
    }

    public interface IInteger<T> : INumeric<T>
    {
        new IIntegerArithmetics<T> Arithmetics { get; }
    }
}
