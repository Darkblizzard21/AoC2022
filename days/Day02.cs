using AoC2022.util;

namespace AoC2022.days
{
    public enum RPS
    {
        Rock,
        Paper,
        Scissors
    }

    public enum Result
    {
        Win,
        Draw,
        Lose
    }
    static class Day02
    {
        public static void solve()
        {
            InputProvider inputProvider = new InputProvider("day02");
            // FirstPart
            int firstResult = inputProvider.GetInput()
                .Split("\r\n")
                .Select(s => s.Split(" ")
                    .Select(c => c switch
                    {
                        "A" or "X" => RPS.Rock,
                        "B" or "Y" => RPS.Paper,
                        "C" or "Z" => RPS.Scissors,
                        _ => throw new InvalidOperationException(c),
                    }).ToArray())
                .Select(l => l[1].Value() + l[1].Match(l[0])).Sum();
            Console.WriteLine("Day02: First Result: " + firstResult);
            // Second Part
            var secondResult = inputProvider.GetInput()
                .Split("\r\n")
                .Select(s => (s[0] switch
                {
                    'A' => RPS.Rock,
                    'B' => RPS.Paper,
                    'C' => RPS.Scissors,
                    _ => throw new InvalidOperationException("" + s[0]),
                },
                s[2] switch
                {
                    'X' => Result.Lose,
                    'Y' => Result.Draw,
                    'Z' => Result.Win,
                    _ => throw new InvalidOperationException("" + s[2]),
                }))
                .Select(t => t.Item1.GetMatching(t.Item2).Value() + t.Item2.Value())
                .Sum();
            Console.WriteLine("Day02: Second Result: " + secondResult);
        }
    }


    public static class RPSExtensions
    {
        public static int Value(this RPS rps) => rps switch
        {
            RPS.Rock => 1,
            RPS.Paper => 2,
            RPS.Scissors => 3,
            _ => throw new InvalidOperationException()
        };

        public static int Match(this RPS first, RPS second)
        {
            if (first == second) return 3; // Draw

            RPS[] rpsList = new RPS[] { RPS.Rock, RPS.Paper, RPS.Scissors };
            int firstIndex = Array.IndexOf(rpsList, first);

            if (second == rpsList[(firstIndex + 1) % 3])
            {
                return 0; // Lose
            }
            if (second == rpsList[(firstIndex - 1 + 3) % 3])
            {
                return 6; // Win
            }
            throw new InvalidOperationException();
        }

        public static RPS GetMatching(this RPS rps, Result result)
        {
            RPS[] rpsList = new RPS[] { RPS.Rock, RPS.Paper, RPS.Scissors };
            return result switch
            {
                Result.Draw => rps,
                Result.Win => rpsList[(Array.IndexOf(rpsList, rps) + 1) % 3],
                Result.Lose => rpsList[(Array.IndexOf(rpsList, rps) + 2) % 3],
                _ => throw new InvalidOperationException()
            };
        }
    }

    public static class ResultExtensions
    {
        public static int Value(this Result result) => result switch
        {
            Result.Win => 6,
            Result.Draw => 3,
            Result.Lose => 0,
            _ => throw new InvalidOperationException()
        };
    }
}
