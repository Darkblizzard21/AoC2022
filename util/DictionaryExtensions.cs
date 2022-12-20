using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022.util
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrThrow<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) where TKey : notnull
        {
            if(dict.TryGetValue(key, out TValue value)) return value;
            throw new ArgumentException();
        }

        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value) where TKey : notnull => dict[key] = value;
        
    }
}
