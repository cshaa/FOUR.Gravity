using System.GenericArithmetics.Arithmetics;

namespace System.GenericArithmetics
{
    public struct OrderedNumeric<T>
        : IEquatable<OrderedNumeric<T>>,
          IComparable<OrderedNumeric<T>>
    {
        public readonly T Value;
        public static IOrderedArithmetic<T> Arithmetic => Implementation;
        static IOrderedArithmetic<T> Implementation;

        static OrderedNumeric()
        {
            OrderedNumeric<short  >.InitializeArithmetic(default(Arithmetics.Int16));
            OrderedNumeric<int    >.InitializeArithmetic(default(Arithmetics.Int32));
            OrderedNumeric<long   >.InitializeArithmetic(default(Arithmetics.Int64));
            OrderedNumeric<ushort >.InitializeArithmetic(default(Arithmetics.UInt16));
            OrderedNumeric<uint   >.InitializeArithmetic(default(Arithmetics.UInt32));
            OrderedNumeric<ulong  >.InitializeArithmetic(default(Arithmetics.UInt64));
            OrderedNumeric<float  >.InitializeArithmetic(default(Arithmetics.Single));
            OrderedNumeric<double >.InitializeArithmetic(default(Arithmetics.Double));
            OrderedNumeric<decimal>.InitializeArithmetic(default(Arithmetics.Decimal));

        }

        public OrderedNumeric(T value)
        {
            Value = value;
        }

        public static void InitializeArithmetic(IOrderedArithmetic<T> implementation)
        {
            if (Implementation != null)
                throw new InvalidOperationException("Arithmetic for type " + typeof(T).FullName + " has already been initialized.");

            if (implementation == null)
                throw new InvalidOperationException("Cannot initalize arithmetic for type " + typeof(T).FullName + " with null.");

            Implementation = implementation;
        }
        
        public static implicit operator OrderedNumeric<T>(T value) => new OrderedNumeric<T>(value);
        public static implicit operator T(OrderedNumeric<T> a) => a.Value;

        
        public OrderedNumeric<T> Next => Implementation.Next(Value);
        public OrderedNumeric<T> Previous => Implementation.Previous(Value);



        public int CompareTo(OrderedNumeric<T> other)
            => Implementation.Compare(Value, other.Value);

        public bool Equals<S>(OrderedNumeric<S> other)
            where S : IEquatable<S>, IComparable<S>
            => Value.Equals(other.Value);

        public bool Equals(OrderedNumeric<T> other)
            => Value.Equals(other.Value);

        override public bool Equals(object other)
            => other is OrderedNumeric<T> num
               ? Value.Equals(num)
               : Value.Equals(other);

        public override int GetHashCode()
            => Value.GetHashCode();

        public static bool operator ==(OrderedNumeric<T> a, OrderedNumeric<T> b)
            => Implementation.AreEqual(a, b);

        public static bool operator !=(OrderedNumeric<T> a, OrderedNumeric<T> b)
            => !Implementation.AreEqual(a, b);

        public static bool operator <(OrderedNumeric<T> a, OrderedNumeric<T> b)
            => Implementation.IsLessThan(a, b);

        public static bool operator >(OrderedNumeric<T> a, OrderedNumeric<T> b)
            => Implementation.IsGreaterThan(a, b);

        public static bool operator <=(OrderedNumeric<T> a, OrderedNumeric<T> b)
            => Implementation.IsLessOrEqual(a, b);

        public static bool operator >=(OrderedNumeric<T> a, OrderedNumeric<T> b)
            => Implementation.IsGreaterOrEqual(a, b);



        public static Numeric<T> operator +(OrderedNumeric<T> a, OrderedNumeric<T> b)
            => Implementation.Add(a, b);

        public static Numeric<T> operator -(OrderedNumeric<T> a, OrderedNumeric<T> b)
            => Implementation.Subtract(a, b);

        public static Numeric<T> operator *(OrderedNumeric<T> a, OrderedNumeric<T> b)
            => Implementation.Multiply(a, b);

        public static Numeric<T> operator /(OrderedNumeric<T> a, OrderedNumeric<T> b)
            => Implementation.Multiply(a, b);

        public static Numeric<T> operator %(OrderedNumeric<T> a, OrderedNumeric<T> b)
            => Implementation.Remainder(a, b);
        
    }
}
