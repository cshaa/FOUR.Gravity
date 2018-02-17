using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using MathNet.Extensions;
using static MathNet.Numerics.Combinatorics;

namespace MathNet.GeometricAlgebra
{
    public class Space
    {
        virtual public int Dimension { get; }
        virtual public int PositiveDimension { get; }
        virtual public int NegativeDimension { get; }
        virtual public int NilpotentDimension { get; }

        public Multivector Zero { get; }

        public Space(int positive, int negative = 0, int nil = 0) : this()
        {
            if (positive < 0 || negative < 0 || nil < 0)
                throw new ArgumentOutOfRangeException();

            var dimension = positive + negative + nil;

            if (dimension <= 30)
            {
                Dimension = (int)dimension;
                PositiveDimension = (int)positive;
                NegativeDimension = (int)negative;
                NilpotentDimension = (int)nil;
            }
            else throw new ArgumentOutOfRangeException("The total dimension cannot be greater than 30.");
            // “No one will ever need 34.3 T of memory.” —Bill Gates

            SetUpConversionBetweenIndexAndBladeBasis();

            Zero = new Multivector(this, ImmutableArray.CreateRange(Enumerable.Repeat(0.0, Dimension)));
        }

        protected Space() { }
        



        public static Space DegenerateSplitComplexSpace { get; } = new Space(1);
        public static Space DegenerateScalarSpace { get; } = new Space(0);
        public static Space NullSpace { get; } = new NullSpace();



        public int BasisSquared(uint grade, uint component)
        {
            if (this is NullSpace)
                throw new InvalidOperationException("There is no basis in null space.");

            if (grade == 1)
            {
                if (component < PositiveDimension) return 1;
                else component -= (uint)PositiveDimension;

                if (component < NegativeDimension) return -1;
                else component -= (uint)NegativeDimension;

                if (component < NilpotentDimension) return 0;
                else throw new IndexOutOfRangeException("Base number out of range");
            }

            return BasisSquared(IndexFromBladeBasis(grade,component));
        }

        public int BasisSquared(ulong index)
        {
            if (this is NullSpace)
                throw new InvalidOperationException("There is no basis in null space.");

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

        public (ulong Index, int Sign) BasisMultiply(ulong e, ulong f)
        {
            if (this is NullSpace)
                throw new InvalidOperationException("There is no basis in null space.");

            if (e==1 && f==1)
            {
                Console.WriteLine();
            }

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
                    var a = lsb;
                    var b = (uint)lsb;
                    sign *= BasisSquared(1, b);
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

        public ulong IndexFromBladeBasis(uint grade, uint component)
            => (this is NullSpace)
            ? throw new InvalidOperationException("There is no basis in null space.")
            : indexFromBladeBasisCache[grade][component];

        public (uint Grade, uint Component) BladeBasisFromIndex(ulong index)
            => (this is NullSpace)
            ? throw new InvalidOperationException("There is no basis in null space.")
            : bladeBasisFromIndexCache[index];



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


    public class NullSpace : Space
    {
        public NullSpace() : base() { }
        override public int Dimension { get; } = -1;
        override public int PositiveDimension { get; } = -1;
        override public int NegativeDimension { get; } = -1;
        override public int NilpotentDimension { get; } = -1;

        public override bool Equals(object obj)
            => obj is NullSpace;

        public override int GetHashCode() => 0;
    }

}
