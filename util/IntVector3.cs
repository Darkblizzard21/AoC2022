
namespace AoC2022.util
{
    public struct IntVector3
    {
        public int x;
        public int y;
        public int z;

        public IntVector3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public IntVector3(string[] strings)
        {
            this.x = int.Parse(strings[0]);
            this.y = int.Parse(strings[1]);
            this.z = int.Parse(strings[2]);
        }

        public IntVector3(int n)
        {
            this.x = n;
            this.y = n;
            this.z = n;
        }

        public static IntVector3 operator +(IntVector3 iv1, IntVector3 iv2) => new IntVector3(iv1.x + iv2.x, iv1.y + iv2.z, iv1.z + iv2.z);
        public static IntVector3 operator -(IntVector3 iv1, IntVector3 iv2) => new IntVector3(iv1.x - iv2.x, iv1.y - iv2.z, iv1.z - iv2.z);

        public override string ToString()
        {
            return x + "," + y + "," + z;
        }

        public override int GetHashCode()
        {
            return ((x + 17) * 31 + y + 17) * 31 + z;
        }
    }
}
