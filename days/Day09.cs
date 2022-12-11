using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using AoC2022.util;

namespace AoC2022.days
{
    public static class Day09
    {
        public static void solve()
        {
            InputProvider inputProvider = new InputProvider("day09");
            // First Part
            RopeState shortRope = new RopeState();
            shortRope.ExecuteInstructions(inputProvider.Get(InputType.Input).Split("\r\n"));
            Console.WriteLine(shortRope.TrailToString());
            int shortVisits = shortRope.UniqueVisits();
            Console.WriteLine(shortVisits);
            // SecondPart
            RopeState longRope = new RopeState(10, Draw);
            longRope.ExecuteInstructions(inputProvider.Get(InputType.Input).Split("\r\n"));
            Console.WriteLine(longRope.TrailToString());
            Console.WriteLine(longRope.UniqueVisits());

        }

        private static void Draw(string name, RopeState state)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;

            var size = new Vector2(700, 700);
            var offset = new Vector2(-350, -350);

            Bitmap image = new Bitmap((int) size.X, (int) size.Y);

            using (Graphics g = Graphics.FromImage(image))
            {
                g.FillRectangle(Brushes.Black, 0, 0, image.Width, image.Height);
            }

            var length = state.visited.Count();
            state.visited.ZipWithIndices().DoForEach(t =>
            {
                var truePos = t.value - offset;
                int x = (int) truePos.X + 1;
                int y = (int)   truePos.Y + 1;
                float percent = t.index /  (float) length;
                image.SetPixel(x, y, Drawing.Inferno(percent));
            });
            var truePos = Vector2.Zero - offset;
            int x = (int)truePos.X + 1;
            int y = (int)truePos.Y + 1;
            image.SetPixel(x, y, Color.White);

            state.ropeParts.DoForEach(v =>
            {
                var truePos = v - offset;
                int x = (int)truePos.X + 1;
                int y = (int)truePos.Y + 1;
                image.SetPixel(x, y, Color.Brown);
                var _ = "";
            });

            Drawing.Save(image, @"day09\raw\" + name + ".png");
            image.Dispose();
        }
    }

    class RopeState
    {
        public readonly int length;
        public List<Vector2> visited;
        public Vector2[] ropeParts;
        Action<string, RopeState>? drawAction;

        int instruction = 0;
        public Vector2 head
        {
            get => ropeParts[0];
            set => ropeParts[0] = value;
        }

        public Vector2 tail
        {
            get => ropeParts[length-1];  
            set => ropeParts[length - 1] = value;
        }

        public RopeState(int ropeLength = 2, Action<string, RopeState>? draw = null)
        {
            length = Math.Max(Math.Abs(ropeLength), 2);
            ropeParts = IEnumerableExtentions.Fill(length, () => new Vector2(0)).ToArray();
            visited = new List<Vector2>() { new Vector2(0, 0) };
            drawAction = draw;
        }

        public void MoveHead(Vector2 move)
        {
            bool moveHoizontal = move.Y == 0;
            int movement = (int) (move.X == 0 ? move.Y : move.X);
            int dir = Math.Sign(movement);
            int count = Math.Abs(movement);

            for (int m = 0; m < count; m++)
            {
                if(moveHoizontal)
                {
                    ropeParts[0].X += dir;
                }
                else {
                    ropeParts[0].Y += dir;
                }

                for (int i = 1; i < length; i++)
                {
                    var current = ropeParts[i];
                    var predecessor = ropeParts[i-1];
                    var toPredecessor = predecessor - current;
                    var distanceToPredecessor = toPredecessor.LengthSquared();
                    if (distanceToPredecessor < 4.0) break;
                    else if(distanceToPredecessor < 8.0)
                    {
                        ropeParts[i] = Math.Abs(toPredecessor.X) > Math.Abs(toPredecessor.Y) ? 
                            new Vector2(predecessor.X - Math.Sign(toPredecessor.X), predecessor.Y) 
                            : new Vector2(predecessor.X, predecessor.Y - Math.Sign(toPredecessor.Y));
                            
                    }
                    else if(distanceToPredecessor == 8.0)
                    {
                        ropeParts[i] = new Vector2(predecessor.X - Math.Sign(toPredecessor.X),
                                         predecessor.Y - Math.Sign(toPredecessor.Y));

                    }
                    else
                    {
                        Console.WriteLine(StateToString());
                        Console.WriteLine("Err");
                    }
                }
                visited.Add(ropeParts[ropeParts.Length - 1]);

                if (drawAction != null)
                {
                    drawAction((instruction * 100 + m).ToString("D6"), this);
                }
            }
        }

        public void ExecuteInstructions(IEnumerable<string> instructions)
        {
            var enumerator = instructions.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var split = enumerator.Current.Split(" ");
                int count = Int32.Parse(split[1]);

                var moveVec = split[0] switch
                {
                    "R" => new Vector2(count, 0),
                    "L" => new Vector2(-count, 0),
                    "D" => new Vector2(0, count),
                    "U" => new Vector2(0, -count),
                    _ => throw new ArgumentException()
                };
                MoveHead(moveVec);

                instruction++;
            }
        }

        public int UniqueVisits() => visited.Distinct().Count();

        public string TrailToString() => Visualize(false).ToFormmatedString();
        public string StateToString() => Visualize(true).ToFormmatedString();

        private char[][] Visualize(bool visualizeRopParts)
        {
            (Vector2, Vector2) bounds = visited.Concat(visualizeRopParts ?
                    ropeParts :
                    Enumerable.Empty<Vector2>())
                .Aggregate(
                    (new Vector2(0), new Vector2(0)),
                    (p, c) => (new Vector2(Math.Min(p.Item1.X, c.X), Math.Min(p.Item1.Y, c.Y)),
                                new Vector2(Math.Max(p.Item2.X, c.X), Math.Max(p.Item2.Y, c.Y))));
            Vector2 lowerBound = bounds.Item1;
            Vector2 upperBound = bounds.Item2;
            Vector2 size = upperBound - lowerBound;

            char[][] map = IEnumerableExtentions.FromTo(0, (int)size.Y + 3).Select(_ => IEnumerableExtentions.FromTo(0, (int)size.X + 3).Select(_ => '.').ToArray()).ToArray();
            visited.ForEach(v => {
                var truePos = v - lowerBound;
                map[(int)truePos.Y + 1][(int)truePos.X + 1] = '#';
            });

            var truePos = new Vector2(0) - lowerBound;
            map[(int)truePos.Y + 1][(int)truePos.X + 1] = 's';

            if (visualizeRopParts)
                ropeParts.ZipWithIndices().Reverse().DoForEach(t => {
                    var truePos = t.value - lowerBound;
                    int y = (int)truePos.Y + 1;
                    int x = (int)truePos.X + 1;
                    char c = t.index == 0 ? 'H' : t.index.ToString().Last();
                    if (map[y][x] != '.' && map[y][x] != '#')
                        Console.WriteLine("" + c + " covers " + map[y][x]);
                    map[y][x] = c;
                });

            return map;
        }
    }
}
