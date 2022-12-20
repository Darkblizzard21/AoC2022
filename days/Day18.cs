using AoC2022.util;

namespace AoC2022.days
{
    public static class Day18
    {
        private enum State
        {
            Air,
            Lava,
            BubbleAir,
            FreeAir
        }
        public static void solve()
        {
            InputProvider inputProvider = new InputProvider("day18");
            var points = inputProvider.Get(InputType.Input).Split("\r\n").Select(s => s.Split(",")).Select(s => new IntVector3(s)).ToList();
            var minMax = points.Aggregate((min: new IntVector3(int.MaxValue), max: new IntVector3(int.MinValue)), (p, c) =>
            {
                p.min.x = Math.Min(p.min.x, c.x);
                p.min.y = Math.Min(p.min.y, c.y);
                p.min.z = Math.Min(p.min.z, c.z);
                p.max.x = Math.Max(p.max.x, c.x);
                p.max.z = Math.Max(p.max.y, c.y);
                p.max.y = Math.Max(p.max.z, c.z);
                return p;
            });
            var min = minMax.min - new IntVector3(2); 
            var max = minMax.max + new IntVector3(2);
            var size = max - min;
            var map = ArrayExtensions.New3DWithDefault(size.x, size.y, size.z, () => State.Air);
            points.ForEach(point =>
            {
                var pos = point - min;
                map[pos.x][pos.y][pos.z] = State.Lava;
            });


            var surfaceAreaWithBubbles = map.SelectMany((ar, x) => ar.SelectMany((ar, y) => ar.Select((b, z) => (b, z)).Where(t => t.b == State.Lava).Select(t => map.DirectNeighboursFor(x, y, t.z).Where(b => b != State.Lava).Count()))).Sum();
            Console.WriteLine("SurfaceAreaWithAirBubbles: " + surfaceAreaWithBubbles);

            while (map.TryGetFirstVectorOfQuery(s => s == State.Air, out IntVector3 vec)) {
                var airPatch = BreathFirstSearch.Reachable(vec, v => map.DirectVectorNeighboursFor(v).Where(s => map.Get(s) == State.Air)).ToList();
                bool isFreeAir = airPatch.Any(v => v.x == 0 || v.y == 0 || v.z == 0 || v.x == size.x - 1 || v.y == size.y -1 || v.z == size.z - 1); 
                var state = isFreeAir? State.FreeAir : State.BubbleAir;
                airPatch.ForEach(v => map.Set(v, state));
            }

            var surfaceArea = map.SelectMany((ar, x) => ar.SelectMany((ar, y) => ar.Select((b, z) => (b, z)).Where(t => t.b == State.Lava).Select(t => map.DirectNeighboursFor(x, y, t.z).Where(b => b == State.FreeAir).Count()))).Sum();
            Console.WriteLine("SurfaceArea: " + surfaceArea);
        }
    }
}
