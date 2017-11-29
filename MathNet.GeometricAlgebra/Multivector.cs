using System;

namespace MathNet.GeometricAlgebra
{
    

    public class Multivector
    {
        public double[] Elements { get; }
        Space Space;

        // Constructor, Copy & Clone 

        public Multivector(Space space)
        {
            Space = space;
            Elements = new double[1ul<<(int)Space.Dimension];
        }

        double ScalarPart
        {
            get => Elements[0];
            set => Elements[0] = value;
        }

        public Multivector Copy(Multivector M)
        {
            if (Space != M.Space) throw new ArgumentException("Cannot copy a multivector from a different space.");

            var Me = M.Elements;
            for (long i = 0, l = Elements.LongLength; i < l; i++)
            {
                Elements[i] = Me[i];
            }

            return this;
        }

        public Multivector(Multivector M) : this(M.Space) => Copy(M);
        public Multivector Clone() => new Multivector(Space).Copy(this);
        public static Multivector Clone(Multivector M) => M.Clone();





        // Add

        public Multivector Add(double d)
        {
            Elements[0] += d;
            return this;
        }

        public Multivector Add(Multivector M)
        {
            if (Space != M.Space) throw new ArgumentException("Cannot subtract two multivectors from different spaces.");

            var Me = M.Elements;
            for (long i = 0, l = Elements.LongLength; i < l; i++)
            {
                Elements[i] += Me[i];
            }

            return this;
        }

        public static Multivector Add(Multivector a, Multivector b)
            => new Multivector(a).Add(b);

        public static Multivector Add(double a, Multivector b)
            => new Multivector(b).Add(a);

        public static Multivector Add(Multivector a, double b)
            => new Multivector(a).Add(b);

        public static Multivector operator +(Multivector a, Multivector b) => Add(a,b);
        public static Multivector operator +(double a, Multivector b) => Add(a, b);
        public static Multivector operator +(Multivector a, double b) => Add(a, b);





        // Subtract

        public Multivector Sub(double d) => Subtract(d);
        public Multivector Subtract(double d) => Add(-d);

        public Multivector Sub(Multivector M) => Subtract(M);
        public Multivector Subtract(Multivector M)
        {
            if (Space != M.Space) throw new ArgumentException("Cannot subtract two multivectors from different spaces.");

            var Me = M.Elements;
            for (long i = 0, l = Elements.LongLength; i < l; i++)
            {
                Elements[i] -= Me[i];
            }

            return this;
        }



        public static Multivector Subtract(Multivector a, Multivector b)
            => new Multivector(a).Sub(b);

        public static Multivector Subtract(double a, Multivector b)
            => new Multivector(b.Space).Add(a).Sub(b);

        public static Multivector Subtract(Multivector a, double b)
            => new Multivector(a).Sub(b);

        public static Multivector Sub(Multivector a, Multivector b) => Subtract(a, b);
        public static Multivector Sub(double a, Multivector b) => Subtract(a, b);
        public static Multivector Sub(Multivector a, double b) => Subtract(a, b);
        public static Multivector operator -(Multivector a, Multivector b) => Subtract(a,b);
        public static Multivector operator -(double a, Multivector b) => Subtract(a, b);
        public static Multivector operator -(Multivector a, double b) => Subtract(a, b);





        // Negate

        public Multivector Negate()
        {
            for (long i = 0, l = Elements.LongLength; i < l; i++)
            {
                Elements[i] = -Elements[i];
            }
            return this;
        }

        public static Multivector operator -(Multivector a)
            => new Multivector(a).Negate();





        // Multiply

        public Multivector Mul(double s) => Multiply(s);
        public Multivector Multiply(double s)
        {
            for(long i = 0, l = Elements.LongLength; i < l; i++)
            {
                Elements[i] *= s;
            }
            return this;

        }

        public static Multivector operator *(double a, Multivector b)
            => new Multivector(b).Mul(a);

        public static Multivector operator *(Multivector a, double b)
            => new Multivector(a).Mul(b);


        /// <summary>Multiply two multivectors a & b and write the result to c.</summary>
        /// <param name="c">Will be overwritten with the result of multiplication.</param>
        /// <returns>Returns c for chaining.</returns>
        public static Multivector Multiply(Multivector a, Multivector b, ref Multivector c)
        {
            if (a.Space != b.Space || a.Space != c.Space) throw new ArgumentException("Cannot multiply two multivectors from different spaces.");

            var space = a.Space;

            double[] aE = a.Elements,
                     bE = b.Elements,
                     cE = c.Elements;

            var l = a.Elements.LongLength;

            for (long i = 0; i < l; i++)
            for (long j = 0; j < l; j++)
            {
                    ulong e = (ulong)i, f = (ulong)j;
                    int sign = 1;
                    while (f!=0)
                    {
                        var lsb = Binary.LeastSignificantBit(f);
                        if ((e >> lsb + 1) % 2 != 0) sign = -sign;
                        sign*=space.BasisSquared((uint)lsb);

                        e ^= 1ul << lsb;
                        f &= f - 1;
                    }
                    cE[e] = sign * aE[i] * bE[j];
            }

            return c;
        }

        public static Multivector Mul(Multivector a, Multivector b) => Multiply(a, b);
        public static Multivector Multiply(Multivector a, Multivector b)
        {
            var c = new Multivector(a.Space);
            return Multiply(a, b, ref c);
        }

        public Multivector Mul(Multivector M) => Multiply(M);
        public Multivector Multiply(Multivector M)
        {
            return Copy(Multiply(this, M));
        }


        public static Multivector operator *(Multivector a, Multivector b)
            => new Multivector(a.Space).Add(a).Mul(b);

    }
}
