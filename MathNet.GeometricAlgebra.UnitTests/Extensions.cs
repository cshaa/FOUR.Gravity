using System;
using System.Collections.Generic;
using System.Text;

namespace MathNet.Extensions
{
    class Assert : NUnit.Framework.Assert
    {
        public static void AreApproximatelyEqual<T>(
            IApproximatelyEquatable<T> expected,
            T actual,
            double? delta = null,
            double? relativeDelta = null,
            long? ulpsApart = null
        ) where T : IEquatable<T>
        {
            if (expected.ApproximatelyEquals(actual, delta, relativeDelta, ulpsApart)) return;
            else AreEqual(expected, actual);
        }
    }
}
