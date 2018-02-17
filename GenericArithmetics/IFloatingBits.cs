using System;
using System.Collections.Generic;
using System.Text;

namespace GenericArithmetics
{
    public interface IFloatingBits<T>
        : IEquatable<IFloatingBits<T>>,
          IComparable<IFloatingBits<T>>
    {
    }
}
