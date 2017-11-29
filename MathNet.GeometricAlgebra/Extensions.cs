using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MathNet
{

    public static class Binary
    {
        public static int ContBits(uint i)
        {
            i = i - ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);
            return (int)((((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24);
        }

        public static int CountBits(ulong i)
        {
            i = i - ((i >> 1) & 0x5555555555555555ul);
            i = (i & 0x3333333333333333ul) + ((i >> 2) & 0x3333333333333333ul);
            return (int)(unchecked(((i + (i >> 4)) & 0xF0F0F0F0F0F0F0Ful) * 0x101010101010101ul) >> 56);
        }


        static readonly int[] MultiplyDeBruijnBitPosition =
        {
            0, 9, 1, 10, 13, 21, 2, 29, 11, 14, 16, 18, 22, 25, 3, 30,
            8, 12, 20, 28, 15, 17, 24, 7, 19, 27, 23, 6, 26, 5, 4, 31
        };

        static int MostSignificantBit(uint i)
        {
            if (i == 0) return -1;

            i |= i >> 1;
            i |= i >> 2;
            i |= i >> 4;
            i |= i >> 8;
            i |= i >> 16;

            return MultiplyDeBruijnBitPosition[(i * 0x07C4ACDDU) >> 27];
        }


        static readonly ulong[] ZenonBig = { 0x2, 0xC, 0xF0, 0xFF00, 0xFFFF0000, 0xFFFFFFFF00000000 };
        static readonly int[] ZenonSmallInt = { 1, 2, 4, 8, 16, 32 };
        static readonly ulong[] ZenonSmallUlong = { 1, 2, 4, 8, 16, 32 };

        static int MostSignificantBit(ulong v)
        {
            if (v == 0) return -1;

            ulong r = 0;
            for(int i = 5; i >= 0; i--)
            {
                if ((v & ZenonBig[i]) != 0)
                {
                    v >>= ZenonSmallInt[i];
                    r |= ZenonSmallUlong[i];
                }
            }
            return (int)r;
        }


        private const ulong Magic = 0x37E84A99DAE458F;

        private static readonly int[] MagicTable =
        {
            0, 1, 17, 2, 18, 50, 3, 57,
            47, 19, 22, 51, 29, 4, 33, 58,
            15, 48, 20, 27, 25, 23, 52, 41,
            54, 30, 38, 5, 43, 34, 59, 8,
            63, 16, 49, 56, 46, 21, 28, 32,
            14, 26, 24, 40, 53, 37, 42, 7,
            62, 55, 45, 31, 13, 39, 36, 6,
            61, 44, 12, 35, 60, 11, 10, 9,
        };

        public static int LeastSignificantBit(ulong b)
            => b==0 ? -1 : MagicTable[((ulong)((long)b & -(long)b) * Magic) >> 58];


        /*
        //This one might be faster
        public static int MostSignificantBit(ulong b)
        {
            b |= b >> 1;
            b |= b >> 2;
            b |= b >> 4;
            b |= b >> 8;
            b |= b >> 16;
            b |= b >> 32;
            b = b & ~(b >> 1);
            return MagicTable[b * Magic >> 58];
        }
        */

    }
 
}
