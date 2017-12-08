using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MathNet.GeometricAlgebra.Extensions
{
    class SparseArray<T> : ICloneable, IList<T>, ICollection<T>, IEnumerable<T>, IStructuralComparable, IStructuralEquatable
    {
        protected int Length;
        protected Dictionary<int, T> Dictionary;

        public T DefaultValue;

        public SparseArray(int length, T defaultValue = default(T))
        {
            Length = length < 0 ? 0 : length;
        }
        
        public T this[int index]
        {
            get => Dictionary[index];
            set => Dictionary[index] = value;
        }

        public int Count => Length;



        public bool IsReadOnly => false;

        public void Add(T item) => Dictionary[Length++] = item;

        public void Clear() => Dictionary.Clear();
        
        IEnumerator IEnumerable.GetEnumerator() => Dictionary.GetEnumerator();

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)Dictionary).GetEnumerator();


        
        public SparseArray<T> Clone()
        {
            var clone = new SparseArray<T>(Length);
            foreach (var (key, value) in Dictionary)
                clone.Dictionary.Add(key,value);

            return clone;
        }

        object ICloneable.Clone() => Clone();

        
        public void CopyTo(T[] array, int index)
        {
            array = array ?? throw new ArgumentNullException("The array cannot be null.");
            if (index >= array.Length) throw new ArgumentOutOfRangeException("The starting index is larger than the array's length.");
            if (index + Length > array.Length) throw new ArgumentException("The array is too short, cannot fit the contents of SparseArray there.");

            for (int i = 0; i < Length; i++)
                array[index + i]
                    = Dictionary.ContainsKey(i)
                    ? Dictionary[i]
                    : DefaultValue;
        }


        public bool Contains(T item) => Dictionary.ContainsValue(item);

        
        public int IndexOf(T item)
            => Dictionary.First(
                typeof(T) is IEquatable<T> ? (Func<KeyValuePair<int, T>, bool>)
                (x => (IEquatable<T>)x.Value == (IEquatable<T>)item) : (x => x.Value.Equals(item))
               ).Key;



        public bool Equals(object other, IEqualityComparer comparer)
        {
            throw new NotImplementedException();
        }
        

        public int CompareTo(object other, IComparer comparer)
        {
            throw new NotImplementedException();
        }


        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            if (!Dictionary.ContainsValue(item)) return true;

            var index = IndexOf(item);

            foreach (var (key, value) in Dictionary.OrderBy(x => x.Key).SkipWhile(x => x.Key<=index))
            {

            }
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }


        public int GetHashCode(IEqualityComparer comparer)
        {
            throw new NotImplementedException();
        }

    }
}
