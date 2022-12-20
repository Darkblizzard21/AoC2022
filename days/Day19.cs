using AoC2022.util;
using System;
using System.Runtime.Versioning;

namespace AoC2022.days
{
    public static class Day19
    {
        private enum ResouceType
        {
            Ore,
            Clay,
            Obsidian,
            Geode
        }

        private struct Resouces
        {
            public int ore = 0;
            public int clay = 0;
            public int obsidian = 0;
            public int geode = 0;

            public static Resouces Ore(int count)
            {
                var res = new Resouces();
                res.ore = count;
                return res;
            }

            public static Resouces Clay(int count)
            {
                var res = new Resouces();
                res.clay = count;
                return res;
            }

            public static Resouces Obsidian(int count)
            {
                var res = new Resouces();
                res.obsidian = count;
                return res;
            }
            public void Add(ResouceType type, int count) {
                switch(type)
                {
                    case ResouceType.Ore:
                        ore += count; break;
                    case ResouceType.Clay:
                        clay += count; break;
                    case ResouceType.Obsidian:
                        obsidian += count; break;
                    case ResouceType.Geode:
                        geode += count; break;
                }
            }

            private Resouces(int ore, int clay, int obsidian, int geode)
            {
                this.ore = ore;
                this.clay = clay;
                this.obsidian = obsidian;
                this.geode = geode;
            }
            public Resouces Copy()
            {
                var res = new Resouces();
                res.ore= this.ore;
                res.clay = this.clay;
                res.obsidian = this.obsidian;
                res.geode = this.geode;
                return res;
            }
            public bool CanBeReachedOrSurpassedByMuliplication(Resouces other) =>
                (other.ore == 0 || ore > 0) &&
                (other.clay == 0 || clay > 0) &&
                (other.obsidian == 0 || obsidian > 0);

            public static Resouces operator *(Resouces a, int b) => new Resouces(a.ore * b, a.clay * b, a.obsidian * b, a.geode * b);
            public static Resouces operator +(Resouces a, Resouces b) => new Resouces(a.ore + b.ore, a.clay + b.clay, a.obsidian + b.obsidian, a.geode + b.geode);
            public static Resouces operator -(Resouces a, Resouces b) => new Resouces(a.ore - b.ore, a.clay - b.clay, a.obsidian - b.obsidian, a.geode + b.geode);
            public static bool operator <(Resouces a, Resouces b) => a.ore < b.ore && a.clay < b.clay && a.obsidian < b.obsidian && a.geode < b.geode;
            public static bool operator >(Resouces a, Resouces b) => a.ore > b.ore && a.clay > b.clay && a.obsidian > b.obsidian && a.geode < b.geode;

        }
        private class RoboFactory
        {
            public Resouces resouces;
            public Dictionary<ResouceType, Resouces> roboCosts;
            public Resouces robos = Resouces.Ore(1);
            int round = 0;

            public RoboFactory(string blueprint)
            {
                resouces = new Resouces();
                var roboList = blueprint.Split("Each");
                roboCosts = new Dictionary<ResouceType, Resouces>
                {
                    { ResouceType.Ore, ResourcesFromString(roboList[1]) },
                    { ResouceType.Clay, ResourcesFromString(roboList[2]) },
                    { ResouceType.Obsidian, ResourcesFromString(roboList[3]) },
                    { ResouceType.Geode, ResourcesFromString(roboList[4]) }
                };


            }
            private RoboFactory(RoboFactory old, ResouceType build, int rounds)
            {
                resouces = old.resouces + (old.robos * rounds) - old.roboCosts[build];

                robos = old.robos.Copy();
                robos.Add(build, 1);
                resouces = resouces + robos;

                roboCosts = old.roboCosts;
                round = old.round + rounds + 1;
            }

            public IEnumerable<RoboFactory> Neibours()
            {
                foreach(var kv in roboCosts.ToList())
                {
                    var roboType = kv.Key;
                    var roboCost = kv.Value;    
                    if (robos.CanBeReachedOrSurpassedByMuliplication(roboCost))
                    {
                        var rounds = 0;
                        while (!(roboCost < (resouces * rounds))) rounds++;
                        yield return new RoboFactory(this, roboType, rounds);
                    }
                }
            }

        }

        public static void solve()
        {
            InputProvider inputProvider = new InputProvider("day19");
            var RoboFactories = inputProvider.Get(InputType.Sample).Split("\r\n").Select(s => new RoboFactory(s)).ToArray();
            var roboCosts = RoboFactories.Select(f => DeapthFirstSearch.Best(
                f,
                f => f.resouces.geode,
                f => f.Neibours()
                )).ToArray();
            Console.WriteLine();
        }

        private static Resouces ResourcesFromString(string text)
        {
            var itemList = text.Split("costs")[1].Replace(".","").Split(" ").Where(s => s != "" && s != "and").ToList();
            var resouces = new Resouces();
            var oreIdx = itemList.IndexOf("ore");
            if (oreIdx != -1) resouces.ore = int.Parse(itemList[oreIdx-1]);

            var clayIdx = itemList.IndexOf("clay");
            if (clayIdx != -1) resouces.clay = int.Parse(itemList[clayIdx - 1]);

            var obsidianIdx = itemList.IndexOf("obsidian");
            if (obsidianIdx != -1) resouces.obsidian = int.Parse(itemList[obsidianIdx - 1]);
            return resouces;
        }
    }
}
