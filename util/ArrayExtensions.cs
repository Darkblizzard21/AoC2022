using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.util
{
    public static class ArrayExtensions
    {
        public static string ToFormmatedString(this char[][] array) => array.Select(l => l.Aggregate("", (p, c) => p + c)).Aggregate((p, c) => p + "\n" + c);
    }
}
