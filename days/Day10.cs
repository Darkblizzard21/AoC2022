using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.days
{
    public static class Day10
    {
        private enum Instruction
        {
            Noop,
            AddX
        }

        public static void solve()
        {
            InputProvider inputProvider = new InputProvider("day10");
            var instructionTrace = inputProvider.Get(Type.Input).Split("\r\n").Select(s =>
                {
                    var split = s.Split(" ");
                    return (split[0] switch
                    {
                        "addx" => Instruction.AddX,
                        _ => Instruction.Noop
                    },
                    split.Length == 2 ? Int32.Parse(split[1]) : 0);
                })
                .FlatAggregateTrace(1, (p, c) => c.Item1 switch
                {
                    Instruction.AddX => new List<int>() { p, p + c.Item2 },
                    _ => new List<int>() { p }
                })
                .Prepend(1).ToList();
            var signalStrength = instructionTrace
                .ZipWithIndices(startValue: 1)
                .Where(t => (t.index - 20) % 40 == 0 && t.index <= 220)
                .Select(t => t.index * t.value)
                .Sum();
            Console.WriteLine(signalStrength);

            char[][] display = IEnumerableExtentions.FromTo(0,6).Select(_ => IEnumerableExtentions.FromTo(0,40).Select(_ => '.').ToArray()).ToArray();

            instructionTrace.Chunk(40).ZipWithIndices().DoForEach(t => t.value.ZipWithIndices().DoForEach(v =>
            {
                int row = t.index;
                int cycle = v.index;
                int register = v.value;
                if (6 <= row) return;
                if (register - 1 <= cycle && cycle <= register + 1) display[row][cycle] = '#';
            }));
            Console.WriteLine(display.Aggregate("", (p,c) => p + "\n" + c.Aggregate("", (p,c) => p + c)));
        }
    }
}
