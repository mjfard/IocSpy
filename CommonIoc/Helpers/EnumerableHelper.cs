using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.CommonIoc.Helpers
{
    public static class EnumerableHelper
    {
        public static string AggregateToString<T>(this IEnumerable<T> list, string seperator)
        {
            return list.Aggregate("", (str, item) => str + (string.IsNullOrEmpty(str) ? "" : seperator) + item.ToString());
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T obj in source)
                action(obj);
        }
    }
}
