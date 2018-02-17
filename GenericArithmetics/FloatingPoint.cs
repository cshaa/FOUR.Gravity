using System;
using System.Collections.Generic;
using System.Text;
using System.GenericArithmetics.Arithmetics;

namespace System.GenericArithmetics
{
    public partial struct FloatingPoint<T>
        : IEquatable<FloatingPoint<T>>,
          IComparable<FloatingPoint<T>>
    {
        public readonly T Value;

        static IFloatingPointArithmetic<T> Implementation;

        public FloatingPoint(T value, IFloatingPointArithmetic<T> implementation)
        {
            Implementation
                = Implementation == null
                ? (Implementation = implementation)
                : Implementation != implementation
                ? throw new InvalidOperationException("One type cannot have multiple implementations!")
                : Implementation;

            Value = value;
        }

        static FloatingPoint<T> Wrap(T value) => new FloatingPoint<T>(value, Implementation);
        public static implicit operator T(FloatingPoint<T> a) => a.Value;

        
        public FloatingPoint<T> Next => Wrap(Implementation.Next(Value));
        public FloatingPoint<T> Previous => Wrap(Implementation.Previous(Value));



        public int CompareTo(FloatingPoint<T> other)
            => Implementation.Compare(Value, other.Value);

        public bool Equals<S>(FloatingPoint<S> other)
            where S : IEquatable<S>, IComparable<S>
            => Value.Equals(other.Value);

        public bool Equals(FloatingPoint<T> other)
            => Value.Equals(other.Value);

        override public bool Equals(object other)
            => other is FloatingPoint<T> num
               ? Value.Equals(num)
               : Value.Equals(other);

        public override int GetHashCode()
            => Value.GetHashCode();

        public static bool operator ==(FloatingPoint<T> a, FloatingPoint<T> b)
            => Implementation.AreEqual(a, b);

        public static bool operator !=(FloatingPoint<T> a, FloatingPoint<T> b)
            => !Implementation.AreEqual(a, b);

        public static bool operator <(FloatingPoint<T> a, FloatingPoint<T> b)
            => Implementation.IsLessThan(a, b);

        public static bool operator >(FloatingPoint<T> a, FloatingPoint<T> b)
            => Implementation.IsGreaterThan(a, b);

        public static bool operator <=(FloatingPoint<T> a, FloatingPoint<T> b)
            => Implementation.IsLessOrEqual(a, b);

        public static bool operator >=(FloatingPoint<T> a, FloatingPoint<T> b)
            => Implementation.IsGreaterOrEqual(a, b);



        public static FloatingPoint<T> operator +(FloatingPoint<T> a, FloatingPoint<T> b)
            => Wrap(Implementation.Add(a, b));

        public static FloatingPoint<T> operator -(FloatingPoint<T> a, FloatingPoint<T> b)
            => Wrap(Implementation.Subtract(a, b));

        public static FloatingPoint<T> operator *(FloatingPoint<T> a, FloatingPoint<T> b)
            => Wrap(Implementation.Multiply(a, b));

        public static FloatingPoint<T> operator /(FloatingPoint<T> a, FloatingPoint<T> b)
            => Wrap(Implementation.Multiply(a, b));

        public static FloatingPoint<T> operator %(FloatingPoint<T> a, FloatingPoint<T> b)
            => Wrap(Implementation.Remainder(a, b));


        public static FloatingPoint<T> operator -(FloatingPoint<T> a)
            => Wrap(Implementation.Negation(a));
    }
}
