using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AoC2022.util;

namespace AoC2022.days
{
    public static class Day08
    {
        public static void solve()
        {
            InputProvider inputProvider = new InputProvider("day08");
            int[][] trees = inputProvider.Get(Type.Input).Split("\r\n").Select(s => s.ToCharArray().Select(c => Int32.Parse(""+c)).ToArray()).ToArray();
    

            var visibleTrees = trees.SelectMany((a, x) => a.Where((_, y) => xyIsVisible(x, y, trees))).Count();
            Console.WriteLine(visibleTrees);

            var bestScenicScore = trees.SelectMany((a, x) => a.Select((_, y) => scenicScore(x, y, trees))).Max();
            Console.WriteLine(bestScenicScore);

        }
        static bool xyIsVisible(int x, int y, int[][] map)
        {
            if (x == 0 || x == map.Length - 1) return true;
            if (y == 0 || y == map[x].Length - 1) return true;

            int height = map[x][y];
            bool visibleInXMinus = !IEnumerableExtentions.FromTo(x -  1, -1).Select(i => map[i][y] < height).Any(b => !b);
            if (visibleInXMinus) return true;

            bool visibleInYMinus = !IEnumerableExtentions.FromTo(y - 1, -1).Select(i => map[x][i] < height).Any(b => !b);
            if (visibleInYMinus) return true;

            bool visibleInXPositiv = !IEnumerableExtentions.FromTo(x + 1, map.Length).Select(i => map[i][y] < height).Any(b => !b);
            if (visibleInXPositiv) return true;

            bool visibleInYPositiv = !IEnumerableExtentions.FromTo(y + 1, map[x].Length).Select(i => map[x][i] < height).Any(b => !b);
            if (visibleInYPositiv) return true;

            return false;
        }

        static int scenicScore(int x, int y, int[][] map)
        {
            if (x == 0 || x == map.Length - 1) return 0;
            if (y == 0 || y == map[x].Length - 1) return 0;

            int height = map[x][y];
            int upScore = x - IEnumerableExtentions.FromTo(x - 1, -1).FirstOrDefault(i => map[i][y] >= height, 0);
            int leftScore = y - IEnumerableExtentions.FromTo(y - 1, -1).FirstOrDefault(i => map[x][i] >= height, 0);
            int downScore = IEnumerableExtentions.FromTo(x + 1, map.Length).FirstOrDefault(i => map[i][y] >= height, map.Length-1) - x;
            int rightScore = IEnumerableExtentions.FromTo(y + 1, map[x].Length).FirstOrDefault(i => map[x][i] >= height, map[x].Length - 1) - y;

            int score = upScore * leftScore * downScore * rightScore;
            return score;
        }
    }
}
