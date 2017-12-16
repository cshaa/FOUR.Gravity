using MathNet.GeometricAlgebra;
using NUnit.Framework;
using System.Collections.Generic;

using static System.Math;
using static MathNet.GeometricAlgebra.Constants.Basis;

namespace MathNet.GeometricAlgebra.UnitTests
{
    [TestFixture]
    public class MultivectorTests
    {

        Space R3 = new Space(3);
        Space R4 = new Space(4);

        //Space R7 = new Space(7);
        //Space R30 = new Space(30);

        Dictionary<string, Multivector> M = new Dictionary<string, Multivector>();

        [OneTimeSetUp]
        public void SetUp()
        {
            M["GeneralMultivector3"] = new Multivector(R3)
            {
                [EScalar] = 2/13,
                [E1] = 5,
                [E2] = -3,
                [E13] = 2*PI,
                [E123] = 42
            };

            M["Vector3A"] = new Multivector(R3)
            {
                [E1] = 4,
                [E2] = 3,
                [E3] = 5
            };

            M["Vector3B"] = new Multivector(R3)
            {
                [E1] = 3.5,
                [E2] = 0.14,
                [E3] = -1/12
            };

            M["Vector3C"] = new Multivector(R3)
            {
                [E2] = 8,
                [E3] = -7
            };

            M["GeneralMultivector4"] = new Multivector(R4)
            {
                [EScalar] = 5/3,
                [E1] = 12,
                [E3] = -45,
                [E14] = 3/5,
                [E23] = -6.2832,
                [E34] = 1,
                [E124] = -3,
                [E134] = 86,
                [E1234] = -5
            };

            M["Vector4A"] = new Multivector(R4)
            {
                [E1] = 5,
                [E2] = -3,
                [E3] = 9,
                [E4] = -14
            };

            M["Vector4B"] = new Multivector(R4)
            {
                [E1] = 21,
                [E3] = 3
            };

            M["Vector4C"] = new Multivector(R4)
            {
                [E2] = -6,
                [E3] = -5,
                [E4] = 4
            };

            /*
            M["Big"] = new Multivector(R7)
            {
                [E()] = 8,
                [E(7)] = 4,
                [E(1, 5)] = 42,
                [E(5, 3, 7)] = -99,
                [E(1, 2, 3, 4, 5, 7)] = 222
            };

            M["BigSparse"] = new SparseMultivector(R7)
            {
                [E()] = -8,
                [E(7)] = -4,
                [E(1, 6)] = 2,
                [E(5, 3, 7)] = -1,
                [E(1, 2, 3, 4, 5, 6, 7)] = -0.2
            };

            M["GiantSparse"] = new SparseMultivector(R30)
            {
                [E()] = 1,
                [E(1, 29)] = 2,
                [E(8, 13, 15, 30)] = -3,
                [E(1, 2, 3, 4, 5, 6, 7, 8, 9, 10)] = 4.2,
                [E(1,2,3,4,5,6,7,8,9,10,11,12,13,4)] = 640
            };
            */
        }

        [Test]
        public void ConstructorCloneAndEquality()
        {
            var A = new Multivector(R3)
            {
                [E1] = 3.5,
                [E2] = 0.14,
                [E3] = -1 / 12
            };

#pragma warning disable CS1718 // Comparison made to same variable

            Assert.IsTrue(A == A);
            Assert.IsTrue(A.Equals(A));

#pragma warning restore CS1718 // Comparison made to same variable


            Assert.AreEqual(A,       M["Vector3B"]);
            Assert.IsTrue  (A ==     M["Vector3B"]);
            Assert.IsFalse (A !=     M["Vector3B"]);
            Assert.IsTrue  (A.Equals(M["Vector3B"]));


            A.Copy(M["Vector3A"]);

            Assert.AreEqual(A,       M["Vector3A"]);
            Assert.IsTrue  (A ==     M["Vector3A"]);
            Assert.IsFalse (A !=     M["Vector3A"]);
            Assert.IsTrue  (A.Equals(M["Vector3A"]));
            
            Assert.AreNotEqual(A,   M["Vector3B"]);
            Assert.IsFalse(A ==     M["Vector3B"]);
            Assert.IsTrue (A !=     M["Vector3B"]);
            Assert.IsTrue (A.Equals(M["Vector3A"]));

            Assert.AreEqual(A.GetHashCode(), M["Vector3A"].GetHashCode());

            var B = M["Vector3C"].Clone();

            Assert.IsFalse(ReferenceEquals(B,M["Vector3C"]));
            Assert.AreEqual(B, M["Vector3C"]);
        }
    }
}
