using AoC2022.util;
using System.Linq;

namespace AoC2022.days
{
    public static class Day21
    {
        private class Monkey
        {
            public string name;
            public string a = "";
            public string b = "";
            public char op;
            public Monkey(string name, string a, string b, char op) {
                this.name= name;
                this.a= a;  
                this.b= b;
                this.op= op;
            }

            public bool TryToSolve(Dictionary<string, long> map, out long res)
            {
                res = 0;
                if (!map.ContainsKey(a) || !map.ContainsKey(b)) return false;
                long aValue = map[a];
                long bValue = map[b];
                res = op switch
                {
                    '+' => aValue + bValue,
                    '-' => aValue - bValue,
                    '*' => aValue * bValue,
                    '/' => aValue / bValue,
                    _ => throw new ArgumentException()
                };
                return true;
            }
        }
        public static void solve()
        {
            InputProvider inputProvider = new InputProvider("day21");
            var lines = inputProvider.Get(InputType.Input).Split("\r\n").Select(l => l.Split(" ")).ToList();
            var dict = new Dictionary<string, long>(lines.Where(l => l.Length == 2).Select(l => new KeyValuePair<string, long>(l[0].Substring(0,4), long.Parse(l[1]))));
            var operationMonkeys = lines.Where(l => l.Length == 4).Select(l => new Monkey(l[0].Substring(0, 4), l[1], l[3], l[2][0])).ToList();

            while (operationMonkeys.Any())
            {
                var remove = new List<Monkey>();
                foreach (var monkey in operationMonkeys)
                {
                    if(monkey.TryToSolve(dict, out long res))
                    {
                        remove.Add(monkey);
                        dict.Add(monkey.name, res);
                    }
                }
                foreach (var item in remove)
                {
                    operationMonkeys.Remove(item);
                }
            }
            var result = dict["root"];
            Console.WriteLine(result);

            
            dict = new Dictionary<string, long>(lines.Where(l => l.Length == 2 && l[0] != "humn:").Select(l => new KeyValuePair<string, long>(l[0].Substring(0, 4), long.Parse(l[1]))));
            operationMonkeys = lines.Where(l => l.Length == 4 && l[0] != "root:").Select(l => new Monkey(l[0].Substring(0, 4), l[1], l[3], l[2][0])).ToList();
            var root = lines.Where(l => l[0] == "root:").Select(l => new Monkey(l[0].Substring(0, 4), l[1], l[3], l[2][0])).First();

            var solved = new List<Monkey>();
            do
            {
                solved.Clear();
                foreach (var monkey in operationMonkeys)
                {
                    if (monkey.TryToSolve(dict, out long res))
                    {
                        solved.Add(monkey);
                        dict.Add(monkey.name, res);
                    }
                }
                foreach (var item in solved)
                {
                    operationMonkeys.Remove(item);
                }
            } while (solved.Count > 0);

            if (dict.ContainsKey(root.a)) result = HumanYellComputation(dict[root.a], root.b, operationMonkeys, dict);
            else result = HumanYellComputation(dict[root.b], root.a, operationMonkeys, dict);

            var rootAValue = dict.ContainsKey(root.a) ? dict[root.a] : 0;
            var rootbValue = dict.ContainsKey(root.b) ? dict[root.b] : 0;
            Console.WriteLine(result);
        }

        private static long HumanYellComputation(long toMatch, string target, List<Monkey> unSolvedMonkeys, Dictionary<string, long> lookUpTable)
        {
            var unsolveMonkeysByName = new Dictionary<string, Monkey>(unSolvedMonkeys.Select(m => new KeyValuePair<string,Monkey>(m.name, m)));
            unsolveMonkeysByName.Add("humn", new Monkey("humn", "", "", '+'));

            long Solve(long toMatch, Monkey current)
            {
                if (current.name == "humn") return toMatch;
                if (lookUpTable.ContainsKey(current.a))
                {
                    return current.op switch
                    {
                        '+' => Solve(toMatch - lookUpTable[current.a], unsolveMonkeysByName[current.b]),
                        '-' => Solve(lookUpTable[current.a] - toMatch, unsolveMonkeysByName[current.b]),
                        '*' => Solve(toMatch / lookUpTable[current.a], unsolveMonkeysByName[current.b]),
                        '/' => Solve(lookUpTable[current.a] / toMatch, unsolveMonkeysByName[current.b]),
                        _ => throw new ArgumentException()
                    };
                }
                else
                {
                    return current.op switch
                    {
                        '+' => Solve(toMatch - lookUpTable[current.b], unsolveMonkeysByName[current.a]),
                        '-' => Solve(lookUpTable[current.b] + toMatch, unsolveMonkeysByName[current.a]),
                        '*' => Solve(toMatch / lookUpTable[current.b], unsolveMonkeysByName[current.a]),
                        '/' => Solve(lookUpTable[current.b] * toMatch, unsolveMonkeysByName[current.a]),
                        _ => throw new ArgumentException()
                    };
                }
            }
            return Solve(toMatch, unsolveMonkeysByName[target]);
        }


    }
}
