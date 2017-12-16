using System;
using System.Collections.Generic;
using System.Text;

namespace MathNet.GeometricAlgebra.Constants
{
    public static class Basis
    {
        public const int EScalar = 0b000000;

        public const int E1 = 0b000001;
        public const int E2 = 0b000010;
        public const int E3 = 0b000100;
        public const int E4 = 0b001000;
        public const int E5 = 0b010000;
        public const int E6 = 0b100000;

        public const int E12 = 0b000011;
        public const int E13 = 0b000101;
        public const int E14 = 0b001001;
        public const int E15 = 0b010001;
        public const int E16 = 0b100001;
        public const int E23 = 0b000110;
        public const int E24 = 0b001010;
        public const int E25 = 0b010010;
        public const int E26 = 0b100010;
        public const int E34 = 0b001100;
        public const int E35 = 0b010100;
        public const int E36 = 0b100100;
        public const int E45 = 0b011000;
        public const int E46 = 0b101000;
        public const int E56 = 0b110000;

        public const int E123 = 0b000111;
        public const int E124 = 0b001011;
        public const int E125 = 0b010011;
        public const int E126 = 0b100011;
        public const int E134 = 0b001101;
        public const int E135 = 0b010101;
        public const int E136 = 0b100101;
        public const int E145 = 0b011001;
        public const int E146 = 0b101001;
        public const int E156 = 0b110001;
        public const int E234 = 0b001110;
        public const int E235 = 0b010110;
        public const int E236 = 0b100110;
        public const int E245 = 0b011010;
        public const int E246 = 0b101010;
        public const int E256 = 0b110010;
        public const int E345 = 0b011100;
        public const int E346 = 0b101100;
        public const int E356 = 0b110100;
        public const int E456 = 0b111000;

        public const int E1234 = 0b001111;
        public const int E1235 = 0b010111;
        public const int E1236 = 0b100111;
        public const int E1245 = 0b011011;
        public const int E1246 = 0b101011;
        public const int E1256 = 0b110011;
        public const int E1345 = 0b011101;
        public const int E1346 = 0b101101;
        public const int E1356 = 0b110101;
        public const int E1456 = 0b111001;
        public const int E2345 = 0b011110;
        public const int E2346 = 0b101110;
        public const int E2356 = 0b110110;
        public const int E2456 = 0b111010;
        public const int E3456 = 0b111100;

        public const int E12345 = 0b011111;
        public const int E12346 = 0b101111;
        public const int E12356 = 0b110111;
        public const int E12456 = 0b111011;
        public const int E13456 = 0b111101;
        public const int E23456 = 0b111110;

        public const int E123456 = 0b111111;


        /// <summary>
        /// Allows you to construct an index from vector basis numbers.<br/>
        /// E() == E(0) == EScalar;<br/>
        /// E(5) == E5<br/>
        /// E(2,4,3) == E234<br/>
        /// E(4,3,2,1,5,6) == E123456
        /// </summary>
        /// <param name="numbers">The ordinal numbers of basis vectors starting at 1</param>
        /// <returns>The base index</returns>
        public static int E(params int[] numbers)
        {
            int index = 0;

            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] > 30) throw new ArgumentException("Dimension has to be at most 30.");
                if ((index & 1 << numbers[i] - 1) != 0) throw new ArgumentException("One basis number was given multiple times");
                index &= 1 << numbers[i] - 1;
            }

            return index;
        }
    }
}

