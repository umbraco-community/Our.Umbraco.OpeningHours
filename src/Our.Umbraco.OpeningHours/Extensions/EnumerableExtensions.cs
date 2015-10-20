using System;
using System.Collections.Generic;
using System.Linq;

namespace Our.Umbraco.OpeningHours.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<TResult> Compact<T, TKey, TResult>(this IEnumerable<T> source, Func<T, TKey> keySelector, Func<TKey, IEnumerable<T>, TResult> resultSelector)
        {
            if (!source.Any())
                yield break;

            var comparer = EqualityComparer<TKey>.Default;

            TKey previousKey = keySelector(source.First());

            List<T> group = new List<T>() { source.First() };

            foreach (var item in source.Skip(1))
            {
                TKey currentKey = keySelector(item);
                if (!comparer.Equals(previousKey, currentKey))
                {
                    yield return resultSelector(previousKey, group);
                    group = new List<T>();
                }
                group.Add(item);
                previousKey = currentKey;
            }
            if (group.Any())
            {
                yield return resultSelector(previousKey, group);
            }
        }
    }
}