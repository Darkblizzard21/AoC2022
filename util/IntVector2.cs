using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.util
{
    public struct IntVector2
    {
        public int x;
        public int y;

        public IntVector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public IntVector2(int n)
        {
            this.x = n;
            this.y = n;
        }

        public int ManthattenDistanceTo(IntVector2 other) => Math.Abs(x - other.x) + Math.Abs(y - other.y);
        public override string ToString()
        {
            return + x + "," + y;
        }
        public override int GetHashCode()
        {
            return (x + 17) * 31 + y;
        }
    }
}
