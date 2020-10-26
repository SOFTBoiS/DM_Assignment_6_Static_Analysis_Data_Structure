using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace SetTheory
{
    public class HashedSet : ISet<long>
    {
        public long[] Values { get; private set; }
        public long Length => Values.Length;
        public HashedSet(long[] set)
        {
            Values = set.Distinct().ToArray();
            Array.Sort(Values);
        }

        public int CompareTo(ISet<long> other)
        {
            if (other is HashedSet hs)
            {
                // This HashedSet is shorter than the other
                int commonMinValueIndex;
                if (Length < hs.Length)
                {
                    if (Length > Rules.MaxValueToLoop) return 2;
                    commonMinValueIndex = hs.IndexOf(Values[0]);
                    if (commonMinValueIndex == -1) return -2;
                    for (long i = 0, j = commonMinValueIndex; i < Length && j < hs.Length;)
                    {
                        if (Values[i] != hs.Values[j])
                        {
                            if (hs.Values[j] > Values[i]) return -2;
                        }
                        else 
                        {
                            i++;
                        }

                        j++;
                    }
                    return -1;
                }

                // Other is longer than this HashedSet
                if (hs.Length > Rules.MaxValueToLoop) return 2;
                commonMinValueIndex = IndexOf(hs.Values[0]);
                if (commonMinValueIndex == -1) return -2;
                for (long i = 0, j = commonMinValueIndex; i < hs.Length && j < Length;)
                {
                    if (hs.Values[i] != Values[j])
                    {
                        if (Values[j] > hs.Values[i]) return -2;
                    }
                    else
                    {
                        i++;
                    }

                    j++;
                }

                return Length == hs.Length ? 0 : 1;
            }

            if (other is RangeSet rs)
            {
                var first = Values[0];
                var last = Values[^1];
                // Equals
                if (Length == rs.Length && rs.Min == first && rs.Max == last) return 0;
                // If HashedSet length is smaller
                if (Length < rs.Length)
                {
                    if (Length >= Rules.MaxValueToLoop) return 2;

                    foreach (var x in Values)
                    {
                        if (!rs.IsMember(x))
                        {
                            return -2;
                        }
                    }
                    // RangeSet is pure superset of HashedSet
                    return -1;
                }

                // Else if RangeSet length is smaller

                // Because we loop through the shortest set, we won't loop if the set 
                if (rs.Length >= Rules.MaxValueToLoop) return 2;

                // Find the smallest common value in the Hashed Set
                var minCommonMemberIndex = IndexOf(rs.Min);

                // If smallest common value is not found
                if (minCommonMemberIndex == -1)
                {
                    return -2;
                }

                // The current value in the RangedSet which we are using in the loop below.
                var currentValue = rs.Min;

                // If we have a common value in the RangedSet and HashedSet
                // We iterate through the HashedSet to try and figure out if the RangedSet
                // Is a pure subset of the HashedSet
                for (var i = minCommonMemberIndex; i <= rs.Max; i++, currentValue++)
                {
                    //Check for indexOutOfRange and if values is not member of HashedSet
                    if (i >= Length || Values[i] != currentValue)
                    {
                        return -2;
                    }
                }
                // RangeSet is pure subset of HashedSet
                return 1;
            }

            if (other is UnionSet us)
            {
                var res = us.CompareTo(this);
                if (res == 2 || res == -2) return res;
                // Because we use the "other" set's compareTo method,
                // We have to reverse the result value.
                return res * -1;
            }

            throw new NotImplementedException();
        }

        public ISet<long> Complement()
        {
            var first = Values[0];
            var last = Values[^1];

            if (Length > Rules.MaxValueToLoop)
                throw new Exception("Set size too large to calculate");


            //Separates the complementset into 2 rangesets for upper and Lower parts + middle hashedSet
            var lowerRange = new RangeSet(long.MinValue, first);
            var upperRange = new RangeSet(last, long.MaxValue);
            HashedSet middleRange;

            var nextValueIdx = 0;
            var complements = new List<long>();

            // This adds only values not in the original HashedSet
            for (long i = first; i < last; i++)
            {
                var nextValue = Values[nextValueIdx];
                if (i == nextValue)
                    nextValue = Values[nextValueIdx++];
                if (i != nextValue)
                    complements.Add(i);
            }

            middleRange = new HashedSet(complements.ToArray());

            return new ComplementOrDifferenceSet(lowerRange, middleRange, upperRange);
        }

        public ISet<long> Difference(ISet<long> other)
        {
            if (other is HashedSet hs)
            {
                if (Length >= Rules.MaxValueToLoop || hs.Length >= Rules.MaxValueToLoop)
                    throw new Exception("Set size too large to calculate");

                var difference = new List<long>();
                var iterations = Length < hs.Length ? Length : hs.Length;
                // Since the length is a long, we have to count the integer ourselves.
                // Because the MaxValueToLoop value is smaller than the max integer value, this is safe
                var hsLengthInInt = 0;
                // Iterate through the shortest Set and check for values that are not in the 'hs' HashedSet
                for (var i = 0; i < iterations; i++, hsLengthInInt++)
                {
                    if (!hs.IsMember(Values[i]))
                    {
                        difference.Add(Values[i]);
                    }
                }

                // If this HashedSet is longer than the other, we need to add the remaining values after the loop has finished
                if (Length > hs.Length)
                {
                    // Get the rest of the set values from the set and combine them with the set values anot found in the other set
                    var remaining = Values[hsLengthInInt..^1];

                    // Convert the list of difference values to array so we can merge
                    var setArrayValues = difference.ToArray();

                    // Resize the array to fit the remaining values
                    var newLength = setArrayValues.Length + remaining.Length;
                    Array.Resize(ref setArrayValues, newLength);

                    // Copy the elements of the remaining values into the setArrayValues array
                    Array.Copy(remaining, setArrayValues, newLength);
                    return new HashedSet(setArrayValues);
                }

                return new HashedSet(difference.ToArray());
            }

            if (other is RangeSet rs)
            {

            }
            throw new NotImplementedException();
        }

        public ISet<long> Intersection(ISet<long> other)
        {
            if (other is HashedSet hs)
            {
                var intersectionSet = new List<long>();
                if (Length < hs.Length)
                {
                    for (var i = 0; i < Length; i++)
                    {
                        if (hs.IsMember(Values[i]))
                        {
                            intersectionSet.Add(Values[i]);
                        }
                    }
                }
                else // if the other set is shorter
                {
                    for (var i = 0; i < hs.Length; i++)
                    {
                        if (IsMember(hs.Values[i]))
                        {
                            intersectionSet.Add(Values[i]);
                        }
                    }
                }

                return new HashedSet(intersectionSet.ToArray());
            }

            if (other is RangeSet rs)
            {
                var first = Values[0];
                var last = Values[^1];


                // If same interval
                if (Length == rs.Length && rs.Min == first && rs.Max == last) return rs;

                // If no intersection
                if (rs.Min > last || rs.Max < first)
                    return new HashedSet(new long[] { }); // TODO: Implement empty set and return that instead


                List<long> intersection = new List<long>();

                // If HashedSet is smaller
                if (Length < rs.Length)
                {
                    if (rs.Length >= Rules.MaxValueToLoop) throw new Exception("Set size too large");

                    foreach (var x in Values)
                    {
                        if (rs.IsMember(x))
                        {
                            intersection.Add(x);
                        }
                    }
                    return new HashedSet(intersection.ToArray());
                }

                // Else if RangeSet is smaller
                if (rs.Length >= Rules.MaxValueToLoop) throw new Exception("Set size too large");
                var currentMinValue = rs.Min;
                var currentMaxValue = rs.Max;

                // These while loops finds upper bounds and lower bounds of hs
                while (IndexOf(currentMinValue) == -1)
                {
                    currentMinValue++;
                }

                while (IndexOf(currentMaxValue) == -1)
                {
                    currentMaxValue--;
                }

                var minCommonMember = IndexOf(currentMinValue);
                var maxCommonMember = IndexOf(currentMaxValue);

                for (var i = minCommonMember; i < maxCommonMember; i++)
                {
                    intersection.Add(Values[i]);
                }

                return new HashedSet(intersection.ToArray());
            }
            throw new NotImplementedException();
        }

        public bool IsMember(long value)
        {
            return IndexOf(value) != -1;
        }

        public ISet<long> Union(ISet<long> other)
        {
            return new UnionSet(this, other);
        }

        public int IndexOf(long value)
        {
            return Array.IndexOf(Values, value);
        }

        public override bool Equals(object? obj)
        {
            if (obj is HashedSet hs)
            {
                if (Length != hs.Length) return false;

                for (var i = 0; i < Length; i++)
                {
                    if (Values[i] != hs.Values[i]) return false;
                }

                return true;
            }

            return base.Equals(obj);
        }

        public override string ToString()
        {
            var str = "{ ";
            foreach (var x in Values)
            {
                str += $"{x},  ";
            }

            return str + "}";
        }
    }
}
