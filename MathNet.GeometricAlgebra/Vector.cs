using System;
using System.Collections.Generic;

namespace MathNet.GeometricAlgebra
{
    public class Vector : PureGrade
    {

        public Vector(Vector V) : base(V) { }
        public Vector(Space space) : base(space, 1) { }
        public Vector(Space space, IList<double> elements) : base(space, 1, elements) { }
        public Vector(Space space, Numerics.LinearAlgebra.Vector<double> vector) : base(space, vector) { }

        new public Vector Clone() => new Vector(Space).Copy(this);
        public static Vector Clone(Vector M) => M.Clone();
        public Vector Copy(Vector a) => (Vector)base.Copy(a);
        new public Vector Copy(IList<double> a) => (Vector)base.Copy(a);

        public Vector Copy(Numerics.LinearAlgebra.Vector<double> vector)
        {
            var l = Elements.Count;

            if (vector.Count != l)
                throw new ArgumentException("The dimension of LinearAlgebra.Vector is different that the dimension of the space");

            Copy(vector.AsArray());

            return this;
        }
        


        public double Dot(Vector V)
        {
            var l = Elements.Count;
            double result = 0;

            for(int i = 0; i < l; i++)
            {
                result += this.Elements[i] * V.Elements[i];
            }

            return result;
        }

        public double InnerProduct(Vector V) => Dot(V);
        public static double Dot(Vector A, Vector B) => A.Dot(B);
        public static double InnerProduct(Vector A, Vector B) => A.Dot(B);



        // Retyping PureGrade methods to return Vector


        public Vector Add(Vector M)
        {
            if (Space != M.Space) throw new ArgumentException("Cannot subtract two multivectors from different spaces.");

            for (int i = 0, l = Elements.Count; i < l; i++)
            {
                Elements[i] += M.Elements[i];
            }

            return this;
        }

        public static Vector Add(Vector a, Vector b)
            => a.Clone().Add(b);


        public static Vector operator +(Vector a, Vector b) => Add(a, b);


        public Vector Sub(Vector M) => Subtract(M);
        public Vector Subtract(Vector M)
        {
            if (Space != M.Space) throw new ArgumentException("Cannot subtract two multivectors from different spaces.");

            for (int i = 0, l = Elements.Count; i < l; i++)
            {
                Elements[i] -= M.Elements[i];
            }

            return this;
        }


        public static Vector Subtract(Vector a, Vector b)
            => a.Clone().Sub(b);

        public static Vector Sub(Vector a, Vector b) => Subtract(a, b);

        public static Vector operator -(Vector a, Vector b) => Subtract(a, b);


        new public Vector Negate()
        {
            for (int i = 0, l = Elements.Count; i < l; i++)
            {
                Elements[i] = -Elements[i];
            }
            return this;
        }

        public static Vector operator -(Vector a)
            => a.Clone().Negate();


        new public Vector Mul(double s) => Multiply(s);
        new public Vector Multiply(double s)
        {
            for (int i = 0, l = Elements.Count; i < l; i++)
            {
                Elements[i] *= s;
            }
            return this;
        }

        public static Vector operator *(double a, Vector b)
            => b.Clone().Mul(a);

        public static Vector operator *(Vector a, double b)
            => a.Clone().Mul(b);

        public static Vector operator /(Vector a, double b)
            => a * (1 / b);


        new public Vector Involute() => (Vector)base.Involute();

        public static Vector Involute(Vector M)
            => M.Clone().Involute();


        new public Vector Reverse() => (Vector)base.Involute();

        public static Vector Reverse(Vector M)
            => M.Clone().Reverse();

        ///<summary>Returns the reverse of M</summary>
        public static Vector operator ~(Vector M)
            => M.Clone().Reverse();


        new public Vector Conjugate() => (Vector)base.Conjugate();

        public static Vector Conjugate(Vector M)
            => M.Clone().Conjugate();



        /// <summary>Pre-multiplies this vector by matrix M and saves the result to this vector.</summary>
        /// <param name="M">A square matrix in which both source space and target space are equal to the space of this vector.</param>
        /// <returns>Returns this for chaining.</returns>
        public Vector Transform(Matrix M)
        {
            if (Space != M.SourceSpace || Space != M.TargetSpace)
                throw new ArgumentException("The matrix has to be an endomorphism of the vector space of this vector.");

            return Copy(M * this);
        }
    }
}
