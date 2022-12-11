using AoC2022.util;

namespace AoC2022.days
{
    public static class Day05
    {
        public static void solve()
        {
            InputProvider inputProvider = new InputProvider("day05");
            string[] stateAndInstructions = inputProvider.Get(InputType.Input).Split("\r\n\r\n");
            Stack<char>[] stacks = ReadStacks(stateAndInstructions[0]);
            var countFromToList = stateAndInstructions[1].Split("\r\n").Select(s => s.Split(' ').Where(s => Int32.TryParse(s, out int _)).Select(s => Int32.Parse(s)).ToArray()).ToArray();

            foreach (var countFromTo in countFromToList)
            {
                int count = countFromTo[0];
                int from = countFromTo[1]-1;
                int to = countFromTo[2]-1;
                for (int i = 0; i < count; i++)
                {
                    stacks[to].Push(stacks[from].Pop());
                }
            }
            var firstResult = stacks.Select(stack => stack.Peek()).Aggregate("", (a, b) => a + b);
            Console.WriteLine(firstResult);
            // Second
            stacks = ReadStacks(stateAndInstructions[0]);
            foreach (var countFromTo in countFromToList)
            {
                int count = countFromTo[0];
                int from = countFromTo[1] - 1;
                int to = countFromTo[2] - 1;
                Stack<char> auxStack = new Stack<char>();
                for (int i = 0; i < count; i++)
                {
                    auxStack.Push(stacks[from].Pop());
                }
                while(auxStack.Count > 0)
                {
                    stacks[to].Push(auxStack.Pop());
                }
            }
            var secondResult = stacks.Select(stack => stack.Peek()).Aggregate("", (a, b) => a + b);
            Console.WriteLine(secondResult);

        }

        public static Stack<char>[] ReadStacks(string input)
        {
            var levels = input.Split("\r\n");
            var stackCount = levels[levels.Length - 1].Split("   ").Length;
            Stack<char>[] stacks = new Stack<char>[stackCount].Select(_ => new Stack<char>()).ToArray();
            for (int i = levels.Length - 2; i >= 0; i--)
            {
                for (int j = 0; j < stackCount; j++)
                {
                    char c = levels[i][1 + 4 * j];
                    if (c != ' ')
                        stacks[j].Push(c);
                }
            }

            return stacks; 
        }
    }
}
