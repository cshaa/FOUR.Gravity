using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

using static MathNet.GeometricAlgebra.Constants.Basis;
using static System.Math;

namespace MathNet.GeometricAlgebra.UnitTests
{
    [TestFixture]
    public class MatrixTests
    {
        Space R2 = new Space(2);
        Space R3 = new Space(3);
        Space R4 = new Space(4);

        Dictionary<string, Matrix> M = new Dictionary<string, Matrix>();
        Dictionary<string, Vector> v = new Dictionary<string, Vector>();

        [OneTimeSetUp]
        public void SetUp()
        {
            M["A2x2"] = new Matrix(R2, new double[,]
            {
                { 0, 1 },
                { 1, 0 }
            });

            M["B2x2"] = new Matrix(R2, new double[,]
            {
                { 2, -3 },
                { 1, 0 }
            });

            M["Proj3x2"] = new Matrix(R2, R3, new double[,]
            {
                { 1, 0 },
                { 0, 1 },
                { 2, 3 }
            });

            M["Id3x3"] = new Matrix(R3, new double[,]
            {
                { 1, 0, 0 },
                { 0, 1, 0 },
                { 0, 0, 1 }
            });

            M["A3x3"] = new Matrix(R3, new double[,]
            {
                { 2, 3, 0 },
                { 1, -1, 2 },
                { 2, 3, 1 }
            });
        }



        [Test]
        public void MatrixConstructor()
        {
            var A = new Matrix(R3, R4, new double[,]
            {
                { 5, 8 ,3 },
                { 6, 2, -1 },
                { PI, 5, 8 },
                { 0, 1/13d, 0 }
            });

            Assert.AreEqual(5, A[0,0]);
            Assert.AreEqual(PI, A[2,0]);
            Assert.AreEqual(1 / 13d, A[3,1]);

#pragma warning disable CS1718 // Comparison made to same variable

            Assert.IsTrue(A == A);
            Assert.IsTrue(A.Equals(A));
            Assert.IsFalse(A != A);

#pragma warning restore CS1718 // Comparison made to same variable
        }
    }
}
