using System;
using System.Collections.Generic;
using System.Text;
using static MathNet.Numerics.Combinatorics;

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

            if (dimension <= 30)
            {
                Dimension = dimension;
                PositiveDimension = positive;
                NegativeDimension = negative;
                NilpotentDimension = nil;
            }
            else throw new ArgumentOutOfRangeException("The total dimension cannot be greater than 30.");
            // “No one will ever need terabytes of memory.” —Bill Gates

            SetUpConversionBetweenIndexAndBladeBasis();
        }


        public int BasisSquared(uint grade, uint baseNumber)
        {
            if (grade == 1)
            {
                if ((baseNumber -= PositiveDimension) <= 0) return 1;
                if ((baseNumber -= NegativeDimension) <= 0) return -1;
                if ((baseNumber -= NilpotentDimension) <= 0) return 0;
                throw new IndexOutOfRangeException("Base number out of range");
            }

            return BasisSquared(IndexFromBladeBasis(grade,baseNumber));
        }

        public int BasisSquared(ulong index)
        {
            var grade = BladeBasisFromIndex(index).Grade;
            int sign = grade % 4 < 2 ? 1 : -1; //see below
            while (index != 0)
            {
                var lsb = (uint)Binary.LeastSignificantBit(index);

                switch (BasisSquared(1,lsb))
                {
                    case 0: return 0;
                    case -1: sign = -sign; break;
                }

                index &= index - 1; //remove lsb
            }

            return sign;

            /*
             * The number of flips you have to perform when squaring
             * a basis blade of N basis vectors is the triangular
             * number of N-1, that is N*(N-1)/2.
             * 
             * e₁e₂e₃e₄ * e₁e₂e₃e₄   = −e₁²e₂e₃e₄ * e₂e₃e₄ (3 flips)
             * e₂e₃e₄ * e₂e₃e₄ *(-1) = −e₂²e₃e₄ * e₃e₄     (2 flips)
             * e₃e₄ * e₃e₄     *(-1) = e₃²e₄ * e₄          (1 flip )
             * e₄ * e₄               = e₄²            = 1  (0 flips)
             * 
             * Total of 3+2+1 = tri(3) = tri(N-1) = 6 flips.
             * 
             * 
             * When the number of flips is odd, the sign gets flipped,
             * when it's even, it doesn't.
             * 
             * "Number of flips is even"
             * ⇒ (∃int k) N*(N-1)/2 = 2k
             * ⇒ (∃int k) N*(N-1) = 4k
             * ⇒ N*(N-1) mod 4 = 0
             * ⇒ N mod 4 ∈ {0, 1}
             * ⇒ N mod 4 < 2
             * 
             * Thus:
             * sign = grade % 4 < 2 ? 1 : -1;
             */
        }

        public (ulong BasisBlade, int Sign) BasisMultiply(ulong e, ulong f)
        {
            int sign = 1;
            while (f != 0)
            {
                // Get the number of the lowest basis vector
                var lsb = Binary.LeastSignificantBit(f);

                // e₁e₂(e₄e₇e₁₂) * e₂e₃
                //      ^^^^^^^ Count these basis vectors.
                // If the number is odd, flip the sign
                if (Binary.CountBits(e >> lsb + 1) % 2 != 0)
                    sign = -sign;

                // If the basis vector is a factor of both the multiplicator
                // and the multiplicand, square it and change the sign respectively.
                // If the basis vector is nilpotent, return scalar zero.
                if( (e & 1ul << lsb) != 0)
                {
                    sign *= BasisSquared(1, (uint)lsb);
                    if (sign == 0) return (0, 0);
                }

                // Flip the presence of the basis vector in e (if it's already
                // there, remove it, and vice versa) and remove it from f.
                e ^= 1ul << lsb;
                f &= f - 1;
            }
            return (e, sign);
            
            //TODO explain a little more
        }


        ulong[][] indexFromBladeBasisCache;
        (uint, uint)[] bladeBasisFromIndexCache;

        public ulong IndexFromBladeBasis(uint grade, uint baseNumber)
            => indexFromBladeBasisCache[grade][baseNumber];

        public (uint Grade, uint BaseNumber) BladeBasisFromIndex(ulong index)
            => bladeBasisFromIndexCache[index];



        /* Set up the conversion between (basis blade index) and (grade and base number) */

        void SetUpConversionBetweenIndexAndBladeBasis()
        {

            indexFromBladeBasisCache = new ulong[Dimension+1][];
            bladeBasisFromIndexCache = new(uint, uint)[1ul << (int)Dimension];

            // Array of indices of basis vectors this basis blade is composed of.
            // The maximum length we will need is equal to the dimension of the space
            int[] indices = new int[Dimension];

            for (uint grade = 0; grade <= Dimension; grade++)
            {
                // The number of basis blades of this grade
                var combinations = (int)Combinations((int)Dimension, (int)grade);

                // Add a new sub-array to the jagged array
                indexFromBladeBasisCache[grade] = new ulong[combinations];

                // Set the indices of basis vectors to the lowest end
                for (int i = 0; i < grade; i++) indices[i] = i;

                // Increment through all possible combinations
                // This code solves https://stackoverflow.com/q/1776442/1137334
                for (uint baseNumber = 0; true; baseNumber++)
                {
                    int j;

                    // Construct the (basis blade) index from basis vector indices
                    ulong index = 0;
                    for (j = 0; j < grade; j++) index |= 1ul << indices[j];

                    indexFromBladeBasisCache[grade][baseNumber] = index;
                    bladeBasisFromIndexCache[index] = (grade, baseNumber);


                    /* Find the next combination: */

                    // Foreach all the indices
                    for (j = (int)grade - 1; j >= 0; j--)
                    {
                        // If this index isn't at the end
                        if (indices[j] < Dimension-grade+j)
                        {
                            // Move this index one place up
                            var newplace = ++indices[j++];

                            // Set the greater indices to their new lowest place
                            for (; j < grade; j++) indices[j] = ++newplace;

                            // We just found the next combination
                            break;
                        }
                    }

                    // If all indices are at the end, move on to the next grade
                    if(j<1) break;
                }

                // All combinations of this grade are done, let's move on!
            }

            // All the grades are done, our cache is initilized!
        }

    }
}
