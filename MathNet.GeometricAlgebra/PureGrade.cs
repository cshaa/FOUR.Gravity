using System;
using System.Collections.Generic;
using System.Text;
using static MathNet.Numerics.Combinatorics;

namespace MathNet.GeometricAlgebra
{
    public class PureGrade
    {
        public double[] Elements { get; }
        public Space Space { get; }
        public uint Grade { get; }

        public PureGrade(Space space, uint grade)
        {
            var dim = (int)space.Dimension;
            if (grade > dim) throw new ArgumentException("Grade cannot be larger that the dimension of the space.");

            Space = space;
            Grade = grade;
            Elements = new double[(long)Combinations(dim, (int)grade)];
        }
    }
}
