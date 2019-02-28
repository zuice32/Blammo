
using System;

namespace Agent.Core.Misc
{
    public class Range<T> : IEquatable<Range<T>> where T : IEquatable<T>
    {
        public T Min{ get; set;}

        public T Max{ get; set;}

        public bool Equals(Range<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Min, Min) && Equals(other.Max, Max);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (Range<T>)) return false;
            return Equals((Range<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Min.GetHashCode() * 397) ^ Max.GetHashCode();
            }
        }

        public static bool operator ==(Range<T> left, Range<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Range<T> left, Range<T> right)
        {
            return !Equals(left, right);
        }
  
        public override string ToString()
        {
            return string.Format("Min:{0} Max:{1}", Min, Max);
        }
  }

    public enum DateTimeRangeMagnitude
    {
        Days,
        Weeks,
        Months,
        Quarters
    }

    public static class DateTimeRangeExtenstions
    {
        public static TimeSpan TimeSpan(this Range<DateTime> source)
        {
            return source.Max - source.Min;
        }

        public static DateTimeRangeMagnitude Magnitude(this Range<DateTime> source)
        {
            double totalDays = source.TimeSpan().TotalDays;

            if (totalDays < 14)
            {
                return DateTimeRangeMagnitude.Days;
            }

            if (totalDays < 60)
            {
                return DateTimeRangeMagnitude.Weeks;
            }

            if(totalDays < 230 )
            {
                return DateTimeRangeMagnitude.Months;
            }

            return DateTimeRangeMagnitude.Quarters;
        }
    }
}