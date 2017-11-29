using System;
using System.Collections.Generic;
using System.Text;

namespace MathNet.GeometricAlgebra
{
    public class Space
    {
        public uint Dimension { get; }
        public uint PositiveDimension { get; }
        public uint NegativeDimension { get; }
        public uint NilpotentDimension { get; }

        public Space(uint positive, uint negative = 0, uint nil = 0)
        {
            var dimension = positive + negative + nil;

            if (dimension <= 64)
            {
                Dimension = dimension;
                PositiveDimension = positive;
                NegativeDimension = negative;
                NilpotentDimension = nil;
            }
            else throw new ArgumentOutOfRangeException("The total dimension cannot be greater than 64.");
        }

        public int BasisSquared(uint baseNumber)
        {
            if ((baseNumber -= PositiveDimension) <= 0) return 1;
            if ((baseNumber -= NegativeDimension) <= 0) return -1;
            if ((baseNumber -= NilpotentDimension) <= 0) return 0;
            throw new IndexOutOfRangeException("Base number out of range");
        }

    }
}
