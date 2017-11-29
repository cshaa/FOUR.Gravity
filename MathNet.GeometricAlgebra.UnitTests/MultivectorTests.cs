using MathNet.GeometricAlgebra;
using NUnit.Framework;
using System.Collections.Generic;

namespace MathNet.GeometricAlgebra.UnitTests
{
    [TestFixture]
    public class MultivectorTests
    {

        Space R3 = new Space(2);
        Space R4 = new Space(4);
        Space R64 = new Space(64);

        Dictionary<string, Multivector> M;

        [OneTimeSetUp]
        public void SetUp()
        {
            M["GeneralMultivector4"] = new Multivector(R4);
        }

        [Test]
        public void TestMethod1()
        {
            Assert.IsTrue(true);
        }
    }
}
