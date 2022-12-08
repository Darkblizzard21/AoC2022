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



        public static IEnumerable<(int index, TResult value)> Enumerate<TResult>(this IEnumerable<TResult> items)
        {
            var enumerator = items.GetEnumerator();
            int i = 0;
            while (enumerator.MoveNext())
            {
                yield return (index: i++, value: enumerator.Current);
            }
        }
    }
}
