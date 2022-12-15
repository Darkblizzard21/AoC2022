using AoC2022.util;
using System.Numerics;

namespace AoC2022.days
{
    internal struct IntRange
    {
        public int start = 0;
        public int end = 0;

        public IntRange(int start, int end)
        {
            if (end < start)
                Console.WriteLine("ERROR");
            this.start = start;
            this.end = end;
        }

        public int Length => end - start + 1;
        public bool Contains(int i) => start <= i && i <= end;
        public bool IsMergable(IntRange other) => start - 1 <= other.end && other.start - 1 <= end;
        public IntRange Merge(IntRange other) => new IntRange(Math.Min(start, other.start), Math.Max(end, other.end));

        public bool CanBeUnified(IntRange other) => CanBeUnified(other.start, other.end);
        public bool CanBeUnified(int min, int max) => min <= end || start <= max;
        public IntRange Union(IntRange other) => Union(other.start, other.end);
        public IntRange Union(int min, int max) => new IntRange(Math.Clamp(start, min, max), Math.Clamp(end, min, max));
    }

    internal static class IntRangeExtensions{
        public static IEnumerable<IntRange> Merge(this IEnumerable<IntRange> ranges)
        {
            var enumerator = ranges.OrderByDescending(r => r.end).OrderBy(r => r.start).GetEnumerator();
            if (!enumerator.MoveNext()) yield break;
            var current = enumerator.Current;
            while (enumerator.MoveNext())
            {
                var next = enumerator.Current;
                if (current.IsMergable(next))
                {
                    current = current.Merge(next);
                }
                else
                {
                    yield return current;
                    current = next;
                }
            }
            yield return current;
        }

        public static int OccupationCount(this IEnumerable<IntRange> ranges) => ranges.Aggregate(0, (p, c) => p + c.Length);
        public static bool ContainsInt(this IEnumerable<IntRange> ranges, int i) => ranges.Any(r => r.Contains(i));
    }


    public static class Day15
    {

        public static void solve()
        {
            InputProvider inputProvider = new InputProvider("day15");
            var inputType = InputType.Input;
            var signals = inputProvider.Get(inputType).Split("\r\n").Select(s =>
            {
                var byComma = s.Substring(12).Split(",");
                var sX = int.Parse(byComma[0]);
                var sY = int.Parse(byComma[1].Substring(3, byComma[1].IndexOf(':') - 3));
                var bX = int.Parse(byComma[1].Substring(byComma[1].LastIndexOf('=')+1));
                var bY = int.Parse(byComma[2].Substring(3));
                return (s: new IntVector2(sX, sY), b: new IntVector2(bX, bY));
            }).ToArray();

            int lineToCheck = inputType == InputType.Input ? 10 : 2000000;
            var occupied = signals.Select(t => (s: t.s, distance: t.b.ManthattenDistanceTo(t.s)))
                .SelectMany(t =>
                {
                    int remaingDistance = t.distance - Math.Abs(t.s.y - lineToCheck);
                    if (remaingDistance < 0) return Enumerable.Empty<IntRange>();
                    return (new IntRange(t.s.x - remaingDistance, t.s.x + remaingDistance)).WrapInEnumerable();
                }).Merge().ToArray();
            var lineSignalsAndBeacons = signals.SelectMany(t => t.s.WrapInEnumerable().Append(t.b)).Unique().Where(v => (v.y == lineToCheck && occupied.ContainsInt(v.x))).ToArray();
            var blockedPositions = occupied.OccupationCount() - lineSignalsAndBeacons.Count();
            Console.WriteLine("Blocked Positions: " + blockedPositions);

            // Second
            int max = inputType == InputType.Sample ? 20 : 4000000;
            var tuningFrequency = IEnumerableExtentions.FromTo(0, max + 1)
                .Select(ltC =>
                {
                    if (ltC % 100000 == 0) Console.WriteLine(ltC);
                    return (i: ltC, r: signals.Select(t => (s: t.s, distance: t.b.ManthattenDistanceTo(t.s)))
                    .SelectMany(t =>
                    {
                        int remaingDistance = t.distance - Math.Abs(t.s.y - ltC);
                        if (remaingDistance < 0) return Enumerable.Empty<IntRange>();
                        return (new IntRange(t.s.x - remaingDistance, t.s.x + remaingDistance)).WrapInEnumerable();
                    })
                    .Merge()
                    .Where(r => r.CanBeUnified(0, max + 1))
                    .Select(r => r.Union(0, max + 1)).ToArray());
                })
                .Where(t => t.r.Length > 1 || t.r[0].start == 1 || t.r[0].end == max - 1)
                .Select(t => new IntVector2(t.r[1].start - 1, t.i))
                .Select(b => b.x * 4000000L + b.y);
            tuningFrequency.DoForEach(tf => Console.WriteLine("Posible frequency: " + tf));
        }
    }
}
