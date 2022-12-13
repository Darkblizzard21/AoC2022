using AoC2022.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.days
{
    internal enum PacketType
    {
        List,
        Value,
        Invalid
    }

    internal enum Order
    {
        Unordered,
        Ordered,
        Equal
    }

    internal class Packet
    {
        public PacketType type = PacketType.Invalid;
        public int? value = null;
        public List<Packet>? list = null;

        private Packet(PacketType type, int? value = null, List<Packet>? list = null)
        {
            this.type = type;
            switch (type)
            {
                case PacketType.List:
                    this.list = list;
                    break;
                case PacketType.Value:
                    this.value = value;
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        public Order isLowerThan(Packet other) => type switch
        {
            PacketType.Value => other.type switch
            {
                PacketType.Value => value == other.value ? Order.Equal :
                                    value < other.value ? Order.Ordered : Order.Unordered,
                PacketType.List => compareLists(this.WrapInEnumerable(), other.list),
                _ => throw new ArgumentException()
            },
            PacketType.List => other.type switch
            {
                PacketType.Value => compareLists(list, other.WrapInEnumerable()),
                PacketType.List => compareLists(list, other.list),
                _ => throw new ArgumentException()
            },
            _ => throw new ArgumentException()
        };

        private static Order compareLists(IEnumerable<Packet> first, IEnumerable<Packet> second)
        {
            var firstEnumerator = first.GetEnumerator();
            var secondEnumerator = second.GetEnumerator();

            var firstHasNext = firstEnumerator.MoveNext();
            var secondHasNext = secondEnumerator.MoveNext();
            while(firstHasNext && secondHasNext)
            {
                var order = firstEnumerator.Current.isLowerThan(secondEnumerator.Current);
                if (order != Order.Equal) return order;

                // Move Next
                firstHasNext = firstEnumerator.MoveNext();
                secondHasNext = secondEnumerator.MoveNext();
            }

            if (!firstHasNext && secondHasNext) return Order.Ordered;
            if (firstHasNext && !secondHasNext) return Order.Unordered;
            return Order.Equal;
        }

        public static Packet fromString(string input)
        {
            if (input.StartsWith('['))
            {
                IEnumerable<string> remerge(IEnumerable<string> input)
                {
                    var enumerator = input.GetEnumerator();
                    int depth = 0;
                    string merged = "";
                    while (enumerator.MoveNext())
                    {
                        string cur = enumerator.Current;
                        var lastOpen = cur.LastIndexOf('[');
                        if (lastOpen != -1) { 
                            depth += 1 + lastOpen;
                        }
                        var firstClose = cur.IndexOf(']');
                        if (firstClose != -1)
                        {
                            depth -= cur.Length - firstClose;
                        }

                        merged += cur;
                        if (depth == 0)
                        {
                            yield return merged;
                            merged = "";
                        }
                        else
                        {
                            merged += ",";
                        }
                    }
                    if (depth != 0) 
                        Console.WriteLine("remerge failed");
                };

                var newList = remerge(input.Substring(1, input.Length - 2).Split(',').Where(s => s != "")).Select(s => fromString(s)).ToList();
                return new Packet(PacketType.List, list: newList);
            }
            else
            {
                return new Packet(PacketType.Value, value: Int32.Parse(input));
            }
        }

        override public string ToString() => type switch
        {
            PacketType.Value => "" + value,
            PacketType.List => "[" + list.Select(p => p.ToString()).DefaultIfEmpty("").Aggregate((p, c) => p + "," + c) + "]",
            _ => throw new ArgumentException()
        };
    }


    internal class PacketComparer : IComparer<Packet>
    {
        public int Compare(Packet? x, Packet? y) => x.isLowerThan(y) switch
                            {
                                Order.Unordered => 1,
                                Order.Equal => 0,
                                Order.Ordered => -1,
                                _ => throw new NotImplementedException()
                            };
    }

    public static class Day13
    {
        public static void solve()
        {
            InputProvider inputProvider = new InputProvider("day13");
            var input = inputProvider.Get(InputType.Input).Split("\r\n\r\n").Select(s => s.Split("\r\n").Select(s => Packet.fromString(s)).ToArray()).ToList();
            Console.WriteLine(input.Select(t => t.Select(p => p.ToString()).Aggregate((p, c) => p + "\n" + c)).Aggregate((p, c) => p + "\n\n" + c));

            // First 
            var sumOfIndecies = input.ZipWithIndices().Where(t => t.value[0].isLowerThan(t.value[1]) == Order.Ordered).Select(t => t.index + 1).Sum();
            Console.WriteLine("\nSum of ordered Indecies " + sumOfIndecies + "\n");

            // Ordered with Divider
            var ordered = input.SelectMany(t => t)
                .Append(Packet.fromString("[[2]]"))
                .Append(Packet.fromString("[[6]]"))
                .OrderBy(p => p, new PacketComparer()).ToArray();
            Console.WriteLine(ordered.Select(p => p.ToString()).Aggregate((p, c) => p + "\n" + c));
            var deviderSum = ordered.ZipWithIndices()
                .Where(t =>
                    {
                        var str = t.value.ToString();
                        return str.Equals("[[2]]") || str.Equals("[[6]]");
                    })
                .Select(t => t.index + 1)
                .Aggregate((p,c) => p * c);
            Console.WriteLine("\nDivider Product " + deviderSum);
        }
    }
}
