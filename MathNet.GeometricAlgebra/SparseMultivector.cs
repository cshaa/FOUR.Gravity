using MathNet.GeometricAlgebra.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MathNet.GeometricAlgebra
{
    class SparseMultivector : Multivector
    {
        
        public SparseMultivector(Space space)
            : base(space, new SparseArray<double>(1 << (int)space.Dimension)) {}
    }
}
