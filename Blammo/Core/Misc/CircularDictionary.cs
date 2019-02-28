using System.Collections;
using System.Collections.Generic;

namespace Agent.Core.Misc
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
            this.MaxItems = maxItems;
        }

        #region Implementation of IEnumerable

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return this._internalDictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<KeyValuePair<TKey,TValue>>

        public void Add(KeyValuePair<TKey, TValue> item)
        {
//            ((ICollection<KeyValuePair<TKey, TValue>>) _internalDictionary).Add(item);
            this.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<TKey, TValue>>) this._internalDictionary).Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return
                ((ICollection<KeyValuePair<TKey, TValue>>) this._internalDictionary).Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>) this._internalDictionary).CopyTo(array,
                                                                                   arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.Remove(item.Key);
//            return ((ICollection<KeyValuePair<TKey, TValue>>) _internalDictionary).Remove(item);
        }

        public int Count
        {
            get { return ((ICollection<KeyValuePair<TKey, TValue>>) this._internalDictionary).Count; }
        }
        public bool IsReadOnly
        {
            get
            {
                return
                    ((ICollection<KeyValuePair<TKey, TValue>>) this._internalDictionary).IsReadOnly;
            }
        }

        #endregion

        #region Implementation of IDictionary<TKey,TValue>

        public bool ContainsKey(TKey key)
        {
            return this._internalDictionary.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            lock (this._syncLock)
            {
                this._internalDictionary.Add(key, value);

                this._keyList.Add(key);

                this.EnforceMaxCacheSize();
            }
        }

        private void EnforceMaxCacheSize()
        {
            while (this._keyList.Count > this.MaxItems)
            {
                this._internalDictionary.Remove(this._keyList[0]);

                this._keyList.RemoveAt(0);
            }
        }

        public bool Remove(TKey key)
        {
            lock (this._syncLock)
            {
                bool removed = this._internalDictionary.Remove(key);

                if (removed)
                {
                    this._keyList.Remove(key);
                }

                return removed;
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return this._internalDictionary.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get { return this._internalDictionary[key]; }
            set { this._internalDictionary[key] = value; }
        }

        public ICollection<TKey> Keys
        {
            get { return this._internalDictionary.Keys; }
        }
        public ICollection<TValue> Values
        {
            get { return this._internalDictionary.Values; }
        }

        #endregion
    }
}