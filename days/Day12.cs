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
            var res = aStar(map, start);
            PrintTrace(map, res.trace);
            Console.WriteLine("Shortest from start: " + res.steps);

            var senicRoute = aStarMin(map);
            PrintTrace(map, senicRoute.trace);
            Console.WriteLine("Scenic Rout length: " + senicRoute.steps);
        }


        private static (int steps, List<Tile> trace) aStar(Tile[][] map, Tile? start = null) {
            if (start == null)
                start = map.SelectMany(x => x).First(t => t.isStart);

            PriorityQueue<Tile, int> fScoreQueue = new PriorityQueue<Tile, int>();
            fScoreQueue.Enqueue(start, start.minRemainingStepsToMax);

            Dictionary<Tile, int> gScore = new Dictionary<Tile, int>
            {
                { start, 0 }
            };

            Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();

            while (fScoreQueue.TryPeek(out Tile _, out int currentFscore))
            {
                Tile current = fScoreQueue.Dequeue();

                if (current.isEnd) {
                    //PrintTrace(map, cameFrom, current);
                    return (
                            currentFscore,
                            IEnumerableExtentions.InfiniteByValueTransformation(current, c => cameFrom.GetValueOrDefault(c, start)).TakeWhile(t => !t.isStart).Reverse().ToList());
                }

                foreach (Tile neigbour in map.DirectNeighboursFor(current.y, current.x).Where(other => current.canBeClimbed(other)))
                {
                    int tentativeGscore = gScore.GetOrThrow(current) + 1;
                    if((!gScore.ContainsKey(neigbour)) || tentativeGscore < gScore.GetOrThrow(neigbour))
                    {
                        cameFrom[neigbour] = current;
                        gScore[neigbour] = tentativeGscore;
                        fScoreQueue.EnqueueOrUpdate(neigbour, tentativeGscore + neigbour.minRemainingStepsToMax); 
                    }
                }
            }

            throw new NotImplementedException();
        }

        private static (int steps, List<Tile> trace) aStarMin(Tile[][] map, Tile? start = null)
        {
            if (start == null)
                start = map.SelectMany(x => x).First(t => t.isEnd);

            PriorityQueue<Tile, int> fScoreQueue = new PriorityQueue<Tile, int>();
            fScoreQueue.Enqueue(start, start.height);

            Dictionary<Tile, int> gScore = new Dictionary<Tile, int>
            {
                { start, 0 }
            };

            Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();

            while (fScoreQueue.TryPeek(out Tile _, out int currentFscore))
            {
                Tile current = fScoreQueue.Dequeue();

                if (current.height == 0)
                {
                    //PrintTrace(map, cameFrom, current);
                    return (
                            currentFscore,
                            IEnumerableExtentions.InfiniteByValueTransformation(current, c => cameFrom.GetValueOrDefault(c, current)).TakeWhile(t =>  !t.isEnd).ToList());
                }

                foreach (Tile neigbour in map.DirectNeighboursFor(current.y, current.x).Where(other => other.canBeClimbed(current)))
                {
                    int tentativeGscore = gScore.GetOrThrow(current) + 1;
                    if ((!gScore.ContainsKey(neigbour)) || tentativeGscore < gScore.GetOrThrow(neigbour))
                    {
                        cameFrom[neigbour] = current;
                        gScore[neigbour] = tentativeGscore;
                        fScoreQueue.EnqueueOrUpdate(neigbour, tentativeGscore + neigbour.height);
                    }
                }
            }

            throw new NotImplementedException();
        }

        private static void PrintTrace(Tile[][] map, Dictionary<Tile, Tile> cameFrom, Tile current)
        {
            char[][] cMap = map.Select(s => s.Select(t => t.Name).ToArray()).ToArray();
            cMap[current.y][current.x] = '#';
            while(cameFrom.ContainsKey(current))
            {
                var next = cameFrom[current];
                int xDist = current.x - next.x;
                int yDist = current.y -next.y;
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
                current = next;
            }
            Console.WriteLine(cMap.ToFormmatedString() + "\n");
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
