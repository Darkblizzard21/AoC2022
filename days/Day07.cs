using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.days
{
    public static class Day07
    {
        public static void solve()
        {
            InputProvider inputProvider = new InputProvider("day07");
            IEnumerable<string> enumerable = inputProvider.Get(Type.Input).Split("\r\n");
            IEnumerator<string> instructions = enumerable.GetEnumerator();

            // Build Folder Structure
            instructions.MoveNext();
            Folder root = new Folder("\\");
            Folder current = root;
            while (instructions.MoveNext())
            {
                var instruction = instructions.Current;
                var words = instruction.Split(" ");
                if (instruction[0] == '$' && words.Length == 3)
                {
                    if (words[2] == "..")
                    {
                        current = current.GetParent();
                    }
                    else
                    {
                        current = current.MakeChild(words[2]);
                    }
                }
                else if (Int32.TryParse(words[0], out int number))
                {
                    current.AddFile(number);
                }
            }
            // Calculate Part 1
            var smallerThan100000 = root.AllFolders().Select(f => f.getSize()).Where(s => s < 100000).Sum();
            Console.WriteLine(smallerThan100000);

            // Calculate Part 2
            var freeSpace = 70000000 - root.getSize();
            var requiedSpace = 30000000 - freeSpace;
            var smallestBigerThan30000000 = root.AllFolders().Select(f => f.getSize()).Where(s => s >= requiedSpace).Min();
            Console.WriteLine(smallestBigerThan30000000);

        }
    }

    public class Folder
    {
        private List<Folder> _children = new List<Folder>();
        private int fileSize = 0;
        private string name;
        private Folder? parent = null;

        public Folder(string name)
        {
            this.name = name;
        }

        public int getSize()
        {
            return fileSize + _children.Aggregate(0, (a, b) => a + b.getSize());
        }

        public void AddFile(int Size)
        {
            fileSize += Size;
        }

        public Folder MakeChild(string name)
        {
            Folder child = new Folder(name);
            child.parent = this;
            _children.Add(child);
            return child;
        }

        public IEnumerable<Folder> AllFolders()
        {
            return _children.SelectMany(c => c.AllFolders()).Append(this);
        }

        public Folder GetParent()
        {
            if (parent == null) throw new InvalidOperationException();
            return parent;
        }
    }
}

    
