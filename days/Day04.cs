using AoC2022.util;

namespace AoC2022.days
{
    public static class Day04
    {
        public static void solve()
        {
            InputProvider inputProvider = new InputProvider("day04");
            // First Part
            var SubsetAsignments = inputProvider.GetInput()
                .Split("\r\n")
                .Select(s => s.Split(",").Select(s => s.Split("-").Select(s => { Int32.TryParse(s, out int i); return i; }).ToArray()).ToArray())
                .Where(a => IsSubset(a[0][0], a[0][1], a[1][0], a[1][1]))
                .Count();
            Console.WriteLine(SubsetAsignments);
            // Second Part
            var OverlappingAsignments = inputProvider.GetInput()
                .Split("\r\n")
                .Select(s => s.Split(",").Select(s => s.Split("-").Select(s => { Int32.TryParse(s, out int i); return i; }).ToArray()).ToArray())
                .Where(a => IsOverlapping(a[0][0], a[0][1], a[1][0], a[1][1]))
                .Count();
            Console.WriteLine(SubsetAsignments);
        }

        public static bool IsSubset(int firstMin, int firstMax, int secondMin, int secondMax) =>
           (secondMin >= firstMin && secondMax <= firstMax) || (firstMin >= secondMin && secondMax >= firstMax);

        public static bool IsOverlapping(int firstMin, int firstMax, int secondMin, int secondMax) =>
          (firstMin <= secondMax && firstMin >= secondMin) || (secondMin <= firstMax && secondMin >= firstMin);
    }
}
