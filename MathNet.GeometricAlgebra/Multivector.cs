using MathNet.Extensions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using static MathNet.Numerics.Combinatorics;

namespace MathNet.GeometricAlgebra
{
    

    public class Multivector : IApproximatelyEquatable<Multivector>, IFormattable
    {
        protected IList<double> Elements;
        Space Space;





        // Basic OOP methods

        /**
         * <summary>Creates new multivector in the Clifford algebra <paramref name="space"/>.</summary>
         * <param name="space">The Clifford algebra this multivector is an element of.</param>
         */
        public Multivector(Space space)
            : this(space, new double[1ul << (int)space.Dimension]) { }
        

        /** <summary>
         *      Creates new multivector in Clifford algebra <paramref name="space"/>
         *      whose elements will be stored in <paramref name="elements"/>.
         *  </summary>
         *  <param name="space">The Clifford algebra this multivector is an element of.</param>
         *  <param name="elements">List that will be used to store the elements´of the multivector.</param>
         */
        public Multivector(Space space, IList<double> elements)
        {
            Space = space;
            Elements = elements;
        }


        /// <summary>Returns the scalar part (the grade zero part) of the multivector.</summary>
        public double ScalarPart
        {
            get => Elements[0];
            set => Elements[0] = value;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public double this[int index]
        {
            get => Elements[index];
            set => Elements[index] = value;
        }

        public Multivector Copy(Multivector M)
        {
            if (Space != M.Space) throw new ArgumentException("Cannot copy a multivector from a different space.");

            var Me = M.Elements;
            for (int i = 0, l = Elements.Count; i < l; i++)
            {
                Elements[i] = Me[i];
            }

            return this;
        }

        public Multivector(Multivector M) : this(M.Space) => Copy(M);
        public Multivector Clone() => new Multivector(Space).Copy(this);
        public static Multivector Clone(Multivector M) => M.Clone();


        public string ToString(string format, IFormatProvider formatProvider)
        {
            var str = new StringBuilder();
            var beginning = true;

            for (int g = 0; g <= Space.Dimension; g++)
            for (int i = 0; i < Combinations((int)Space.Dimension, g); i++)
            {
                var index = (int)Space.IndexFromBladeBasis((uint)g, (uint)i);

                if (Elements[index] == 0) continue;

                if (beginning)
                {
                    beginning = false;
                }
                else
                {
                    str.Append(" + ");
                }
                
                str.Append(Elements[index].ToString(format, formatProvider));

                if (index != 0) str.Append(" ");

                int j = 1;
                while(index != 0)
                {
                    if ((index & 1 << j-1) != 0)
                    {
                        str.Append("e");
                        str.Append(j);
                        index &= index - 1;
                    }
                    j++;
                }
            }

            if (str.Length == 0) str.Append("0");

            str.Append(" ∈ Space#");
            str.Append(Space.GetHashCode().ToString("x").Substring(0,5));

            return str.ToString();
        }





        // Equals

        /// <summary>
        /// Compares the values of this Multivector with those of M.
        /// </summary>
        /// <param name="M">The other Multivector</param>
        /// <returns>True if the Multivectors are from the space and their coordinates are identical, false otherwise.</returns>
        public bool Equals(Multivector other)
        {
            if (other == null) return false;
            if (Space != other.Space) return false;

            if (Elements is ISparseList<double> A
            && other.Elements is ISparseList<double> B)
            {
                foreach (var i in A.Keys.Union(B.Keys))
                    if (Elements[i] != other.Elements[i]) return false;

                return true;
            }

            for (int i = 0; i < Elements.Count; i++)
                if (Elements[i] != other.Elements[i]) return false;

            return true;
        }



        override public bool Equals(object M) => Equals(M as Multivector);

        public static bool operator ==(Multivector M, Multivector N)
        {
            if ( ((object)M) == null || ((object)N) == null )
                return Equals(M, N);

            return M.Equals(N);
        }

        public static bool operator ==(Multivector M, object N) => M == N as Multivector;
        public static bool operator ==(object M, Multivector N) => M as Multivector == N;

        public static bool operator !=(Multivector M, Multivector N)
        {
            if (((object)M) == null || ((object)N) == null)
                return !Equals(M, N);

            return !M.Equals(N);
        }


        public static bool operator !=(Multivector M, object N) => M != N as Multivector;
        public static bool operator !=(object M, Multivector N) => M as Multivector != N;

        override public int GetHashCode()
        {
            unchecked
            {
                var hash = Space.GetHashCode();
                foreach (var e in Elements) hash = hash * 29 + e.GetHashCode();
                return hash;
            }
        }


        public bool ApproximatelyEquals(Multivector other, double? delta = null, double? relativeDelta = null, long? ulpsApart = null)
        {
            if (other == null) return false;
            if (Space != other.Space) return false;

            if (Elements is ISparseList<double> A
            && other.Elements is ISparseList<double> B)
            {
                foreach (var i in A.Keys.Union(B.Keys))
                    if (!Elements[i].ApproximatelyEquals(other.Elements[i], delta, relativeDelta, ulpsApart)) return false;

                return true;
            }

            for (int i = 0; i < Elements.Count; i++)
                if (!Elements[i].ApproximatelyEquals(other.Elements[i], delta, relativeDelta, ulpsApart)) return false;

            return true;
        }





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
            for (int i = 0, l = Elements.Count; i < l; i++)
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
            for (int i = 0, l = Elements.Count; i < l; i++)
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
            for (int i = 0, l = Elements.Count; i < l; i++)
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
            for(int i = 0, l = Elements.Count; i < l; i++)
            {
                Elements[i] *= s;
            }
            return this;

        }

        public static Multivector operator *(double a, Multivector b)
            => new Multivector(b).Mul(a);

        public static Multivector operator *(Multivector a, double b)
            => new Multivector(a).Mul(b);


        /// <summary>Multiply two multivectors a and b and add the result to c.</summary>
        /// <param name="c">The result of multiplication will be added to this multivector.</param>
        /// <returns>Returns c for chaining.</returns>
        public static Multivector Multiply(Multivector a, Multivector b, ref Multivector c)
        {
            if (a.Space != b.Space || a.Space != c.Space) throw new ArgumentException("Cannot multiply two multivectors from different spaces.");

            var space = a.Space;

            IList<double> aE = a.Elements,
                          bE = b.Elements,
                          cE = c.Elements;

            var l = (ulong)a.Elements.Count;

            for (ulong i = 0; i < l; i++)
            for (ulong j = 0; j < l; j++)
            {
                var (e, sign) = space.BasisMultiply(i, j);
                cE[(int)e] += sign * aE[(int)i] * bE[(int)j];
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





        // Conjugate, involution, reverse
        // https://en.wikipedia.org/wiki/Clifford_algebra#Antiautomorphisms

        
        public Multivector Involute()
        {
            var l = Elements.Count;

            for (int i = 0; i < l; i++)
                Elements[i]
                    = (Space.BladeBasisFromIndex((ulong)i).Grade % 2) == 0
                    ?  Elements[i]
                    : -Elements[i];

            return this;

            // Implementations: https://goo.gl/b8eSSY https://goo.gl/fWscjS
            // TODO find papers on this
            // TODO prove
        }

        public static Multivector Involute(Multivector M)
            => (new Multivector(M.Space)).Copy(M).Involute();


        public Multivector Reverse()
        {
            var l = Elements.Count;

            for (int i = 0; i < l; i++)
                Elements[i]
                    = ((Space.BladeBasisFromIndex((ulong)i).Grade / 2) % 2) == 0
                    ? Elements[i]
                    : -Elements[i];

            return this;

            // Implementations: https://goo.gl/vwZW7E https://goo.gl/Zs8AZP
            // TODO find papers on this
            // TODO prove
        }

        public static Multivector Reverse(Multivector M)
            => M.Clone().Reverse();

        ///<summary>Returns the reverse of M</summary>
        public static Multivector operator ~(Multivector M)
            => M.Clone().Reverse();


        public Multivector Conjugate()
        {
            var l = Elements.Count;

            for (int i = 0; i < l; i++)
                Elements[i]
                    = (((Space.BladeBasisFromIndex((ulong)i).Grade - 1) / 2) % 2) == 0
                    ? Elements[i]
                    : -Elements[i];

            return this;

            // Implementations: https://goo.gl/XZqsUT https://goo.gl/KQ1GCv
            // TODO find papers on this
            // TODO prove
        }

        public static Multivector Conjugate(Multivector M)
            => (new Multivector(M.Space)).Copy(M).Conjugate();

    }

}
