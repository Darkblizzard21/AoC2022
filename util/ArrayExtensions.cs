using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.util
{
    public static class ArrayExtensions
    {

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

        public static IEnumerable<T> DirectNeighboursFor<T>(this T[][][] array, IntVector3 vec) => array.DirectNeighboursFor(vec.x,vec.y,vec.z);
        public static IEnumerable<T> DirectNeighboursFor<T>(this T[][][] array, int x, int y, int z)
        {
            // left
            int leftX = x - 1;
            if (0 <= leftX && y < array[leftX].Length && z < array[leftX][y].Length)
                yield return array[leftX][y][z];

            // right

            int rightX = x + 1;
            if (rightX < array.Length && y < array[rightX].Length && z < array[rightX][y].Length)
                yield return array[rightX][y][z];
            // down
            int lowerY = y - 1;
            if (0 <= lowerY && z < array[x][lowerY].Length)
                yield return array[x][lowerY][z];

            // up 
            int upperY = y + 1;
            if (upperY < array[x].Length && z < array[x][upperY].Length)
                yield return array[x][upperY][z];

            // forward
            int lowerZ = z - 1;
            if (0 <= lowerZ)
                yield return array[x][y][lowerZ];

            // backwards 
            int upperZ = z + 1;
            if (upperZ < array[x][y].Length)
                yield return array[x][y][upperZ];
        }

        public static IEnumerable<IntVector3> DirectVectorNeighboursFor<T>(this T[][][] array, IntVector3 vec) => array.DirectVectorNeighboursFor(vec.x, vec.y, vec.z);
        public static IEnumerable<IntVector3> DirectVectorNeighboursFor<T>(this T[][][] array, int x, int y, int z)
        {
            // left
            int leftX = x - 1;
            if (0 <= leftX && y < array[leftX].Length && z < array[leftX][y].Length)
                yield return new IntVector3(leftX,y,z);

            // right

            int rightX = x + 1;
            if (rightX < array.Length && y < array[rightX].Length && z < array[rightX][y].Length)
                yield return new IntVector3(rightX, y, z);
            // down
            int lowerY = y - 1;
            if (0 <= lowerY && z < array[x][lowerY].Length)
                yield return new IntVector3(x, lowerY, z);

            // up 
            int upperY = y + 1;
            if (upperY < array[x].Length && z < array[x][upperY].Length)
                yield return new IntVector3(x, upperY, z);

            // forward
            int lowerZ = z - 1;
            if (0 <= lowerZ)
                yield return new IntVector3(x, y, lowerZ);

            // backwards 
            int upperZ = z + 1;
            if (upperZ < array[x][y].Length) 
                yield return new IntVector3(x, y, upperZ);
        }

        public static T[][] New2DWithDefault<T>(int xSize, int ySize, Func<T> defaultGenerator) => IEnumerableExtentions.Generate(xSize, () => IEnumerableExtentions.Generate(ySize, () => 0))
            .Select(t => t.Select(_ => defaultGenerator()).ToArray())
            .ToArray();

        public static T[][][] New3DWithDefault<T>(int xSize, int ySize, int zSize, Func<T> defaultGenerator) => IEnumerableExtentions.Generate(xSize,
            () => IEnumerableExtentions.Generate(ySize,
                () => IEnumerableExtentions.Generate(zSize, () => defaultGenerator()).ToArray())
                .ToArray())
            .ToArray();

        public static IEnumerable<IntVector3> Vectors<T>(this T[][][] values) => IEnumerableExtentions.FromTo(0, values.Length)
            .SelectMany(x => IEnumerableExtentions.FromTo(0, values[x].Length)
            .SelectMany(y => IEnumerableExtentions.FromTo(0, values[x][y].Length).Select(z => new IntVector3(x, y, z))));
        public static bool TryGetFirstVectorOfQuery<T>(this T[][][] values, Func<T,bool> predicate, [MaybeNullWhen(false)] out IntVector3 vector3) where T: notnull
        {
            vector3 = default;
            var b = values.ZipWithIndices()
                .SelectMany(x => x.value.ZipWithIndices()
                .SelectMany(y => y.value.ZipWithIndices()
                .Where(v => predicate(v.value)).Select(v => new IntVector3(x.index, y.index, v.index)))).TryGetFirst(out IntVector3 res);
            if (b) vector3 = res;
            return b;
        }
        public static T Get<T>(this T[][][] values, IntVector3 v) => values[v.x][v.y][v.z];
        public static void Set<T>(this T[][][] values, IntVector3 v, T value)
        {
            values[v.x][v.y][v.z] = value;
        }
    }
}
