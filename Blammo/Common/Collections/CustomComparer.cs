using System;
using System.Collections.Generic;

namespace Agent.Common.Collections
{
    public class CustomComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comparer;
        private readonly Func<T, int> _getHashCode;

        public CustomComparer(Func<T, T, bool> comparer, Func<T, int> getHashCode)
        {
            _comparer = comparer;
            _getHashCode = getHashCode;
        }

        public bool Equals(T x, T y)
        {
            return _comparer(x, y);
        }

        public int GetHashCode(T obj)
        {
            return _getHashCode(obj);
        }
    }
}