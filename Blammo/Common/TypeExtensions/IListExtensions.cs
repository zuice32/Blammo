using System;
using System.Collections.Generic;

namespace Agent.Common.TypeExtensions
{
    public static class IListExtensions
    {
        /// <summary>
        ///     Safely instantiate objects as they are added to the list
        /// </summary>
        public static void SafeAdd<T>(this IList<T> list, Func<T> create, Action<Exception> handleException)
        {
            try
            {
                list.Add(create());
            }
            catch (Exception e)
            {
                handleException(e);
            }
        }

        /// <summary>
        ///     Safely instantiate list of objects
        /// </summary>
        public static void SafeAddRange<T>(this IList<T> list, IEnumerable<T> items , Action<Exception> handleException)
        {
            try
            {
                foreach (T item in items)
                {
                    list.Add(item);
                }
            }
            catch (Exception e)
            {
                handleException(e);
            }
        }
    }
}