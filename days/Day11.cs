using AoC2022.util;

namespace AoC2022.days
{
    internal class Monkey
    {
        List<long> items;
        string operationType;
        string operationSecond;
        public long divideBy;
        Func<long, bool> test;
        int trueTarget;
        int falseTarget;

        public int inspectCount { get; private set; } = 0;
        public int catchCount { get; private set; } = 0;
        public Monkey(string description) {
            var split = description.Split("\r\n");
            items = split[1].Substring(18).Split(" ")
                .Select(s => s.Replace(",", ""))
                .Select(s => Int64.Parse(s))
                .ToList();

            var operationStr = split[2].Substring(23).Split(" ");
            operationType = operationStr[0];
            operationSecond = operationStr[1];

            divideBy = Int64.Parse(split[3].Split(" ").Last());
            test = i => i % divideBy == 0;

            trueTarget = Int32.Parse(split[4].Split(" ").Last());
            falseTarget = Int32.Parse(split[5].Split(" ").Last());
        }

        public void inspectAndBore() 
        {
            inspectCount += items.Count();
            items = items.Select(operation).Select(i => i / 3).ToList();
        }

        public void inspect(long mod)
        {
            inspectCount += items.Count();
            items = items.Select(operation).Select(i => i % mod).ToList();
        }

        public void throwItems(Monkey[] monkeys)
        {
            foreach (var item in items)
            {
                if (test(item))
                {
                    monkeys[trueTarget].Catch(item);
                }
                else
                {
                    monkeys[falseTarget].Catch(item);
                }
            }
            items.Clear();
        }

        public void Catch(long i)
        {
            items.Add(i);
            catchCount++;
        }

        private long operation(long i)
        {
            long second = operationSecond switch
            {
                "old" => i,
                _ => Int64.Parse(operationSecond)
            };
            return operationType switch
            {
                "*" => i * second,
                "+" => i + second,
                _ => throw new ArgumentException()
            };
        }

        public IReadOnlyList<long> getItems() => items;
    }
    public static class Day11
    {
        public static void solve()
        {
            InputProvider inputProvider = new InputProvider("day11");
            Monkey[] monkeys = inputProvider.Get(InputType.Input).Split("\r\n\r\n").Select(s => new Monkey(s)).ToArray();

            // First Part
            int roundCount = 20;
            for (int i = 0; i < roundCount; i++)
            {
                foreach (var monkey in monkeys)
                {
                    monkey.inspectAndBore();
                    monkey.throwItems(monkeys);
                }

                //Console.WriteLine("After round " + (i + 1) + ", the monkeys are holding items with these worry levels:");
                //printMonkeys(monkeys);
            }

            var monkeyBuissnes = monkeys.Select(m=> m.inspectCount).OrderByDescending(i=>i).Take(2).Aggregate((p,c) => p * c);
            Console.WriteLine("MonkeyBuissnes: " + monkeyBuissnes);

            // SecondPart
            monkeys = inputProvider.Get(InputType.Input).Split("\r\n\r\n").Select(s => new Monkey(s)).ToArray();
            roundCount = 10000;
            long monkeyMod = monkeys.Select(m => m.divideBy).Aggregate((p,c) => p * c);
            for (int i = 0; i < roundCount; i++)
            {
                foreach (var monkey in monkeys)
                {
                    monkey.inspect(monkeyMod);
                    monkey.throwItems(monkeys);
                }

                int rNumber = i + 1;
                if (rNumber % 1000 == 0 || rNumber == 1 || rNumber == 20)
                {
                    Console.WriteLine("== After round " + (i + 1) + " ==");
                    printMonkeysInspectCount(monkeys);
                }
            }
            long monkeyBuissnes10000 = monkeys.Select(m => m.inspectCount).OrderByDescending(i => i).Take(2).Select(i => (long) i).Aggregate((p, c) => p * c);
            Console.WriteLine("MonkeyBuissnes 10000: " + monkeyBuissnes10000);
        }


        private static void printMonkeysItems(Monkey[] monkeys)
        {
            for (int i = 0; i < monkeys.Length; i++)
            {
                Console.WriteLine("Monkey " + i + ": " + monkeys[i].getItems().Select(i => ""+i).DefaultIfEmpty("").Aggregate((p,c) => p+", "+c));
            }
            Console.WriteLine();
        }

        private static void printMonkeysInspectCount(Monkey[] monkeys)
        {
            for (int i = 0; i < monkeys.Length; i++)
            {
                Console.WriteLine("Monkey " + i + " inspected items " + monkeys[i].inspectCount + " times.");
            }
            Console.WriteLine();
        }
    }
}
