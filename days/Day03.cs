using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.days
{
    static class Day03
    {
        public static void solve()
        {
            InputProvider inputProvider = new InputProvider("day03");
            // FirstPart
            var rucksackPriority = inputProvider.GetInput()
                .Split("\r\n")
                .Select(s => s.ToCharArray())
                .Select(a => a.Take(a.Length / 2).Intersect(a.Skip(a.Length / 2)).First())
                .Select(c => ToPriority(c))
                .Sum();
            Console.WriteLine("First Part solution: " + rucksackPriority);

            // Second Part
            var groupPriority = inputProvider.GetInput()
                .Split("\r\n")
                .Chunk(3)
                .Select(chunk => chunk.Select(s => s.ToCharArray().AsEnumerable()).Aggregate((a, b) => a.Intersect(b)).First())
                .Select(c => ToPriority(c))
                .Sum();
            Console.WriteLine("Second Part solution: " + groupPriority);

        }

        static int ToPriority(char c) => c < 95 ? c - 65 + 27 : c - 96;
    }
}
