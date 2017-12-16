using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MathNet.GeometricAlgebra.Extensions
{
    public interface ISparseList<T> : ICloneable, IList<T>, ICollection<T>, IEnumerable<T>, IStructuralComparable, IStructuralEquatable
    {
        T DefaultValue { get; set; }

        IEnumerable<KeyValuePair<int, T>> KeyValuePairs { get; }
        IEnumerable<int> Keys { get; }
        IEnumerable<T> Values { get; }
    }
    

    public class SparseArray<T> : ISparseList<T>
    {
        protected int Length;
        protected Dictionary<int, T> Dictionary = new Dictionary<int, T>();

        public T DefaultValue;
        T ISparseList<T>.DefaultValue { get => DefaultValue; set => DefaultValue = value; }

        public SparseArray(int length, T defaultValue = default(T))
        {
            Length = length < 0 ? 0 : length;
            DefaultValue = defaultValue;
        }

        public T this[int index]
        {
            get => Dictionary.ContainsKey(index) ? Dictionary[index] : DefaultValue;
            set
            {
                if (value is IEquatable<T> eqValue ? eqValue == DefaultValue as IEquatable<T> : Object.Equals(value, DefaultValue))
                    Dictionary.Remove(index);
                else
                    Dictionary[index] = value;
            }
        }

        public int Count => Length;



        public bool IsReadOnly => false;

        public void Add(T item) => Dictionary[Length++] = item;

        public void Clear() => Dictionary.Clear();



        class SparseKeyValuePairs<S> : IEnumerable<KeyValuePair<int,S>>
        {
            Dictionary<int, S> Dictionary;
            public SparseKeyValuePairs(Dictionary<int,S> dict) => Dictionary = dict;
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public IEnumerator<KeyValuePair<int, S>> GetEnumerator() => Dictionary.GetEnumerator();
        }

        class SparseKeys<S> : IEnumerable<int>
        {
            Dictionary<int, S> Dictionary;
            public SparseKeys(Dictionary<int, S> dict) => Dictionary = dict;
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public IEnumerator<int> GetEnumerator() => Dictionary.Select( e => e.Key ).GetEnumerator();
        }

        class SparseValues<S> : IEnumerable<S>
        {
            Dictionary<int, S> Dictionary;
            public SparseValues(Dictionary<int, S> dict) => Dictionary = dict;
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public IEnumerator<S> GetEnumerator() => Dictionary.Select(e => e.Value).GetEnumerator();
        }

        public IEnumerable<KeyValuePair<int, T>> KeyValuePairs { get => new SparseKeyValuePairs<T>(Dictionary); }

        public IEnumerable<int> Keys { get => new SparseKeys<T>(Dictionary); }

        public IEnumerable<T> Values { get => new SparseValues<T>(Dictionary); }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Length; i++) yield return this[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();



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
            if (index >= Length) Length = index + 1;

            foreach ( var i in Dictionary.Keys.OrderBy(x => -x).TakeWhile(x => x <= index) )
            {
                Dictionary[i + 1] = Dictionary[i];
                Dictionary.Remove(i);
            }


        }

        public bool Remove(T item)
        {
            if (!Dictionary.ContainsValue(item)) return true;

            RemoveAt(IndexOf(item));

            return true;
        }

        public void RemoveAt(int index)
        {
            if (index >= Length) throw new ArgumentOutOfRangeException("Index has to be less than the length.");

            Dictionary.Remove(index);

            foreach ( var i in Dictionary.Keys.OrderBy(x => x).SkipWhile(x => x <= index) )
            {
                Dictionary[i - 1] = Dictionary[i];
                Dictionary.Remove(i);
            }

            Length -= 1;
        }


        public int GetHashCode(IEqualityComparer comparer)
        {
            throw new NotImplementedException();
        }

    }
}
