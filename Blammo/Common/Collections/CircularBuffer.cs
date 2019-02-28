using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Agent.Common.Collections
{
   /// <summary>
	/// Elements are removed from the front of the collection for every element
	/// added once the MaxCount is reached.
	/// </summary>
    public class CircularBuffer<T> : ObservableCollection<T>
    {
        public CircularBuffer()
            : this(Int32.MaxValue)
        {
        }

        public CircularBuffer(uint maxCount)
        {
            MaxCount = maxCount;
        }

        /// <summary>
        /// Maximum number of elements allowed in the collection.
        /// Elements are removed from the front of the collection for every element
        /// added once the max. count is reached.
        /// </summary>
        public uint MaxCount { get; set; }

        //TODO: The way this is implemented may cause problems in some UI databinding (Infragistics grid on PVNetworkServiceConfig)?
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                while (Count > MaxCount)
                {
                    RemoveAt(0);
                }
            }

        }
    }
}
