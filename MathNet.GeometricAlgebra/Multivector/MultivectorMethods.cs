using MathNet.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathNet.GeometricAlgebra
{
    static class MultivectorMethods
    {
        public static void CheckSpaces(Space a, Space b)
        {
            if (a != b) throw new ArgumentException("To perform this operation, the entities have to be elements of the same space.");
        }

        /// <summary>Performs c = a + b, it is expected that all arrays have the same length.</summary>
        public static void Add(double[] a, double[] b, double[] c)
        {
            for(int i = 0, l = a.Length; i < l; i++) c[i] = a[i] + b[i];
        }

        /// <summary>Performs c = a - b, it is expected that all arrays have the same length.</summary>
        public static void Sub(double[] a, double[] b, double[] c)
        {
            for (int i = 0, l = a.Length; i < l; i++) c[i] = a[i] - b[i];
        }

        /// <summary>Performs b = -a, it is expected that both arrays have the same length.</summary>
        public static void Negate(double[] a, double[] b)
        {
            for (int i = 0, l = a.Length; i < l; i++) b[i] = -a[i];
        }

        ///<summary>Computes the hash code of the array.</summary>
        public static int HashCode(double[] a, int salt)
        {
            unchecked
            {
                int hash = salt;
                for(int i = 0, l = a.Length; i < l; i++) hash = hash* 29 + a[i].GetHashCode();
                return hash;
            }
        }

        ///<summary>Checks whether the contents of two arrays are the same</summary>
        public static bool AreEqual(double[] a, double[] b)
        {
            for (int i = 0, l = a.Length; i < l; i++)
                if (a[i] != b[i]) return false;

            return true;
        }

        ///<summary>Checks whether the contents of two sparse arrays are the same</summary>
        public static bool AreEqual(SparseArray<double> a, SparseArray<double> b)
        {
            foreach (var i in a.Keys.Union(b.Keys))
                if (a[i] != b[i]) return false;

            return true;
        }

        public static bool AreEqual(IEnumerable<double> a, IEnumerable<double> b)
        {
            using (var aEnum = a.GetEnumerator())
            using(var bEnum = b.GetEnumerator())
            while (true)
            {
                if (aEnum.MoveNext())
                {
                    if (bEnum.MoveNext())
                    {
                        if (aEnum.Current != bEnum.Current) return false;
                    }
                    else
                    {
                        throw new ApplicationException("Internal exception: the enumerables are of different lengths. This should have never happened.");
                    }
                }
                else
                {
                    break;
                }
            }

            return true;
        }
    }
}
