using MathNet.Extensions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using static MathNet.Numerics.Combinatorics;
using MathNet.Numerics.LinearAlgebra;

namespace MathNet.GeometricAlgebra
{
    

    public class Multivector : IApproximatelyEquatable<Multivector>, IFormattable
    {
        /// <summary>List of elements. Try to avoid using it thoughtlessly, as its
        /// behaviour may be overriden by inheriting classes.</summary>
        public IList<double> Elements { get => _Elements; }
        protected IList<double> _Elements;

        public Space Space { get => _Space; }
        protected Space _Space;





        // Basic OOP methods


        protected Multivector() { }

        /**
         * <summary>Creates new multivector in the Clifford algebra <paramref name="space"/>.</summary>
         * <param name="space">The Clifford algebra this multivector is an element of.</param>
         */
        public Multivector(Space space)
            : this(space, new double[1ul << (int)space.Dimension]) { }

        /// <see cref="Multivector(Space)"/>
        public static Multivector Create(Space space) => new Multivector(space);
        
        


        /** <summary>
         *      Creates new multivector in Clifford algebra <paramref name="space"/>
         *      whose elements will be stored in <paramref name="elements"/>.
         *  </summary>
         *  <param name="space">The Clifford algebra this multivector will be an element of.</param>
         *  <param name="elements">List that will be used to store the elements of the multivector.</param>
         */
        public Multivector(Space space, IList<double> elements)
        {
            var l = 1 << (int)space.Dimension;

            if (elements.Count != l)
                throw new ArgumentException("The multivector's element storage has to be of length "+l+".");

            _Space = space;
            _Elements = elements;
        }

        /// <see cref="Multivector(Space, IList{Double})"/>
        public static Multivector Create(Space space, IList<double> elements)
            => new Multivector(space, elements);



        /// <summary>Returns/sets the scalar part (the grade zero part) of the multivector.</summary>
        public double ScalarPart
        {
            get => this[0];
            set => this[0] = value;
        }
        


        /// <summary>
        /// Returns/sets the vector part (the grade one part) of the multivector.
        /// The resulting <see cref="Vector"/> is a copy of the elements, not a reference.
        /// </summary>
        public Vector VectorPart
        {
            get => (Vector)GetGrade(1);

            set
            {
                if (Space != value.Space)
                    throw new ArgumentException("The vector is from a different space than this multivector");

                var l = value.Elements.Count;

                for (uint i = 0; i < l; i++)
                {
                    this[(int)Space.IndexFromBladeBasis(1, i)] = value.Elements[(int)i];
                }
            }
        }



        /// <summary>
        /// Get the part with the given index. Indices have to be integers
        /// less than or equal to 2^dimension. Integers with one bit 1 and
        /// other bits 0 correspond to vector coordinates, integers with
        /// 2 bits set to 1 correspond to bivectors, those with 3 correspond
        /// to trivectors and so on. The basis vectors are numbered from the
        /// least significant bit up, that means 0b1 is e1, 0b10 is e2 etc.
        /// Basis multivectors of higher grade are represented as a product
        /// of basis vectors and the bits in their index mark which ones.
        /// That is, 0b011 is e1e2, 0b101 is e1e3 and 0b111 is e1e2e3.
        /// Index 0 represents scalars.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual double this[int index]
        {
            get => Elements[index];
            set => Elements[index] = value;
        }


        /// <summary>The largest number passed to <see cref="this[int]"/> should be less than `this.Count`.</summary>
        public int Count => 1 << (int)Space.Dimension;




        /// <summary>Copy elements from M to this multivector.</summary>
        /// <param name="M">The multivector to copy.</param>
        /// <returns>Returns `this` for chaining.</returns>
        public Multivector Copy(Multivector M)
        {
            if (Space != M.Space) throw new ArgumentException("Cannot copy a multivector from a different space.");
            
            for (int i = 0, l = Count; i < l; i++)
            {
                this[i] = M[i];
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

                if (this[index] == 0) continue;

                if (beginning)
                {
                    beginning = false;
                }
                else
                {
                    str.Append(" + ");
                }
                
                str.Append(this[index].ToString(format, formatProvider));

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





        // Grade

        public PureGrade GetGrade(uint k)
        {
            var pureGrade = k == 1
                ? new Vector(Space)
                : new PureGrade(Space, k);

            var l = pureGrade.Elements.Count;

            for (uint i = 0; i < l; i++)
            {
                pureGrade.Elements[(int)i] = this[(int)Space.IndexFromBladeBasis(k, i)];
            }

            return pureGrade;
        }

        public Multivector SetGrade(PureGrade value)
        {
            if (Space != value.Space) throw new ArgumentException("Cannot set grade from a different space than the original multivector.");

            var k = value.Grade;
            var l = value.Elements.Count;
            for (uint i = 0; i < l; i++)
            {
                this[(int)Space.IndexFromBladeBasis(k, i)] = value.Elements[(int)i];
            }

            return this;
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

            for (int i = 0; i < Count; i++)
                if (this[i] != other[i]) return false;

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
            // cast to use the operator ==(object, object) and avoid recursion
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

            for (int i = 0; i < Count; i++)
                if (!this[i].ApproximatelyEquals(other[i], delta, relativeDelta, ulpsApart)) return false;

            return true;
        }





        // Add

        public Multivector Add(double d)
        {
            this[0] += d;
            return this;
        }

        public Multivector Add(Multivector M)
        {
            if (Space != M.Space) throw new ArgumentException("Cannot subtract two multivectors from different spaces.");
            
            for (int i = 0, l = Count; i < l; i++)
            {
                this[i] += M[i];
            }

            return this;
        }

        public static Multivector Add(Multivector a, Multivector b)
            => a.Clone().Add(b);

        public static Multivector Add(double a, Multivector b)
            => b.Clone().Add(a);

        public static Multivector Add(Multivector a, double b)
            => a.Clone().Add(b);

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
            
            for (int i = 0, l = Count; i < l; i++)
            {
                this[i] -= M[i];
            }

            return this;
        }



        public static Multivector Subtract(Multivector a, Multivector b)
            => a.Clone().Sub(b);

        public static Multivector Subtract(double a, Multivector b)
            => b.Clone().Negate().Add(a);

        public static Multivector Subtract(Multivector a, double b)
            => a.Clone().Sub(b);

        public static Multivector Sub(Multivector a, Multivector b) => Subtract(a, b);
        public static Multivector Sub(double a, Multivector b) => Subtract(a, b);
        public static Multivector Sub(Multivector a, double b) => Subtract(a, b);
        public static Multivector operator -(Multivector a, Multivector b) => Subtract(a,b);
        public static Multivector operator -(double a, Multivector b) => Subtract(a, b);
        public static Multivector operator -(Multivector a, double b) => Subtract(a, b);





        // Negate

        public Multivector Negate()
        {
            for (int i = 0, l = Count; i < l; i++)
            {
                this[i] = -this[i];
            }
            return this;
        }

        public static Multivector Negate(Multivector M) => M.Clone().Negate();
        public static Multivector operator -(Multivector M) => Negate(M);





        // Multiply

        public Multivector Mul(double s) => Multiply(s);
        public Multivector Multiply(double s)
        {
            for(int i = 0, l = Count; i < l; i++)
            {
                this[i] *= s;
            }
            return this;

        }

        public static Multivector operator *(double a, Multivector b)
            => b.Clone().Mul(a);

        public static Multivector operator *(Multivector a, double b)
            => a.Clone().Mul(b);

        public static Multivector operator /(Multivector a, double b)
            => a * (1 / b);


        /// <summary>Multiply two multivectors a and b and add the result to c.</summary>
        /// <param name="c">The result of multiplication will be added to this multivector.</param>
        /// <returns>Returns c for chaining.</returns>
        public static Multivector Multiply(Multivector a, Multivector b, ref Multivector c)
        {
            if (a.Space != b.Space || a.Space != c.Space) throw new ArgumentException("Cannot multiply two multivectors from different spaces.");

            var space = a.Space;

            var l = (ulong)a.Count;

            for (ulong i = 0; i < l; i++)
            for (ulong j = 0; j < l; j++)
            {
                var (e, sign) = space.BasisMultiply(i, j);
                c[(int)e] += sign * a[(int)i] * b[(int)j];
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
            return Multiply(this, M);
        }


        public static Multivector operator *(Multivector a, Multivector b)
            => Multiply(a, b);





        // Exterior product (or wedge product)

        /// <summary>Wedge multiply two multivectors a and b and add the result to c.</summary>
        /// <param name="c">The exterior product of a and b will be added to this multivector.</param>
        /// <returns>Returns c for chaining.</returns>
        public static Multivector Wedge(Multivector a, Multivector b, ref Multivector c)
        {
            if (a.Space != b.Space || a.Space != c.Space) throw new ArgumentException("Cannot wedge multiply two multivectors from different spaces.");

            var space = a.Space;

            var l = (ulong)a.Count;

            for (ulong i = 0; i < l; i++)
            for (ulong j = 0; j < l; j++)
            {
                if ((i & j) != 0) continue; //skip linearly dependent components

                var (e, sign) = space.BasisMultiply(i, j);
                c[(int)e] += sign * a[(int)i] * b[(int)j];
            }


            return c;
        }

        public static Multivector Wedge(Multivector a, Multivector b)
        {
            var c = new Multivector(a.Space);
            return Wedge(a, b, ref c);
        }

        public Multivector Wedge(Multivector M) => Wedge(this, M);

        public static Multivector operator ^(Multivector a, Multivector b)
            => Wedge(a, b);

        
        
        
        
        // Conjugate, involution, reverse
        // https://en.wikipedia.org/wiki/Clifford_algebra#Antiautomorphisms

        
        public Multivector Involute()
        {
            var l = Count;

            for (int i = 0; i < l; i++)
                this[i]
                    = (Space.BladeBasisFromIndex((ulong)i).Grade % 2) == 0
                    ?  this[i]
                    : -this[i];

            return this;

            // Implementations: https://goo.gl/b8eSSY https://goo.gl/fWscjS
            // TODO find papers on this
            // TODO prove
        }

        public static Multivector Involute(Multivector M)
            => (new Multivector(M.Space)).Copy(M).Involute();


        public Multivector Reverse()
        {
            var l = Count;

            for (int i = 0; i < l; i++)
                this[i]
                    = ((Space.BladeBasisFromIndex((ulong)i).Grade / 2) % 2) == 0
                    ?  this[i]
                    : -this[i];

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
            var l = Count;

            for (int i = 0; i < l; i++)
                this[i]
                    = (((Space.BladeBasisFromIndex((ulong)i).Grade - 1) / 2) % 2) == 0
                    ?  this[i]
                    : -this[i];

            return this;

            // Implementations: https://goo.gl/XZqsUT https://goo.gl/KQ1GCv
            // TODO find papers on this
            // TODO prove
        }

        public static Multivector Conjugate(Multivector M)
            => M.Clone().Conjugate();

    }

}
