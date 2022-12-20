using AoC2022.util;
using System.Linq;

namespace AoC2022.days
{
    public static class Day20
    {
        private class MoveableList
        {
            public struct IndexedValue
            {
                public int index;
                public long value;

                public IndexedValue(int index, long value)
                {
                    this.index = index;
                    this.value = value;
                }
                public override string ToString() => $"I:{index}, V:{value}";
            }

            public List<IndexedValue> values;
            public int length;
            public MoveableList(IEnumerable<int> baseList)
            {
                values = baseList.Select((v, i) => new IndexedValue(i, v)).ToList();
                length = values.Count;
            }
            public MoveableList(IEnumerable<long> baseList) {
                values = baseList.Select((v, i) => new IndexedValue(i, v)).ToList();
                length = values.Count;
            }

            public MoveableList move(int index)
            {
                var startIndex = values.FindIndex(0, iv => iv.index == index);
                var item = values[startIndex];
                var value = item.value;
                var moveDir = Math.Sign(value);
                var movement = Math.Abs(value) % (length - 1);
                if (movement == 0) return this;
                values.RemoveAt(startIndex);
                var newIndex = (startIndex + movement * moveDir) % (length - 1);
                if (newIndex <= 0) newIndex += values.Count;
                values.Insert((int)newIndex, item);

                return this;
            }

            public long coordinateSum() => EnumerableGeneration.Repeat(3, values.FindIndex(vi => vi.value == 0)).Select((zeroIndex, i) => valueAt(zeroIndex + (i + 1) * 1000)).Sum();

            public long valueAt(int index) => values[index % length].value;

            public override string ToString() => values.Aggregate("",(p, c) => $"{p},{c.value}").Substring(1);
        }

        public static void solve()
        {
            InputProvider inputProvider = new InputProvider("day20");
            var numbers = inputProvider.Get(InputType.Input).Split("\r\n").Select(int.Parse).ToList();
            var falseSum = EnumerableGeneration.FromTo(0, numbers.Count)
                .Aggregate(new MoveableList(numbers), (p, c) => p.move(c))
                .coordinateSum();
            Console.WriteLine("What is the sum of the three numbers that form the grove coordinates? FLASE METHOD: " + falseSum);

            // Actual Decode
            var cooridnateSum = EnumerableGeneration.Sequence(10, () => EnumerableGeneration.FromTo(0, numbers.Count))
                .Flatten()
                .Aggregate(new MoveableList(numbers.Select(i => i * 811589153L)), (p, c) => p.move(c))
                .coordinateSum();
            Console.WriteLine("What is the sum of the three numbers that form the grove coordinates? Right METHOD: " + cooridnateSum);
        }
    }
}
