using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utility
{
    public static int HashInt(int raw)
    {
        unchecked
        {
            raw += (raw << 12);
            raw ^= (raw >> 22);
            raw += (raw << 4);
            raw ^= (raw >> 9);
            raw += (raw << 10);
            raw ^= (raw >> 2);
            raw += (raw << 7);
            raw ^= (raw >> 12);
            return raw;
        }
    }


    public static int CombineHash(int seed, int value)
    {
        // A copy of the C++ boost library hash_combine function
        // https://stackoverflow.com/questions/2590677/how-do-i-combine-hash-values-in-c0x
        unchecked
        {
            seed ^= value + ((int)0x9e3779b9) + (seed << 6) + (seed >> 2);
            return seed;
        }
    }

    public static string ListToString<T>(this IEnumerable<T> list)
    {
        return "[" + string.Join(", ", list.Select(obj => obj.ToString()).ToArray()) + "]";
    }
}
