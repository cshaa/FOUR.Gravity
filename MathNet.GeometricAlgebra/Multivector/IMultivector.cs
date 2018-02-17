using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using MathNet.Extensions;

namespace MathNet.GeometricAlgebra
{

    public interface IMultivector
        : IApproximatelyEquatable<IMultivector>,
          IApproximatelyEquatable<double>,
          IEnumerable<double>,
          IFormattable
    {
        IReadOnlyList<double> Elements { get; }
        Space Space { get; }

        double this[int index] { get; }
        double ScalarPart { get; }
        Vector VectorPart { get; }
        PureGrade GetGrade(int grade);
    }

    public interface IImmutableMultivector : IMultivector { }
    public interface IMutableMultivector : IMultivector { }
}
