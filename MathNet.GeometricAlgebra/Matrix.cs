using MathNet.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace MathNet.GeometricAlgebra
{
    public class Matrix : IApproximatelyEquatable<Matrix>
    {
        public double[,] Elements => _Elements;
        protected double[,] _Elements;

        public Space SourceSpace => _SourceSpace;
        protected Space _SourceSpace;

        public Space TargetSpace => _TargetSpace;
        protected Space _TargetSpace;



        public Matrix(Space source, Space target, double[,] elements)
        {
            if (elements.GetLength(0) != target.Dimension || elements.GetLength(1) != source.Dimension)
                throw new ArgumentException("The length of elements has to be m×n, where m is the dimension of target and n the dimension of source.");

            _SourceSpace = source;
            _TargetSpace = target;
            _Elements = elements;

            Rows = new MatrixRows(this);
            Columns = new MatrixColumns(this);
        }

        public Matrix(Space source, Space target)
            : this(source, target, new double[target.Dimension, source.Dimension]) { }

        public Matrix(Space space, double[,] elements)
            : this(space, space, elements) { }

        public Matrix(Space space)
            : this(space, space) { }



        public double this[int row, int col]
        {
            get => Elements[row, col];
            set => Elements[row, col] = value;
        }


        public Matrix Copy(Matrix M)
        {
            if (SourceSpace != M.SourceSpace || TargetSpace != M.TargetSpace)
                throw new ArgumentException("To copy from a matrix, it has to be a map between the same spaces as this one.");

            for (int row = 0; row<RowCount; row++)
            for (int col = 0; col<ColumnCount; col++)
            {
                this[row, col] = M[row, col];
            }

            return this;
        }
        
        public Matrix(Matrix M) : this(M.SourceSpace, M.TargetSpace) => Copy(M);
        public Matrix Clone() => new Matrix(SourceSpace, TargetSpace).Copy(this);
        public static Matrix Clone(Matrix M) => M.Clone();





        // Equals

        /// <summary>
        /// Compares the values of this Multivector with those of M.
        /// </summary>
        /// <param name="M">The other Multivector</param>
        /// <returns>True if the Multivectors are from the space and their coordinates are identical, false otherwise.</returns>
        public bool Equals(Matrix other)
        {
            if (other == null) return false;
            if (SourceSpace != other.SourceSpace) return false;
            if (TargetSpace != other.TargetSpace) return false;
            
            for (int row = 0; row < RowCount; row++)
            for (int col = 0; col < ColumnCount; col++)
            {
                if (this[row, col] != other[row, col]) return false;
            }

            return true;
        }



        override public bool Equals(object M) => Equals(M as Matrix);

        public static bool operator ==(Matrix M, Matrix N)
        {
            // cast to use the operator ==(object, object) and avoid recursion
            if (((object)M) == null || ((object)N) == null)
                return Equals(M, N);

            return M.Equals(N);
        }

        public static bool operator ==(Matrix M, object N) => M == N as Matrix;
        public static bool operator ==(object M, Matrix N) => M as Matrix == N;

        public static bool operator !=(Matrix M, Matrix N)
        {
            // cast to use the operator !=(object, object) and avoid recursion
            if (((object)M) == null || ((object)N) == null)
                return !Equals(M, N);

            return !M.Equals(N);
        }


        public static bool operator !=(Matrix M, object N) => M != N as Matrix;
        public static bool operator !=(object M, Matrix N) => M as Matrix != N;

        override public int GetHashCode()
        {
            unchecked
            {
                var hash = SourceSpace.GetHashCode() * 29 + TargetSpace.GetHashCode();
                foreach (var e in Elements) hash = hash * 29 + e.GetHashCode();
                return hash;
            }
        }


        public bool ApproximatelyEquals(Matrix other, double? delta = null, double? relativeDelta = null, long? ulpsApart = null)
        {
            if (other == null) return false;
            if (SourceSpace != other.SourceSpace) return false;
            if (TargetSpace != other.TargetSpace) return false;

            for (int row = 0; row < RowCount; row++)
            for (int col = 0; col < ColumnCount; col++)
            {
                if (!this[row, col].ApproximatelyEquals(other[row, col], delta, relativeDelta, ulpsApart))
                    return false;
            }

            return true;
        }





        /// <summary>Returns the number of rows, equal to the dimension of the target space.</summary>
        public uint RowCount => TargetSpace.Dimension;

        /// <summary>Returns the number of columns, equal to the dimension of the source space.</summary>
        public uint ColumnCount => SourceSpace.Dimension;

        public readonly MatrixRows Rows;
        public readonly MatrixColumns Columns;


        public class MatrixRows : IEnumerable<Vector>
        {
            Matrix Self;
            public MatrixRows(Matrix self) => Self = self;

            public double[] AsArray(int row)
            {
                var l = Self.ColumnCount;
                var arr = new double[l];
                for (var col = 0; col < l; col++) arr[col] = Self[row, col];
                return arr;
            }

            public void FromArray(int row, IList<double> arr)
            {
                var l = Self.ColumnCount;
                if (arr.Count != l) throw new ArgumentException("Array passed to Rows.FromArray was of length "+arr.Count+" but the matrix has "+l+" columns.");
                for (var col = 0; col < l; col++) Self[row, col] = arr[col];
            }

            public IEnumerator<Vector> GetEnumerator()
            {
                for (var row = 0; row < Self.RowCount; row++) yield return this[row];
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public Vector this[int row]
            {
                get => new Vector(Self.SourceSpace, AsArray(row));
                set
                {
                    if (value.Space == Self.SourceSpace) FromArray(row, value.Elements);
                    else throw new ArgumentException("Vector passed to Rows[i] has to be from the source space of the matrix.");
                }
            }
        }

        public class MatrixColumns : IEnumerable<Vector>
        {
            Matrix Self;
            public MatrixColumns(Matrix self) => Self = self;

            public double[] AsArray(int col)
            {
                var l = Self.RowCount;
                var arr = new double[l];
                for (var row = 0; row < l; row++) arr[row] = Self[row, col];
                return arr;
            }

            public void FromArray(int col, IList<double> arr)
            {
                var l = Self.RowCount;
                if (arr.Count != l) throw new ArgumentException("Array passed to Columns.FromArray was of length " + arr.Count + " but the matrix has " + l + " rows.");
                for (var row = 0; row < l; row++) Self[row, col] = arr[row];
            }

            public IEnumerator<Vector> GetEnumerator()
            {
                for (var col = 0; col < Self.ColumnCount; col++) yield return this[col];
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public Vector this[int col]
            {
                get => new Vector(Self.SourceSpace, AsArray(col));
                set
                {
                    if (value.Space == Self.TargetSpace) FromArray(col, value.Elements);
                    else throw new ArgumentException("Vector passed to Columns[i] has to be from the target space of the matrix.");
                }
            }
        }




        public Matrix Add(Matrix M)
        {
            if (SourceSpace != M.SourceSpace || TargetSpace != M.TargetSpace)
                throw new ArgumentException("To add matrices, they have to be maps between the same spaces.");

            for(int row = 0; row < RowCount; row++)
            for(int col = 0; col < ColumnCount; col++)
            {
                this[row, col] += M[row, col];
            }

            return this;
        }

        public static Matrix Add(Matrix M, Matrix N) => M.Clone().Add(N);
        public static Matrix operator +(Matrix M, Matrix N) => Add(M, N);


        public Matrix Sub(Matrix M) => Subtract(M);
        public Matrix Subtract(Matrix M)
        {
            if (SourceSpace != M.SourceSpace || TargetSpace != M.TargetSpace)
                throw new ArgumentException("To subtract matrices, they have to be maps between the same spaces.");

            for (int row = 0; row < RowCount; row++)
            for (int col = 0; col < ColumnCount; col++)
            {
                this[row, col] += M[row, col];
            }

            return this;
        }

        public static Matrix Subtract(Matrix M, Matrix N) => M.Clone().Sub(N);
        public static Matrix operator -(Matrix M, Matrix N) => Subtract(M, N);


        public Matrix Negate()
        {
            for (int row = 0; row < RowCount; row++)
            for (int col = 0; col < ColumnCount; col++)
            {
                this[row, col] = -this[row, col];
            }
            return this;
        }

        public static Matrix Negate(Matrix M) => M.Clone().Negate();
        public static Matrix operator -(Matrix M) => Negate(M);



        /// <summary>
        /// Returns the product of this matrix and vector v.
        /// <para>
        ///     Beware! Unlike this.Multiply() in other types, matrix.Multiply() does not affect the matrix!
        ///     Instead it returns a new vector. That means it is equivalent to the static method.
        /// </para>
        /// </summary>
        /// <param name="v">A vector that will be pre-multiplied by this matrix.</param>
        /// <returns>Returns the image of vector v.</returns>
        public Vector Multiply(Vector v)
        {
            if (v.Space != SourceSpace) throw new ArgumentException("The vector you want to pre-multiply has to be an element of the source space.");

            var w = new Vector(TargetSpace);

            for(int row = 0; row < RowCount; row++)
            for(int col = 0; col < ColumnCount; col++)
            {
                w[row] += this[row, col] * v[col];
            }

            return w;
        }

        public static Vector Multiply(Matrix M, Vector v) => M.Multiply(v);
        public static Vector operator *(Matrix M, Vector v) => M.Multiply(v);



        /// <summary>
        /// Returns the product of this matrix and the other one.
        /// <para>
        ///     Beware! Unlike this.Multiply() in other types, matrix.Multiply() does not affect the matrix!
        ///     Instead it returns a new vector. That means it is equivalent to the static method.
        /// </para>
        /// </summary>
        /// <param name="M">A matrix that will be pre-multiplied by this one.</param>
        /// <returns>Returns composition of maps represented by this matrix and M.</returns>
        public Matrix Multiply(Matrix M)
        {
            if (SourceSpace != M.TargetSpace)
                throw new ArgumentException("To multiply two matrices, the source space of the left one has to be equal to the target space of the right one.");

            var N = new Matrix(M.SourceSpace, TargetSpace);

            for(int row = 0; row < RowCount; row++)
            for(int col = 0; col < M.ColumnCount; col++)
            for(int i = 0; i < ColumnCount; i++)
            {
                N[row, col] += this[row, i] * M[i, col];
            }

            return N;
        }

        public static Matrix Multiply(Matrix M, Matrix N) => M.Multiply(N);
        
        public static Matrix operator *(Matrix M, Matrix N) => M.Multiply(N);

    }
}
