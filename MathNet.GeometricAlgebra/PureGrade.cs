using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Extensions;
using MathNet.Numerics.LinearAlgebra;
using static MathNet.Numerics.Combinatorics;
using static MathNet.Extensions.Ergonomy;

namespace MathNet.GeometricAlgebra
{
    public class PureGrade : Multivector
    {
        public uint Grade { get; }

        public PureGrade(Space space, uint grade, IList<double> elements) : base()
        {
            var dim = (int)space.Dimension;
            if (grade > dim)
                throw new ArgumentException("Grade cannot be larger that the dimension of the space.");
            
            Grade = grade;

            var count = Combinations(dim, (int)grade);

            if (count != elements.Count)
                throw new ArgumentException("The element storage of the pure grade has to be of length " + count + ".");

            _Space = space;
            _Elements = elements;
        }

        public PureGrade(Space space, uint grade)
            : this(space, grade, new double[(int)Combinations((int)space.Dimension, (int)grade)]) { }


        public PureGrade(Space space, Vector<double> vector)
            : this(space, 1, vector.ToArray())
        {
            if(vector.Count != space.Dimension)
                throw new ArgumentException("The dimension of LinearAlgebra.Vector is different that the dimension of the space");
        }

        public static explicit operator Vector<double>(PureGrade self)
            => Vector<double>.Build.Dense(self.Elements.ToArray());


        public PureGrade Copy(IList<double> array)
        {
            var l = Elements.Count;

            if (array.Count != l)
                throw new ArgumentException("The length of the array has to be equal to the number of PureGrade's elements.");

            for (int i = 0; i < l; i++)
            {
                Elements[i] = array[i];
            }

            return this;
        }


        override public double this[int index]
        {
            get
            {
                var (grade, num) = Space.BladeBasisFromIndex((uint)index);
                if (grade != Grade) return 0;
                else return Elements[(int)num];
            }

            set
            {
                var (grade, num) = Space.BladeBasisFromIndex((uint)index);
                if (grade != Grade)
                {
                    if (!value.ApproximatelyEquals(0))
                        throw new IndexOutOfRangeException("Cannot set grade " + grade + " of a PureGrade of grade " + Grade
                            + ". Maybe you did an operation on two PureGrades of a different grade.");
                }
                else Elements[(int)num] = value;
            }
        }


        public double L2Norm
        {
            get
            {
                double sum = 0;
                for (int i = 0; i < Elements.Count; i++)
                    sum += Sq(Elements[i]);

                return Math.Sqrt(sum);
            }
        }



        // Retyping Multivector methods to return PureGrade

        public PureGrade(PureGrade M) : this(M.Space, M.Grade) => Copy(M);
        new public PureGrade Clone() => new PureGrade(Space, Grade).Copy(this);
        public static PureGrade Clone(PureGrade M) => M.Clone();
        public PureGrade Copy(PureGrade b) => (PureGrade)base.Copy(b);

        
        
        new public PureGrade Negate()
        {
            for (int i = 0, l = Elements.Count; i < l; i++)
            {
                Elements[i] = -Elements[i];
            }
            return this;
        }

        public static PureGrade operator -(PureGrade a)
            => a.Clone().Negate();

        
        new public PureGrade Mul(double s) => Multiply(s);
        new public PureGrade Multiply(double s)
        {
            for (int i = 0, l = Elements.Count; i < l; i++)
            {
                this[i] *= s;
            }
            return this;

        }

        public static PureGrade operator *(double a, PureGrade b)
            => b.Clone().Mul(a);

        public static PureGrade operator *(PureGrade a, double b)
            => a.Clone().Mul(b);

        public static PureGrade operator /(PureGrade a, double b)
            => a * (1 / b);


        new public PureGrade Involute() => (PureGrade)base.Involute();

        public static PureGrade Involute(PureGrade M)
            => M.Clone().Involute();


        new public PureGrade Reverse() => (PureGrade)base.Involute();

        public static PureGrade Reverse(PureGrade M)
            => M.Clone().Reverse();

        ///<summary>Returns the reverse of M</summary>
        public static PureGrade operator ~(PureGrade M)
            => M.Clone().Reverse();


        new public PureGrade Conjugate() => (PureGrade)base.Conjugate();

        public static PureGrade Conjugate(PureGrade M)
            => M.Clone().Conjugate();

    }
}
