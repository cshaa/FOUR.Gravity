using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;

namespace PerformanceTesting
{
    class Program
    {
        static Numeric<T>[] PrepareArray<T>(int max, Numeric<T> upper)
        {
            var rnd = new Random();
            var dec = new decimal[max];
            for (int i = 0; i < max; i++)
            {
                dec[i] = (decimal)rnd.NextDouble() * upper;
            }
            Exp(42);
        }


        static void Main(string[] args)
        {
            const int max = 1_000_000;
            const int upper = 65;
            var rnd = new Random();

            var dec = new decimal[max];
            for(int i = 0; i < max; i++)
            {
                dec[i] = (decimal)rnd.NextDouble()*upper;
            }
            Exp(42);

            var dou = new  double[max];
            for (int i = 0; i < max; i++)
            {
                dou[i] = rnd.NextDouble()*upper;
            }
            Math.Exp(42);

            var s1 = Stopwatch.StartNew();
            for (int i = 0; i < max; i++)
            {
                dou[i] = Math.Exp(dou[i]);
            }
            s1.Stop();

            var s2 = Stopwatch.StartNew();
            for (int i = 0; i < max; i++)
            {
                dec[i] = Exp(dec[i]);
            }
            s2.Stop();

            Console.Write("Math.Log(double): ");
            Console.WriteLine(((double)(s1.Elapsed.TotalMilliseconds *
                1000000) / max).ToString("0.00 ns"));
            Console.Write("Log(decimal):     ");
            Console.WriteLine(((double)(s2.Elapsed.TotalMilliseconds *
                1000000) / max).ToString("0.00 ns"));
            Console.Read();
        }



        static ImmutableArray<int> InitializeFromArray(ImmutableArray<int> array)
        {
            var a = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                a[i] = array[i]+1;
            }
            return ImmutableArray.Create(a);
        }

        static ImmutableArray<int> InitializeFromEnumerable(ImmutableArray<int> array)
        {
            return ImmutableArray.CreateRange(array.Select(x=>x+1));
        }

        static ImmutableArray<int> InitializeFromBuilder(ImmutableArray<int> array)
        {
            var a = ImmutableArray.CreateBuilder<int>(array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                a.Add( array[i] + 1 );
            }
            return a.MoveToImmutable();
        }

        static ImmutableArray<int>.Builder CachedBuilder = ImmutableArray.CreateBuilder<int>();

        static ImmutableArray<int> InitializeFromCachedBuilder(ImmutableArray<int> array)
        {
            var a = CachedBuilder;
            a.Capacity = array.Length;

            for (int i = 0; i < array.Length; i++)
            {
                a.Add(array[i] + 1);
            }
            return a.MoveToImmutable();
        }

        static int[] Mutable(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i]++;
            }
            return array;
        }

        static int[] NewMutableArrayEachStep(int[] array)
        {
            var a = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                a[i] = array[i] + 1;
            }
            return array;
        }

        static void Maine(string[] args)
        {
            const int max = 1000000;


            var array1 = ImmutableArray.Create(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            InitializeFromArray(array1);

            var s1 = Stopwatch.StartNew();
            for (int i = 0; i < max; i++)
            {
                array1 = InitializeFromArray(array1);
            }
            s1.Stop();


            var array2 = ImmutableArray.Create(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            InitializeFromEnumerable(array2);

            var s2 = Stopwatch.StartNew();
            for (int i = 0; i < max; i++)
            {
                array2 = InitializeFromEnumerable(array2);
            }
            s2.Stop();


            var array3 = ImmutableArray.Create(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            InitializeFromBuilder(array3);

            var s3 = Stopwatch.StartNew();
            for (int i = 0; i < max; i++)
            {
                array3 = InitializeFromBuilder(array3);
            }
            s3.Stop();


            var array4 = ImmutableArray.Create(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            InitializeFromCachedBuilder(array4);

            var s4 = Stopwatch.StartNew();
            for (int i = 0; i < max; i++)
            {
                array4 = InitializeFromBuilder(array4);
            }
            s4.Stop();


            var array5 = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            Mutable(array5);

            var s5 = Stopwatch.StartNew();
            for (int i = 0; i < max; i++)
            {
                array5 = Mutable(array5);
            }
            s5.Stop();

            var array6 = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            NewMutableArrayEachStep(array6);

            var s6 = Stopwatch.StartNew();
            for (int i = 0; i < max; i++)
            {
                array6 = Mutable(array6);
            }
            s6.Stop();

            Console.WriteLine(((double)(s1.Elapsed.TotalMilliseconds *
                1000000) / max).ToString("0.00 ns"));
            Console.WriteLine(((double)(s2.Elapsed.TotalMilliseconds *
                1000000) / max).ToString("0.00 ns"));
            Console.WriteLine(((double)(s3.Elapsed.TotalMilliseconds *
                1000000) / max).ToString("0.00 ns"));
            Console.WriteLine(((double)(s4.Elapsed.TotalMilliseconds *
                1000000) / max).ToString("0.00 ns"));
            Console.WriteLine(((double)(s5.Elapsed.TotalMilliseconds *
                1000000) / max).ToString("0.00 ns"));
            Console.WriteLine(((double)(s6.Elapsed.TotalMilliseconds *
                1000000) / max).ToString("0.00 ns"));
            Console.Read();
        }
    }
}
