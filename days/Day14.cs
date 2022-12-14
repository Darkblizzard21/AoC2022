using AoC2022.util;
using System.Drawing.Imaging;
using System.Numerics;

namespace AoC2022.days
{
    internal enum State
    {
        Rock,
        Air,
        Sand
    }
    internal class SandPile
    {
        
        private State[][] map;
        private (int x, int y) offset;
        public SandPile(string input) {
            List<List<(int x,int y)>> lines = input.Split("\r\n")
                .Select(l => l.Split(" -> ")
                    .Select(v => v.Split(',')
                        .Select(Int32.Parse)
                        .ToArray())
                    .Select(a => (x: a[0], y: a[1]))
                    .ToList())
                .ToList();

            // create map
            (int minX, int minY, int maxX, int maxY) = lines.Flatten().Append((x: 500,y: 0))
                .Aggregate(
                    (minX: Int32.MaxValue, minY: Int32.MaxValue, maxX: Int32.MinValue, maxY: Int32.MinValue),
                    (p, c) => (Math.Min(p.minX, c.x), Math.Min(p.minY, c.y), Math.Max(p.maxX, c.x), Math.Max(p.maxY, c.y)));
            offset = (minX, minY);
            map = ArrayExtensions.New2DWithDefault(maxX-minX+1, maxY - minY+1, () => State.Air);

            // Fill map
            var t = lines.SelectMany(line => line.GroupSlide(2))
                .SelectMany(l => l[0].x == l[1].x ?
                    IEnumerableExtentions.FromTo(l[0].y, l[1].y, true).Select(y => (x: l[0].x, y: y)) :
                    IEnumerableExtentions.FromTo(l[0].x, l[1].x, true).Select(x => (x: x, y: l[0].y))
                    ).ToArray();
            t.DoForEach(t => map[t.x-offset.x][t.y-offset.y] = State.Rock);
            Console.WriteLine("Created SandPile");
            PrintState();
        }

        public SandPileWithFloor WithFloor()
        {
            return new SandPileWithFloor(map, offset);
        }

        public void PrintState() => Console.WriteLine(map.Select(y => y.Select(s => s switch
            {
                State.Rock => '#',
                State.Air => '.',
                State.Sand => 'o',
                _ => throw new ArgumentException()
            })).SwapInnerWithOuter().ToFormmatedString());

        /// <summary>
        /// Simulates a sand dropping
        /// </summary>
        /// <param name="x">start x</param>
        /// <param name="y">start y</param>
        /// <returns>if the sand setteld or droped to infinity</returns>
        public bool DropSand(int x = 500, int y = 0)
        {
            x = x - offset.x;
            y = y - offset.y;
            while (true)
            {
                if (y + 1 >= map[0].Length) return false;
                if (map[x][y + 1] == State.Air)
                {
                    y++;
                    continue;
                }
                if (x - 1 < 0) return false;
                if (map[x - 1][y + 1] == State.Air)
                {
                    y++;
                    x--;
                    continue;
                }
                if (x + 1 >= map.Length) return false;
                if (map[x + 1][y + 1] == State.Air)
                {
                    y++;
                    x++;
                    continue;
                }
                break;
            }
            map[x][y] = State.Sand;
            return true;
        }
      
        public int SandCount() => map.Flatten().Where(s => s == State.Sand).Count();
    }

    internal class SandPileWithFloor
    {

        private State[][] map;
        private List<(int x, int y)> outSideSand = new List<(int x, int y)>();
        private (int x, int y) offset;
        private int floor;

        public SandPileWithFloor(State[][] map, (int x, int y) oldOffset)
        {
            floor = map[0].Length + 1;
            int additonalXOffset = ((floor - map.Length + 4) / 2 + 1) * 3;
            int newX = map.Length + additonalXOffset * 2;
            offset = (x: oldOffset.x - additonalXOffset, y: oldOffset.y);

            this.map = ArrayExtensions.New2DWithDefault(newX, floor, () => State.Air);
            map.DoForEach((l, x) => l.DoForEach((v, y) => this.map[x+additonalXOffset][y] = v));
        }
        
        public bool DropSandWithFloor(int x = 500, int y = 0)
        {
            x = x - offset.x;
            y = y - offset.y;
            if (map[x][y] != State.Air) return false;

            bool isAir(int x, int y)
            {
                if (y >= floor) return false;
                if (x < 0 || x >= map.Length) return !outSideSand.Any(t => t.x == x && t.y == y);
                return map[x][y] == State.Air;
            }

            while (true)
            {
                if (isAir(x, y + 1))
                {
                    y++;
                    continue;
                }
                if ((isAir(x - 1, y + 1)))
                {
                    y++;
                    x--;
                    continue;
                }
                if ((isAir(x + 1, y + 1)))
                {
                    y++;
                    x++;
                    continue;
                }
                break;
            }
            if (0 <= x && x < map.Length)
            {

                map[x][y] = State.Sand;
            }
            else
            {
                outSideSand.Add((x: x, y: y));
            }
            return true;

        }

        public void PrintState() => Console.WriteLine(map.Select(y => y.Select(s => s switch
        {
            State.Rock => '#',
            State.Air => '.',
            State.Sand => 'o',
            _ => throw new ArgumentException()
        })).SwapInnerWithOuter().ToFormmatedString()
            + IEnumerableExtentions.Generate(map.Length, () => '#').Aggregate("\n", (p, c) => p + c));
        public int SandCount() => map.Flatten().Where(s => s == State.Sand).Count() + outSideSand.Count();
    }
    public static class Day14
    {
        public static void solve()
        {
            InputProvider inputProvider = new InputProvider("day14");
            SandPile sandPile = new SandPile(inputProvider.Get(InputType.Input));
            // Frist
            while (sandPile.DropSand()) ; sandPile.PrintState();
            var sandCountNoFloor = sandPile.SandCount();
            Console.WriteLine("Sandcount NoFloor: "+ sandCountNoFloor);

            // Second
            SandPileWithFloor sandPileWithFloor = sandPile.WithFloor();
            while (sandPileWithFloor.DropSandWithFloor()) ; sandPileWithFloor.PrintState();
            var sandCountFloor = sandPileWithFloor.SandCount();
            Console.WriteLine("Sandcount Floor: " + sandCountFloor);
        }
    }
}
