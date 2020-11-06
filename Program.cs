using System;
using System.Collections.Generic;

namespace DM_Assignment_6a_Static_Analysis_Data_Structure
{
    class Program
    {
        static void Main(string[] args)
        {
            var stateDict1 = new Dictionary<string, SetTheory.ISet<long>>
            {
                ["a"] = new SetTheory.HashedSet(new long[] { 1 }),
                ["b"] = new SetTheory.HashedSet(new long[] { 1, 2, 3 }),
                ["c"] = new SetTheory.HashedSet(new long[] { 3 }),
            };

            var stateDict2 = new Dictionary<string, SetTheory.ISet<long>>
            {
                ["a"] = new SetTheory.HashedSet(new long[] { 1, 2, 3 }),
                ["b"] = new SetTheory.HashedSet(new long[] { 1, 2, 3 }),
                ["c"] = new SetTheory.HashedSet(new long[] { 1, 2, 3 }),
                ["d"] = new SetTheory.HashedSet(new long[] { 3 })
            };

            var preState = new State(stateDict1);
            var postState = new State(stateDict2);

            var res1 = preState.CompareTo(postState.state);
            Console.WriteLine($"res1 {res1}");

            var res2 = postState.CompareTo(preState.state);
            Console.WriteLine($"res2 {res2}");

        }
    }
}
