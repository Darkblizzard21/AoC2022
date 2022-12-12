using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.util
{
    public static class ArrayExtensions
    {
        public static string ToFormmatedString(this char[][] array) => array.Select(l => l.Aggregate("", (p, c) => p + c)).Aggregate((p, c) => p + "\n" + c);

        public static IEnumerable<T> DirectNeighboursFor<T>(this T[][] array, int x, int y)
        {
            // left
            int leftX = x - 1;
            if (0 <= leftX && y < array[leftX].Length)
                yield return array[leftX][y];

            // right
            int rightX = x + 1;
            if (rightX < array.Length && y < array[rightX].Length) 
                yield return array[rightX][y];

            // down
            int lowerY = y - 1;
            if (0 <= lowerY)
                yield return array[x][lowerY];

            // up 
            int upperY = y + 1;
            if (upperY < array[x].Length)
                yield return array[x][upperY];
        }
    }
}
