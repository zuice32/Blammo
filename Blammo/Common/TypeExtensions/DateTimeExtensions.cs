using System;

namespace Agent.Common.TypeExtensions
{
   public static class DateTimeExtensions
   {
       public static long GetJavascriptTicks(this DateTime source)
       {
           return (long)source.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
       }

       public static DateTime FromJavascriptTicks(long javascriptTicks)
       {
           return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(javascriptTicks);
       }

       public static DateTime ConvertUtcToEstLocalTime(this DateTime utcSource)
       {
//           if (utcSource.Kind != DateTimeKind.Utc) throw new ArgumentException("DateTime.Kind must be DateTimeKind.Utc");

           TimeZoneInfo estZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

           return TimeZoneInfo.ConvertTimeFromUtc(utcSource, estZoneInfo);
       }

       public static DateTime ConvertEstLocalTimeToUtc(this DateTime source)
       {
           TimeZoneInfo estZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

           if (source.Kind != DateTimeKind.Unspecified)
           {
               source = new DateTime(source.Ticks, DateTimeKind.Unspecified);
           }

           return TimeZoneInfo.ConvertTimeToUtc(source, estZoneInfo);
       }

       public static DateTime ConvertUtcToTimeZoneTime(this DateTime utcSource, string windowsTimeZoneId)
       {
           TimeZoneInfo estZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(windowsTimeZoneId);

           return TimeZoneInfo.ConvertTimeFromUtc(utcSource, estZoneInfo);
       }

       public static DateTime BinStart(this DateTime source, TimeSpan binSize)
       {
           long intervalFromBinStart = source.Ticks % binSize.Ticks;

           return source.AddTicks(-intervalFromBinStart);
       }

       public static DateTime BinEnd(this DateTime source, TimeSpan binSize)
       {
           long intervalFromBinStart = source.Ticks % binSize.Ticks;

           return source.AddTicks(binSize.Ticks - intervalFromBinStart);
       }

       public static DateTime UtcToday()
       {
            DateTime utcNow = DateTime.UtcNow;

           return new DateTime(utcNow.Year, utcNow.Month, utcNow.Day, 0, 0, 0, DateTimeKind.Utc);
       }

       public static DateTime DropSeconds(this DateTime source)
        {
            return new DateTime(source.Year,
                                source.Month,
                                source.Day,
                                source.Hour,
                                source.Minute,
                                0);
        }

       public static DateTime DropHours(this DateTime source)
        {
            return new DateTime(source.Year,
                                source.Month,
                                source.Day,
                                0,
                                0,
                                0);
        }

       /// <summary>
       /// If time is 10:49, returns 10:45-11:00.
       /// </summary>
       public static Range<DateTime> CurrentQuarterHour(this DateTime now)
       {
           DateTime start =
               new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0)
                   .AddMinutes(Math.Floor(now.Minute / 15.0) * 15);

           DateTime end = start.AddMinutes(15);

           return new Range<DateTime> { Min = start, Max = end };
       }

       /// <summary>
       /// If time is 10:49, returns 10:00-11:00.
       /// </summary>
       public static Range<DateTime> CurrentHour(this DateTime now)
       {
           DateTime start =
               new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);

           DateTime end = start.AddHours(1);

           return new Range<DateTime> { Min = start, Max = end };
       }

       /// <summary>
       /// If time is 2009/1/1 10:49, returns 2009/1/1 12:00AM - 2009/1/2 12:00AM.
       /// </summary>
        public static Range<DateTime> CurrentDay(this DateTime now)
        {
            DateTime start = new DateTime(now.Year, now.Month, now.Day);

            DateTime end = start.AddDays(1);

            return new Range<DateTime> {Min = start, Max = end};
        }

        public static Range<DateTime> PastDay(this DateTime now)
        {
            return new Range<DateTime> { Max = now, Min = now.AddDays(-1) };
        }

        public static Range<DateTime> DaysBeforeToday(this DateTime now, int numberOfDays)
        {
            DateTime start = new DateTime(now.Year, now.Month, now.Day).AddDays(-numberOfDays);

            DateTime end = start.AddDays(numberOfDays).AddSeconds(-1);

            return new Range<DateTime> {Min = start, Max = end};
        }


        public static Range<DateTime> CurrentWeek(this DateTime now)
        {
            DateTime sunday = GetSundayOfCurrentWeek(now);

            DateTime saturday = sunday.AddDays(7);

            return new Range<DateTime> {Min = sunday, Max = saturday};
        }

        public static Range<DateTime> PastWeek(this DateTime now)
        {
            return new Range<DateTime> { Max = now, Min = now.AddDays(-7) };
        }

        public static Range<DateTime> WeeksBeforeThisWeek(this DateTime now, int numberOfWeeks)
        {
            DateTime sunday = GetSundayOfCurrentWeek(now);

            DateTime lastSaturday = sunday.AddSeconds(-1);

            DateTime weeksAgo = sunday.AddDays(-7*numberOfWeeks);

            return new Range<DateTime> {Min = weeksAgo, Max = lastSaturday};
        }

        private static DateTime GetSundayOfCurrentWeek(DateTime now)
        {
            int dayOfWeek = (int) Enum.ToObject(typeof (DayOfWeek), now.DayOfWeek);

            return new DateTime(now.Year, now.Month, now.Day).AddDays(-dayOfWeek);
        }

        public static Range<DateTime> CurrentMonth(this DateTime now)
        {
            DateTime firstDayOfMonth = new DateTime(now.Year, now.Month, 1);

            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1);

            return new Range<DateTime> {Min = firstDayOfMonth, Max = lastDayOfMonth};
        }

        public static Range<DateTime> PastMonth(this DateTime now)
        {
            return new Range<DateTime> { Max = now, Min = now.AddMonths(-1) };
        }

        public static Range<DateTime> MonthsBeforeThisMonth(this DateTime now,
                                                            int numberOfMonths)
        {
            DateTime firstDay = new DateTime(now.Year, now.Month, 1).AddMonths(-numberOfMonths);

            DateTime lastDay = firstDay.AddMonths(numberOfMonths).AddSeconds(-1);

            return new Range<DateTime> {Min = firstDay, Max = lastDay};
        }

        public static Range<DateTime> CurrentQuarter(this DateTime now)
        {
            DateTime firstDayOfQuarter = GetFirstDayOfQuarter(now);

            DateTime lastDayOfQuarter = firstDayOfQuarter.AddMonths(3).AddSeconds(-1);

            return new Range<DateTime> {Min = firstDayOfQuarter, Max = lastDayOfQuarter};
        }

         public static Range<DateTime> PastQuarter(this DateTime now)
         {
             return new Range<DateTime> {Max = now, Min = now.AddMonths(-3)};
         }

        public static Range<DateTime> QuartersBeforeThisQuarter(this DateTime now,
                                                                int numberOfQuarters)
        {
            DateTime firstDayOfQuarter = GetFirstDayOfQuarter(now);

            DateTime start = firstDayOfQuarter.AddMonths(-3*numberOfQuarters);

            DateTime end = firstDayOfQuarter.AddSeconds(-1);

            return new Range<DateTime> {Min = start, Max = end};
        }

        private static DateTime GetFirstDayOfQuarter(DateTime now)
        {
            return new DateTime(now.Year, 1, 1).AddMonths(((int) Math.Floor((now.Month - 1)/
                                                                            (double) 3))
                                                          *3);
        }


        //parent is auth and child is 
        public static bool IsBetweenTwoDates(Range<DateTime> check, Range<DateTime> left, Range<DateTime> right)
        {
            return check.Min > left.Max && check.Max < right.Min;
        }

        public static Range<DateTime> ValidDateRangeInBetween(Range<DateTime> left, Range<DateTime> right)
        {
            return new Range<DateTime> { Min = left.Max, Max = right.Min };
        }

        public static Range<DateTime> CurrentYear(this DateTime now)
        {
            DateTime firstDayOfYear = new DateTime(now.Year, 1, 1);

            DateTime lastDayOfYear = new DateTime(now.Year, 12, 31);

            return new Range<DateTime> { Min = firstDayOfYear, Max = lastDayOfYear };
        }

    }
}