using AoC2022.util;

namespace AoC2022.days
{
    public static class Day01 {
        public static void solve()
        {
            InputProvider inputProvider = new InputProvider("day01");
            var list = inputProvider.Get(InputType.Input)
                .Split("\r\n\r\n")
                .Select(s => s.Split("\r\n"))
                .Select(sl => sl.Select(Int32.Parse).Sum())
                .ToList();

            // First 
            int highest = list.Max();
            Console.WriteLine(highest);
            // Second
            int highest3 = list.OrderByDescending(i => i).Take(3).Sum();
            Console.WriteLine(highest3);
        }
    }
}
