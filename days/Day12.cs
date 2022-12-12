using AoC2022.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.days
{
    public static class Day12
    {
        private class Tile
        {
            private const int offset = 'a';
            public Tile(char name, int x, int y)
            {
                Name = name;
                height = name - offset;
                if (isStart)
                    height = 'a' - offset;
                if (isEnd)
                    height = 'z' - offset;
                this.x = x;
                this.y = y;
            }

            public char Name { get; private set; }
            public int height { get; private set; }
            public int x { get; private set; }
            public int y { get; private set; }

            public bool isStart { get => Name == 'S'; }
            public bool isEnd { get => Name == 'E'; }
            public int minRemainingStepsToMax { get => 'z' - offset - height; }
            public bool canBeClimbed(Tile other) => other.height - height <= 1;
        }
        public static void solve()
        {
            InputProvider inputProvider = new InputProvider("day12");
            Tile[][] map = inputProvider.Get(InputType.Input).Split("\r\n").Select((s, y) => s.ToCharArray().Select((c, x) => new Tile(c, x, y)).ToArray()).ToArray();
            Tile start = map.SelectMany(x => x).First(t => t.isStart);
            var res = AStar.FindPath(
                    startNode: start,
                    neigbours: node => map.DirectNeighboursFor(node.y, node.x).Where(other => node.canBeClimbed(other)),
                    isDestination: node => node.isEnd,
                    heuristic: node => node.minRemainingStepsToMax,
                    costFromTo: (_, _) => 1
                );
            PrintTrace(map, res.trace.ToList());
            Console.WriteLine("Shortest from start: " + res.cost);

            var senicRoute = AStar.FindPath(
                    startNode: map.SelectMany(l => l).First(t => t.isEnd),
                    neigbours: node => map.DirectNeighboursFor(node.y, node.x).Where(other => other.canBeClimbed(node)),
                    isDestination: node => node.height == 0,
                    heuristic: node => node.height,
                    costFromTo: (_, _) => 1
                );
            PrintTrace(map, senicRoute.trace.ToList());
            Console.WriteLine("Scenic Rout length: " + senicRoute.cost);
        }

        private static void PrintTrace(Tile[][] map, List<Tile> trace)
        {
            char[][] cMap = map.Select(s => s.Select(t => t.Name).ToArray()).ToArray();
            var first = trace.First();
            cMap[first.y][first.x] = '#';

            trace.GroupSlide(2).DoForEach(t =>
            {
                var arr = t.ToArray();
                var current = arr[0];
                var next = arr[1];
                int xDist = current.x - next.x;
                int yDist = current.y - next.y;
                cMap[next.y][next.x] = xDist switch
                {
                    0 => yDist switch
                    {
                        1 => 'v',
                        -1 => '^',
                        _ => throw new NotImplementedException()
                    },
                    1 => '>',
                    -1 => '<',
                    _ => throw new NotImplementedException()
                };
            });
            Console.WriteLine(cMap.ToFormmatedString() + "\n");
        }
    }
}
