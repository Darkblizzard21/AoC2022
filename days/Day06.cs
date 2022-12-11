using AoC2022.util;

namespace AoC2022.days
{
    public static class Day06
    {
        public static void solve()
        {
            InputProvider inputProvider = new InputProvider("day06");
            var firstResult = inputProvider.GetInput().ToCharArray().GroupSlide(4).ZipWithIndices().Where(c => c.value.Distinct().Count() == 4).Select(t => t.index + 4).First();
            Console.WriteLine(firstResult);
            var secondResult = inputProvider.GetInput().ToCharArray().GroupSlide(14).ZipWithIndices().Where(c => c.value.Distinct().Count() == 14).Select(t => t.index + 14).First();
            Console.WriteLine(secondResult);
        }
    }
}
