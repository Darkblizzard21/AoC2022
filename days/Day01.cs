using AoC2022.util;

namespace AoC2022.days
{
    static class Day01 {
        static void sovle()
        {
            InputProvider inputProvider = new InputProvider("day01");
            // First 
            int highest = inputProvider.Get(InputType.Input).Split("\r\n\r\n").Select(s => s.Split("\r\n")).Select(sl => sl.Select(s => { Int32.TryParse(s, out int i); return i; }).Sum()).Max();
            Console.WriteLine(highest);
            // Second
            int highest3 = inputProvider.Get(InputType.Input).Split("\r\n\r\n").Select(s => s.Split("\r\n")).Select(sl => sl.Select(s => { Int32.TryParse(s, out int i); return i; }).Sum()).OrderByDescending(i => i).Take(3).Sum();
            Console.WriteLine(highest);
        }
    }
}
