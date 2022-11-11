using System;
using System.Collections.Generic;
using System.Linq;

namespace Dasher
{
    public static class EnumerableExtensions
    {
        public static T GetRandomItem<T>(this IEnumerable<T> collection, System.Random random)
        {
            if (collection == null)
                throw new ArgumentNullException("Cannot get random element in null collection");
            int count = collection.Count();
            if (count == 0)
                throw new ArgumentException("Cannot get random element in empty collection");
            return collection.Skip(random.Next() % count).First();
        }

        public static void CreateOrAdd<T1>(this Dictionary<T1, int> dict, T1 key, int value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] += value;
            }
            else
            {
                dict.Add(key, value);
            }
        }
    }
}