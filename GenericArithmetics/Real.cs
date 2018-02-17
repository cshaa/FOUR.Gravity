using System.GenericArithmetics.Arithmetics;

namespace System.GenericArithmetics
{
    public struct Real<T>
        : IEquatable<Real<T>>,
          IComparable<Real<T>>
    {
        public readonly T Value;
        public static IRealArithmetic<T> Arithmetic => Implementation;
        static IRealArithmetic<T> Implementation;

        static Real()
        {
            Real<float>.InitializeArithmetic(default(Arithmetics.Single));
            Real<double>.InitializeArithmetic(default(Arithmetics.Double));
        }

        public Real(T value)
        {
            Value = value;
        }

        public static void InitializeArithmetic(IRealArithmetic<T> implementation)
        {
            if (Implementation != null)
                throw new InvalidOperationException("Arithmetic for type " + typeof(T).FullName + " has already been initialized.");

            if (implementation == null)
                throw new InvalidOperationException("Cannot initalize arithmetic for type " + typeof(T).FullName + " with null.");

            Implementation = implementation;
        }

        public static implicit operator Real<T>(T value) => new Real<T>(value);
        public static implicit operator T(Real<T> a) => a.Value;


        public Real<T> Next => Implementation.Next(Value);
        public Real<T> Previous => Implementation.Previous(Value);



        public int CompareTo(Real<T> other)
            => Implementation.Compare(Value, other.Value);

        public bool Equals<S>(Real<S> other)
            where S : IEquatable<S>, IComparable<S>
            => Value.Equals(other.Value);

        public bool Equals(Real<T> other)
            => Value.Equals(other.Value);

        override public bool Equals(object other)
            => other is Real<T> num
               ? Value.Equals(num)
               : Value.Equals(other);

        public override int GetHashCode()
            => Value.GetHashCode();

        public static bool operator ==(Real<T> a, Real<T> b)
            => Implementation.AreEqual(a, b);

        public static bool operator !=(Real<T> a, Real<T> b)
            => !Implementation.AreEqual(a, b);

        public static bool operator <(Real<T> a, Real<T> b)
            => Implementation.IsLessThan(a, b);

        public static bool operator >(Real<T> a, Real<T> b)
            => Implementation.IsGreaterThan(a, b);

        public static bool operator <=(Real<T> a, Real<T> b)
            => Implementation.IsLessOrEqual(a, b);

        public static bool operator >=(Real<T> a, Real<T> b)
            => Implementation.IsGreaterOrEqual(a, b);



        public static Real<T> operator +(Real<T> a, Real<T> b)
            => Implementation.Add(a, b);

        public static Real<T> operator -(Real<T> a, Real<T> b)
            => Implementation.Subtract(a, b);

        public static Real<T> operator *(Real<T> a, Real<T> b)
            => Implementation.Multiply(a, b);

        public static Real<T> operator /(Real<T> a, Real<T> b)
            => Implementation.Multiply(a, b);

        public static Real<T> operator %(Real<T> a, Real<T> b)
            => Implementation.Remainder(a, b);

    }
}
