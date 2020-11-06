using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace SetTheory
{
    public class RangeSet : ISet<long>
    {
        public readonly long Min;
        public readonly long Max;
        public long Length => CalculateLength();



        private long CalculateLength()
        {
            var _min = new BigInteger(Min);
            var _max = new BigInteger(Max);
            var realLength = _max - _min + 1;
            return realLength > long.MaxValue ? long.MaxValue : Max - Min + 1;

        }

        public RangeSet(long min, long max)
        {
            Min = min;
            Max = max;
        }

        public bool IsMember(long member)
        {
            return Min <= member && member <= Max;
        }

        public ISet<long> Union(ISet<long> other)
        {
            if (other is RangeSet rs)
            {
                if (Min > rs.Max || rs.Min > Max)
                {
                    return new UnionSet(this, other);
                }

                var minVal = Math.Min(this.Min, rs.Min);
                var maxVal = Math.Max(this.Max, rs.Max);
                return new RangeSet(minVal, maxVal);
            }

            // TODO: Do the same for HashedSet
            return new UnionSet(this, other);
        }

        public ISet<long> Intersection(ISet<long> other)
        {
            if (other is RangeSet rs)
            {
                // TODO: Implement empty set and return that instead
                if (rs.Min > Max || Min > rs.Max) return new HashedSet(new long[] { });

                var lowerBounds = Math.Max(this.Min, rs.Min);
                var upperBounds = Math.Min(this.Max, rs.Max);

                return new RangeSet(lowerBounds, upperBounds);
            }

            if (other is HashedSet hs)
            {
                var first = hs.Values[0];
                var last = hs.Values[^1];

                // If same interval
                if (Length == hs.Length && Min == first && Max == last) return this;

                // If no intersection
                if (Min > last || Max < first)
                    return new HashedSet(new long[] { }); // TODO: Implement empty set and return that instead


                List<long> intersection = new List<long>();

                // If HashedSet is smaller
                if (hs.Length < Length)
                {
                    if (hs.Length >= Rules.MaxValueToLoop) throw new Exception("Set size too large");

                    foreach (var x in hs.Values)
                    {
                        if (IsMember(x))
                        {
                            intersection.Add(x);
                        }
                    }
                    return new HashedSet(intersection.ToArray());
                }

                // Else if RangeSet is smaller
                if (Length >= Rules.MaxValueToLoop) throw new Exception("Set size too large");
                var currentMinValue = Min;
                var currentMaxValue = Max;

                // These while loops finds upper bounds and lower bounds of hs
                while (hs.IndexOf(currentMinValue) == -1)
                {
                    currentMinValue++;
                }

                while (hs.IndexOf(currentMaxValue) == -1)
                {
                    currentMaxValue--;
                }

                var minCommonMember = hs.IndexOf(currentMinValue);
                var maxCommonMember = hs.IndexOf(currentMaxValue);

                for (var i = minCommonMember; i <= maxCommonMember; i++)
                {
                    intersection.Add(hs.Values[i]);
                }

                return new HashedSet(intersection.ToArray());
            }

            // If Union

            throw new NotImplementedException();
        }

        public ISet<long> Difference(ISet<long> other)
        {
            if (other is RangeSet rs)
            {
                // Compare the lower- and upper bounds of the two Range Sets
                var otherMinBiggerThanMin = rs.Min > Min;
                var otherMaxSmallerThanMax = rs.Max < Max;
                // TODO: Implement empty set and return that instead
                // If this is a pure subset of 'rs' / 'other'
                if (CompareTo(rs) == -1) return new HashedSet(new long[] { });

                if (otherMinBiggerThanMin)
                {
                    if (otherMaxSmallerThanMax)
                    {
                        // Other is subset of This. Return Union of the lower and upper ranges
                        var rs1 = new RangeSet(Min, rs.Min-1);
                        var rs2 = new RangeSet(rs.Max+1, Max);
                        return rs1.Union(rs2);
                    }
                    // Cut off top of This range
                    return new RangeSet(Min, rs.Min-1);
                }
                // Cut off bottom of This range
                return new RangeSet(rs.Max+1, Max);
            }
            if (other is HashedSet hs)
            {
                long commonMinIndex = 0;
                long commonMaxIndex = hs.Length-1;
                long commonMin = hs.Values[commonMinIndex];
                long commonMax = hs.Values[commonMaxIndex];
                
                while (!IsMember(commonMin))
                {
                    if (commonMinIndex == hs.Length)
                    {
                        return this;
                    }
                    commonMin = hs.Values[++commonMinIndex];
                }
                while (!IsMember(commonMax))
                {
                    if (commonMaxIndex == commonMinIndex)
                    {
                        break;
                    }
                    commonMax = hs.Values[--commonMinIndex];
                }
                var lowerValues = new RangeSet(long.MinValue, commonMin - 1);
                var upperValues = new RangeSet(commonMin + 1, long.MaxValue);
                var middleValuesTmp = new List<long>();
                var middleValues = new HashedSet(new long[] { });
                if (commonMin == commonMax)
                {
                    middleValues = new HashedSet(new long[] { });
                }
                else
                {
                    commonMinIndex++;
                    for (long i = commonMin + 1; i < commonMax; i++)
                    {
                        if (i == hs.Values[commonMinIndex])
                        {
                            commonMinIndex++;
                        }
                        else
                        {
                            middleValuesTmp.Add(i);
                        }
                    }
                    middleValues = new HashedSet(middleValuesTmp.ToArray());
                }
                
                return new ComplementOrDifferenceSet(lowerValues, middleValues, upperValues);
            }

            // if (other is UnionSet us)
            throw new NotImplementedException();
        }

        public ISet<long> Complement()
        {
            // If the RangedSet contains all long values, there is no complement
            if (Min == long.MinValue)
            {
                if (Max == long.MaxValue)
                    return new HashedSet(new long[] { }); // TODO: Implement empty set and return that instead
                // If the Min value is the smallest long value and the Max value is NOT the maximum long value
                return new RangeSet(Max + 1, long.MaxValue);
            }

            if (Max == long.MaxValue)
                return new RangeSet(long.MinValue, Max - 1);

            var lower = new RangeSet(long.MinValue, Min - 1);
            var upper = new RangeSet(Min + 1, long.MaxValue);
            return lower.Union(upper);
        }

        public int CompareTo(ISet<long> other)
        {
            if (other is RangeSet rs)
            {
                if (Max == rs.Max && Min == rs.Min) return 0;
                if (Min >= rs.Min && Max <= rs.Max) return -1;
                if (Min <= rs.Min && Max >= rs.Max) return 1;
                return -2;
            }

            if (other is HashedSet hs)
            {
                var first = hs.Values[0];
                var last = hs.Values[^1];

                // Equals
                if (Length == hs.Length && Min == first && Max == last) return 0;

                if (Length < hs.Length)
                {
                    // Because we loop through the shortest set, we won't loop if the set contains too many numbers
                    if (Length >= Rules.MaxValueToLoop) return 2;

                    // Find the smallest common value in the Hashed Set
                    var minCommonMember = hs.IndexOf(Min);

                    // If smallest common value is not found
                    if (minCommonMember == -1)
                    {
                        return -2;
                    }

                    // The current value in the RangedSet which we are using in the loop below.
                    var currentValue = Min;

                    // If we have a common value in the RangedSet and HashedSet
                    // We iterate through the HashedSet to try and figure out if the RangedSet
                    // Is a pure subset of the HashedSet
                    for (var i = minCommonMember; i <= Max; i++, currentValue++)
                    {
                        //Check for indexOutOfRange and if values is not member of HashedSet
                        if (i >= hs.Values.Length || hs.Values[i] != currentValue)
                        {
                            return -2;
                        }
                    }
                    // RangeSet is pure subset of HashedSet
                    return -1;
                }

                // Else if HashSet length is smaller
                if (hs.Length <= Rules.MaxValueToLoop) return 2;

                foreach (var x in hs.Values)
                {
                    if (!IsMember(x))
                    {
                        return -2;
                    }
                }
                // RangeSet is pure superset of HashedSet
                return 1;


            }

            // It must be a UnionSet

            var res = other.CompareTo(this);
            if (res == 2 || res == -2) return res;
            // Because we use the "other" set's compareTo method,
            // We have to reverse the result value.
            return res * -1;

        }

        public override bool Equals(object? obj)
        {
            if (obj is RangeSet rs)
            {
                return Min == rs.Min && Max == rs.Max;
            }
            return base.Equals(obj);
        }

 
    }
}
