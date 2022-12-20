using AoC2022.days;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.util
{
    public static class EnumerableGeneration
    {
        /// <summary>
        /// Returns sequence from start (included) to end (excluded)
        /// </summary>
        public static IEnumerable<int> FromTo(int start, int end) => FromTo(start, end, false);

        /// <summary>
        /// Returns sequence from start (included) to end (excluded or indcluded by choise)
        /// </summary>
        public static IEnumerable<int> FromTo(int start, int end, bool includeEnd)
        {
            if (start == end) yield break;
            int step = end - start > 0 ? 1 : -1;
            int current = start;
            if (includeEnd) end += step;
            if (includeEnd && step == 0)
                yield return end;
            while (current != end)
            {
                yield return current;
                current = current + step;
            }
        }

        public static IEnumerable<T> Sequence<T>(int count, Func<T> func)
        {
            for (int i = 0; i < count; i++)
            {
                yield return func();
            }
        }

        public static IEnumerable<IEnumerable<T>> Sequence2D<T>(int xSize, int ySize, Func<T> func) => Sequence(xSize, () => Sequence(ySize, func));
   


        public static IEnumerable<State> InfiniteByValueTransformation<State>(State start, Func<State, State> func)
        {
            var current = start;
            yield return current;
            while (true)
            {
                current = func(current);
                yield return current;
            }
        }
    }

    public static class IEnumerableExtentions
    { 

        public static IEnumerable<TResult[]> GroupSlide<TResult>(this IEnumerable<TResult> items, int groupSize)
        {
            var enumerator = items.GetEnumerator();
            Queue<TResult> currentView = new Queue<TResult>();
            for (int i = 0; i < groupSize; i++)
            {
                if (enumerator.MoveNext())
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


        public static void DoForEach<T>(this IEnumerable<T> items, Action<T> action) => items.DoForEach((t,_) => action(t));
        public static void DoForEach<T>(this IEnumerable<T> items, Action<T,int> action)
        {
            var enumerator = items.GetEnumerator();
            int i = 0;
            while (enumerator.MoveNext())
            {
                action(enumerator.Current, i++);
            }
        }

        public static IEnumerable<T> WrapInEnumerable<T>(this T obj)
        {
            yield return obj;
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

        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> items)
        {
            var enumerator = items.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var innerEnumerator = enumerator.Current.GetEnumerator();
                while (innerEnumerator.MoveNext())
                {
                    yield return innerEnumerator.Current;
                }
            }
        }

        public static T[][] ToArray2D<T>(this IEnumerable<IEnumerable<T>> items) => items.Select(e => e.ToArray()).ToArray();
        


        public static string ToFormmatedString(this IEnumerable<IEnumerable<char>> chars) => chars.Select(l => l.Aggregate("", (p, c) => p + c)).Aggregate((p, c) => p + "\n" + c);

        public static IEnumerable<IEnumerable<T>> SwapInnerWithOuter<T>(this IEnumerable<IEnumerable<T>> values)
        {
            var enumerators = values.Select(x=> x.GetEnumerator()).ToList();
            while(enumerators.All(e => e.MoveNext()))
            {
                yield return enumerators.Select(e => e.Current);
            }
        }
        
        public static IEnumerable<T> Unique<T>(this IEnumerable<T> items)
        {
            HashSet<T> set = new HashSet<T>(items);
            return set;
        }

        public static IEnumerable<T> AsRepeatingSequence<T>(this IEnumerable<T> values) => RepeatingSequenceFromList(values.ToList());

        private static IEnumerable<T> RepeatingSequenceFromList<T>(List<T> values)
        {
            var enumerator = values.GetEnumerator();
            while (true)
            {
                if (enumerator.MoveNext()) yield return enumerator.Current;
                else
                {
                    enumerator.Dispose();
                    enumerator = values.GetEnumerator();
                }
            }
        } 

        public static bool TryGetFirst<T>(this IEnumerable<T> values, [MaybeNullWhen(false)] out T first)
        {
            var enumerator = values.GetEnumerator();
            if (enumerator.MoveNext())
            {
                first = enumerator.Current;
                return true;
            }
            first = default;
            return false;
        }
    }
}
