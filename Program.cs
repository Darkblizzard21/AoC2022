using AoC2022;

InputProvider inputProvider = new InputProvider("day01");
// First 
int highest = inputProvider.GetInput().Split("\r\n\r\n").Select(s => s.Split("\r\n")).Select(sl => sl.Select(s => { Int32.TryParse(s, out int i); return i; }).Sum()).Max();
// Second
int highest3 = inputProvider.GetInput().Split("\r\n\r\n").Select(s => s.Split("\r\n")).Select(sl => sl.Select(s => { Int32.TryParse(s, out int i); return i; }).Sum()).OrderByDescending(i => i).Take(3).Sum();

Console.WriteLine("Hello, World!");
Console.WriteLine(highest);
Console.WriteLine(highest3);