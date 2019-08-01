using System;
using System.Collections.Generic;
using System.Linq;
// ReSharper disable PossibleMultipleEnumeration

namespace NPCGenerator.Util
{
    public static class Extensions
    {
        private static readonly Random rnd = new Random();
        public static T GetRandom<T>(this IEnumerable<T> lst)
        {
            return lst.ElementAt(rnd.Next(0, lst.Count()));
        }
    }
}