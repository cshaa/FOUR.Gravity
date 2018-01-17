using MathNet.GeometricAlgebra;
using NUnit.Framework;
using System.Collections.Generic;

using static System.Math;
using static MathNet.GeometricAlgebra.Constants.Basis;
using System;
using MathNet.Numerics.LinearAlgebra;

namespace MathNet.GeometricAlgebra.UnitTests
{
    using Assert = MathNet.Extensions.UnitTesting.Assert;

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
                [EScalar] = 2/13d,
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
                [E3] = -1/12d
            };

            M["Vector3C"] = new Multivector(R3)
            {
                [E2] = 8,
                [E3] = -7
            };

            M["GeneralMultivector4"] = new Multivector(R4)
            {
                [EScalar] = 5/3d,
                [E1] = 12,
                [E3] = -45,
                [E14] = 3/5d,
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
        public void Space()
        {
            var S = new Space(2, 3, 4);

            Assert.AreEqual(2, S.PositiveDimension);
            Assert.AreEqual(3, S.NegativeDimension);
            Assert.AreEqual(4, S.NilpotentDimension);
            Assert.AreEqual(9, S.Dimension);

            Assert.AreEqual(0b000, S.IndexFromBladeBasis(0, 0));
            Assert.AreEqual(0b001, S.IndexFromBladeBasis(1, 0));
            Assert.AreEqual(0b010, S.IndexFromBladeBasis(1, 1));
            Assert.AreEqual(0b100, S.IndexFromBladeBasis(1, 2));
            Assert.AreEqual(0b011, S.IndexFromBladeBasis(2, 0));
            Assert.AreEqual(0b101, S.IndexFromBladeBasis(2, 1));
            Assert.AreEqual(0b110, S.IndexFromBladeBasis(2, 8));
            Assert.AreEqual(0b111, S.IndexFromBladeBasis(3, 0));
            Assert.AreEqual(0b111111111, S.IndexFromBladeBasis(9, 0));

            Assert.AreEqual((0, 0), S.BladeBasisFromIndex(0b000));
            Assert.AreEqual((1, 0), S.BladeBasisFromIndex(0b001));
            Assert.AreEqual((1, 1), S.BladeBasisFromIndex(0b010));
            Assert.AreEqual((1, 2), S.BladeBasisFromIndex(0b100));
            Assert.AreEqual((2, 0), S.BladeBasisFromIndex(0b011));
            Assert.AreEqual((2, 1), S.BladeBasisFromIndex(0b101));
            Assert.AreEqual((2, 8), S.BladeBasisFromIndex(0b110));
            Assert.AreEqual((3, 0), S.BladeBasisFromIndex(0b111));
            Assert.AreEqual((9, 0), S.BladeBasisFromIndex(0b111111111));

            Assert.AreEqual(new Multivector(S), S.Zero);

            Assert.AreEqual(1,  S.BasisSquared(0,0));
            Assert.AreEqual(1,  S.BasisSquared(1,0));
            Assert.AreEqual(1,  S.BasisSquared(1,1));
            Assert.AreEqual(-1, S.BasisSquared(1,2));
            Assert.AreEqual(-1, S.BasisSquared(1,3));
            Assert.AreEqual(-1, S.BasisSquared(1,4));
            Assert.AreEqual(0,  S.BasisSquared(1,5));
            Assert.AreEqual(0,  S.BasisSquared(1,6));
            Assert.AreEqual(0,  S.BasisSquared(1,7));
            Assert.AreEqual(0,  S.BasisSquared(1,8));

            Assert.Throws<IndexOutOfRangeException>(() => S.BasisSquared(1,9), "Base number out of range");

            for(uint i = 0; i < 1<<9; i++)
                Assert.AreEqual(
                    (0, S.BasisSquared(i)),
                    S.BasisMultiply(i, i)
                );

            Assert.AreEqual(1, S.BasisSquared(0b000010111));
            Assert.AreEqual(-1,S.BasisSquared(0b000011001));
            Assert.AreEqual(-1,S.BasisSquared(0b000001101));
            Assert.AreEqual(0, S.BasisSquared(0b110100110));
            Assert.AreEqual(0, S.BasisSquared(0b010010001));

            Assert.AreEqual((0b011111, 1), S.BasisMultiply(0b001101, 0b010010));
            Assert.AreEqual((0b100110, 1), S.BasisMultiply(0b110101, 0b010011));
            Assert.AreEqual(0, S.BasisMultiply(0b101110, 0b110101).Sign);
        }





        [Test]
        public void MultivectorConstructor()
        {
            var A = new Multivector(R3)
            {
                [E1] = 3.5,
                [E2] = 0.14,
                [E3] = -1 / 12d
            };

            Assert.AreEqual(3.5, A[1]);
            Assert.AreEqual(0.14, A[2]);
            Assert.AreEqual(-1/12d, A[4]);

#pragma warning disable CS1718 // Comparison made to same variable

            Assert.IsTrue(A == A);
            Assert.IsTrue(A.Equals(A));
            Assert.IsFalse(A != A);

#pragma warning restore CS1718 // Comparison made to same variable

        }





        [Test]
        public void MultivectorCopy()
        {

            var A = new Multivector(R3)
            {
                [E1] = 3.5,
                [E2] = 0.14,
                [E3] = -1 / 12d
            };

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

        }





        [Test]
        public void MultivectorClone()
        {
            var A = M["Vector3A"].Clone();

            Assert.AreEqual(A, A.Clone());
            Assert.AreEqual(A, new Multivector(A));
            Assert.AreEqual(A, (new Multivector(R3)).Copy(A));

            var B = M["Vector3C"].Clone();

            Assert.IsFalse(ReferenceEquals(B,M["Vector3C"]));
            Assert.AreEqual(B, M["Vector3C"]);
        }





        [Test]
        public void MultivectorAddAndSub()
        {
            var A = M["Vector3A"].Clone();
            var B = M["Vector3B"].Clone();
            var C = M["Vector3C"].Clone();

            Assert.AreEqual(A + B, A.Add(B));
            Assert.AreEqual(A - B, A.Sub(B));

            Assert.AreEqual(M["Vector3A"], A);

            Assert.AreEqual(A + B, B + A);
            Assert.AreEqual(A + (B + C), (A + B) + C);
            Assert.AreEqual(A - B, A + (-B));
            Assert.AreEqual(A - A, B - B);
            Assert.AreEqual(new Multivector(R3), A - A);

            Assert.AreEqual(new Multivector(R3)
            {
                [E1] = 7.5,
                [E2] = 3.14,
                [E3] = 5 - 1/12d
            },
            A + B);

            Assert.AreEqual(new Multivector(R3)
            {
                [E1] = 4,
                [E2] = -5,
                [E3] = 12
            },
            A - C);

            Assert.AreEqual(
                new Multivector(R4)
                {
                    [EScalar] = 5 / 3d,
                    [E1] = -4,
                    [E2] = -3,
                    [E3] = -39,
                    [E4] = -14,
                    [E14] = 3 / 5d,
                    [E23] = -6.2832,
                    [E34] = 1,
                    [E124] = -3,
                    [E134] = 86,
                    [E1234] = -5,
                },

                M["GeneralMultivector4"]
                + M["Vector4A"]
                - M["Vector4B"]
            );

            Assert.AreEqual(M["Vector3A"], A);
            Assert.AreEqual(M["Vector3B"], B);
            Assert.AreEqual(M["Vector3C"], C);
        }





        [Test]
        public void GeometricProduct()
        {
            var A = M["Vector3A"].Clone();
            var B = M["Vector3B"].Clone();
            var C = M["Vector3C"].Clone();
            var G = M["GeneralMultivector3"].Clone();

            Assert.AreEqual(A * B, A.Clone().Multiply(B));
            Assert.AreEqual(A * B, A.Clone().Mul(B));
            
            Assert.AreApproximatelyEqual(A * (B * C), (A * B) * C);
            Assert.AreApproximatelyEqual(A * (B + C), (A * B) + (A * C));
            Assert.AreApproximatelyEqual((A + B) * C, (A * C) + (B * C));

            Assert.AreApproximatelyEqual(new Multivector(R3)
            {
                ScalarPart = 11,
                [E1] = 8/13d + 10*PI,
                [E2] = 6/13d,
                [E3] = 10/13d - 8*PI,
                [E12] = 237,
                [E13] = -101,
                [E23] = 153,
                [E123] = -6*PI
            }, G * A);
            
            Assert.AreEqual(M["Vector3A"], A);
            Assert.AreEqual(M["Vector3B"], B);
            Assert.AreEqual(M["Vector3C"], C);
            Assert.AreEqual(M["GeneralMultivector3"], G);
        }





        [Test]
        public void WedgeProductn()
        {
            var A = M["Vector3A"].Clone().VectorPart;
            var B = M["Vector3B"].Clone().VectorPart;
            var C = M["Vector3C"].Clone().VectorPart;
            var G = M["GeneralMultivector3"].Clone();

            var A4 = M["Vector4A"].Clone();
            var G4 = M["GeneralMultivector4"].Clone();

            Assert.AreEqual(A ^ B, A.Clone().Wedge(B));

            Assert.AreApproximatelyEqual(A ^ (B ^ C), (A ^ B) ^ C);
            Assert.AreApproximatelyEqual(A ^ (B + C), (A ^ B) + (A ^ C));
            Assert.AreApproximatelyEqual((A + B) ^ C, (A ^ C) + (B ^ C));

            Assert.AreApproximatelyEqual(A ^ B, -B ^ A);
            Assert.AreApproximatelyEqual(A * B, A.Dot(B) + (A ^ B));
            Assert.AreApproximatelyEqual(B * A, A.Dot(B) - (A ^ B));
            Assert.AreApproximatelyEqual(B ^ B, R3.Zero);
            Assert.AreApproximatelyEqual(A4 ^ A4, R4.Zero);

            Assert.AreApproximatelyEqual(new Multivector(R3)
            {
                [E1]   = 7/13d,
                [E2]   = 7/325d,
                [E3]   = -1/78d,
                [E12]  = -11.2,
                [E13]  = 5/12d,
                [E23]  = -1/4d,
                [E123] = -0.28 * PI
            }, B ^ G);


            /*
            
            General3:
                [EScalar] = 2 / 13d,
                [E1] = 5,
                [E2] = -3,
                [E13] = 2 * PI,
                [E123] = 42

            3A:
                [E1] = 4,
                [E2] = 3,
                [E3] = 5

            3B:
                [E1] = 3.5,
                [E2] = 0.14,
                [E3] = -1 / 12d

            3C:
                [E2] = 8,
                [E3] = -7

            General4:
                [EScalar] = 5 / 3d,
                [E1] = 12,
                [E3] = -45,
                [E14] = 3 / 5d,
                [E23] = -6.2832,
                [E34] = 1,
                [E124] = -3,
                [E134] = 86,
                [E1234] = -5

            4A:
                [E1] = 5,
                [E2] = -3,
                [E3] = 9,
                [E4] = -14

            4B:
                [E1] = 21,
                [E3] = 3

            4C:
                [E2] = -6,
                [E3] = -5,
                [E4] = 4
            
            */



            Assert.AreEqual(M["Vector3A"], A);
            Assert.AreEqual(M["Vector3B"], B);
            Assert.AreEqual(M["Vector3C"], C);
            Assert.AreEqual(M["GeneralMultivector3"], G);

            Assert.AreEqual(M["Vector4A"], A4);
            Assert.AreEqual(M["GeneralMultivector4"], G4);
        }





        [Test]
        public void PureGrade()
        {
            var A = M["GeneralMultivector3"].Clone();

            Assert.AreEqual(2 / 13d, A.ScalarPart);

            Assert.AreEqual(new Multivector(R3)
            {
                ScalarPart = 2 / 13d
            },
            A.GetGrade(0));

            Assert.AreEqual(new Multivector(R3)
            {
                [E1] = 5,
                [E2] = -3
            },
            A.GetGrade(1));

            Assert.AreEqual(new Multivector(R3)
            {
                [E13] = 2 * PI
            },
            A.GetGrade(2));

            Assert.AreEqual(new Multivector(R3)
            {
                [E123] = 42
            },
            A.GetGrade(3));



            A.ScalarPart = -3;

            Assert.AreEqual(-3, A.ScalarPart);

            Assert.AreEqual(new Multivector(R3)
            {
                ScalarPart = -3
            },
            A.GetGrade(0));



            Assert.AreEqual(new Vector(R3, new double[]
            {
                5, -3, 0
            }),
            A.VectorPart);

            A.VectorPart = new Vector(R3, Vector<double>.Build.Dense(new double[] { 1, 2, 3 }));

            Assert.AreEqual(new Vector(R3, new double[]
            {
                1, 2, 3
            }),
            A.VectorPart);



            A.SetGrade(new PureGrade(R3, 2)
            {
                [E12] = 42,
                [E13] = 43,
                [E23] = 44
            });

            Assert.AreEqual(new Multivector(R3)
            {
                ScalarPart = -3,
                [E1] = 1,
                [E2] = 2,
                [E3] = 3,
                [E12] = 42,
                [E13] = 43,
                [E23] = 44,
                [E123] = 42
            }, A);

        }
    }
}
