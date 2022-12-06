using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.days
{
    public static class EnumeratorExtentions
    {
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

        public static IEnumerable<(int, TResult)> Enumerate<TResult>(this IEnumerable<TResult> items)
        {
            var enumerator = items.GetEnumerator();
            int i = 0;
            while (enumerator.MoveNext())
            {
                yield return (i++, enumerator.Current);
            }
        }
    }
}
