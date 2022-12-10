using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.days
{
    public static class IEnumerableExtentions
    {
        /// <summary>
        /// Returns sequence from start (included) to end (excluded)
        /// </summary>
        public static IEnumerable<int> FromTo(int start, int end)
        {
            if(start == end) yield break;
            int step = end - start > 0 ? 1 : -1;
            int current = start;
            while (current != end) {
                yield return current;
                current = current + step;
            }
        }

        public static IEnumerable<TResult[]> GroupSlide<TResult>(this IEnumerable<TResult> items, int groupSize)
        {
            var enumerator = items.GetEnumerator();
            Queue<TResult> currentView = new Queue<TResult>();
            for (int i = 0; i < groupSize; i++)
            {
                if(enumerator.MoveNext())
                {
                    currentView.Enqueue(enumerator.Current);
                }
                else
                {
                    yield break;
                }
            }
            yield return currentView.ToArray();

            while (enumerator.MoveNext())
            {
                currentView.Enqueue(enumerator.Current);
                currentView.Dequeue();
                yield return currentView.ToArray();
            }
        }



        public static IEnumerable<(int index, TResult value)> ZipWithIndices<TResult>(this IEnumerable<TResult> items, int startValue = 0)
        {
            var enumerator = items.GetEnumerator();
            int i = startValue;
            while (enumerator.MoveNext())
            {
                yield return (index: i++, value: enumerator.Current);
            }
        }

        public static IEnumerable<TTraceState> FlatAggregateTrace<TInput, TTraceState>(
                this IEnumerable<TInput> items,
                TTraceState seed,
                Func<TTraceState, TInput, List<TTraceState>> func)
        {

            var enumerator = items.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var l = func(seed, enumerator.Current);
                foreach (var v in l)
                    yield return v;
                seed = l.Last();
            }
        }

        public static void DoForEach<T>(this IEnumerable<T> values, Action<T> action)
        {
            var enumerator = values.GetEnumerator();
            while (enumerator.MoveNext())
            {
                action(enumerator.Current);
            }
        }
    }
}
