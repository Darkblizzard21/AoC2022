namespace AoC2022.days
{
    public static class Day06
    {
        public static void solve()
        {
            InputProvider inputProvider = new InputProvider("day06");
            var firstResult = inputProvider.GetInput().ToCharArray().GroupSlide(4).Enumerate().Where(c => c.Item2.Distinct().Count() == 4).Select(t => t.Item1 + 4).First();
            Console.WriteLine(firstResult);
            var secondResult = inputProvider.GetInput().ToCharArray().GroupSlide(14).Enumerate().Where(c => c.Item2.Distinct().Count() == 14).Select(t => t.Item1 + 14).First();
            Console.WriteLine(secondResult);
        }
    }
}
