 using System;
using System.Collections.Generic;
using System.Text;

namespace SetTheory
{
    public class UnionSet : ISet<long>
    {
        private ISet<long> first;
        private ISet<long> second;

        public UnionSet(ISet<long> first, ISet<long> second)
        {
            this.first = first;
            this.second = second;
        }

        public int CompareTo(ISet<long> set)
        {
            var a = first.CompareTo(set);
            var b = second.CompareTo(set);
            Console.WriteLine("a: " + a);
            Console.WriteLine("b: " + b);
            // C is a pure subset to either a or b, therefore c is a pure subset to to the whole unionset
            if (a == 1 || b == 1) return 1;

            // A contains all the same values as c and b has additional values which means ab is pure superset to c
            if ((a == 0 && b == -2) ||
                (a == -2 && b == 0)) return -1;

            // Either a and b are both equal to c, or one is equal and the other is a pure subset, which means that the one actually contains the other and therefore equals c.
            if ((a == 0 && (b == 0 || b == -1)) ||
                (a == 0 || a == -1) && b == 0) return 0;

            // There are no subsets
            if (a == -2 && b == -2) return -2;

            // This can either return -1, 0, 1 or -2, but is indeterminable which one it is (unless further logic is implementet)
            return 2;
            // TODO: implement feature to compress sets so we can figure out if we can return -1
        }

        public ISet<long> Complement()
        {
            throw new NotImplementedException();
        }

        public ISet<long> Difference(ISet<long> set)
        {
            throw new NotImplementedException();
        }

        public ISet<long> Intersection(ISet<long> set)
        {
            throw new NotImplementedException();
        }

        public bool IsMember(long set)
        {
            throw new NotImplementedException();
        }

        public ISet<long> Union(ISet<long> other)
        {

            return new UnionSet(this, other);
        }

        public override bool Equals(object? obj)
        {
            if (obj is UnionSet us)
            {
                return first.Equals(us.first) && second.Equals(us.second);
            }
            return base.Equals(obj);
        }

        public override string ToString()
        {
            var str1 = "First: ";
            var str2 = "";
            if (first is RangeSet rs)
            {
                str1 += $"min: {rs.Min} max: {rs.Max}";
            }

            if (second is RangeSet rs2)
            {
                str2 += $"min: {rs2.Min} max: {rs2.Max}";

            }

            return str1 + "\n" + str2;
        }
    }

}
