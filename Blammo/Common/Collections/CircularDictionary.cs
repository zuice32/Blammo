using System.Collections;
using System.Collections.Generic;

namespace Agent.Common.Collections
{
    /// <summary>
    /// Circular buffer implemented as a dictionary.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class CircularDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> _internalDictionary =
            new Dictionary<TKey, TValue>();

        private readonly List<TKey> _keyList = new List<TKey>();

        private readonly object _syncLock = new object();

        public uint MaxItems { get; set; }

        public CircularDictionary(uint maxItems)
        {
            MaxItems = maxItems;
        }

        #region Implementation of IEnumerable

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _internalDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<KeyValuePair<TKey,TValue>>

        public void Add(KeyValuePair<TKey, TValue> item)
        {
//            ((ICollection<KeyValuePair<TKey, TValue>>) _internalDictionary).Add(item);
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<TKey, TValue>>) _internalDictionary).Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return
                ((ICollection<KeyValuePair<TKey, TValue>>) _internalDictionary).Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>) _internalDictionary).CopyTo(array,
                                                                                   arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
//            return ((ICollection<KeyValuePair<TKey, TValue>>) _internalDictionary).Remove(item);
        }

        public int Count
        {
            get { return ((ICollection<KeyValuePair<TKey, TValue>>) _internalDictionary).Count; }
        }
        public bool IsReadOnly
        {
            get
            {
                return
                    ((ICollection<KeyValuePair<TKey, TValue>>) _internalDictionary).IsReadOnly;
            }
        }

        #endregion

        #region Implementation of IDictionary<TKey,TValue>

        public bool ContainsKey(TKey key)
        {
            return _internalDictionary.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            lock (_syncLock)
            {
                _internalDictionary.Add(key, value);

                _keyList.Add(key);

                EnforceMaxCacheSize();
            }
        }

        private void EnforceMaxCacheSize()
        {
            while (_keyList.Count > MaxItems)
            {
                _internalDictionary.Remove(_keyList[0]);

                _keyList.RemoveAt(0);
            }
        }

        public bool Remove(TKey key)
        {
            lock (_syncLock)
            {
                bool removed = _internalDictionary.Remove(key);

                if (removed)
                {
                    _keyList.Remove(key);
                }

                return removed;
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _internalDictionary.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get { return _internalDictionary[key]; }
            set { _internalDictionary[key] = value; }
        }

        public ICollection<TKey> Keys
        {
            get { return _internalDictionary.Keys; }
        }
        public ICollection<TValue> Values
        {
            get { return _internalDictionary.Values; }
        }

        #endregion
    }
}