using System.Collections.ObjectModel;

namespace Agent.Core.Misc
{
    public class CircularList<T> : Collection<T>
    {
        public CircularList(uint maxCount)
        {
            MaxCount = maxCount;
        }

        public uint MaxCount { get; private set; }

        protected override void InsertItem(int index, T item)
        {
            while (Count > MaxCount)
            {
                RemoveAt(0);
            }

            base.InsertItem(index, item);
        }
    }
}
