using System.GenericArithmetics.Arithmetics;

namespace System.GenericArithmetics
{
    public struct Numeric<T>
        : IEquatable<Numeric<T>>
    {
        public readonly T Value;
        public static IArithmetic<T> Arithmetic => Implementation;
        static IArithmetic<T> Implementation;

        static Numeric()
        {
            Numeric<short  >.InitializeArithmetic(default(Arithmetics.Int16));
            Numeric<int    >.InitializeArithmetic(default(Arithmetics.Int32));
            Numeric<long   >.InitializeArithmetic(default(Arithmetics.Int64));
            Numeric<ushort >.InitializeArithmetic(default(Arithmetics.UInt16));
            Numeric<uint   >.InitializeArithmetic(default(Arithmetics.UInt32));
            Numeric<ulong  >.InitializeArithmetic(default(Arithmetics.UInt64));
            Numeric<float  >.InitializeArithmetic(default(Arithmetics.Single));
            Numeric<double >.InitializeArithmetic(default(Arithmetics.Double));
            Numeric<decimal>.InitializeArithmetic(default(Arithmetics.Decimal));

        }

        public Numeric(T value)
        {
            Value = value;
        }

        public static void InitializeArithmetic(IArithmetic<T> implementation)
        {
            if (Implementation != null)
                throw new InvalidOperationException("Arithmetic for type " + typeof(T).FullName + " has already been initialized.");

            if (implementation == null)
                throw new InvalidOperationException("Cannot initalize arithmetic for type " + typeof(T).FullName + " with null.");

            Implementation = implementation;
        }
        
        public static implicit operator Numeric<T>(T value) => new Numeric<T>(value);
        public static implicit operator T(Numeric<T> a) => a.Value;
        
        

        public bool Equals<S>(Numeric<S> other)
            where S : IEquatable<S>, IComparable<S>
            => Value.Equals(other.Value);

        public bool Equals(Numeric<T> other)
            => Value.Equals(other.Value);

        override public bool Equals(object other)
            => other is Numeric<T> num
               ? Value.Equals(num)
               : Value.Equals(other);

        public override int GetHashCode()
            => Value.GetHashCode();

        public static bool operator ==(Numeric<T> a, Numeric<T> b)
            => Implementation.AreEqual(a, b);

        public static bool operator !=(Numeric<T> a, Numeric<T> b)
            => !Implementation.AreEqual(a, b);



        public static Numeric<T> operator +(Numeric<T> a, Numeric<T> b)
            => Implementation.Add(a, b);

        public static Numeric<T> operator -(Numeric<T> a, Numeric<T> b)
            => Implementation.Subtract(a, b);

        public static Numeric<T> operator *(Numeric<T> a, Numeric<T> b)
            => Implementation.Multiply(a, b);

        public static Numeric<T> operator /(Numeric<T> a, Numeric<T> b)
            => Implementation.Multiply(a, b);
        
    }
}
