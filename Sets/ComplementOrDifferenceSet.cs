using System;
using SetTheory;

public class ComplementOrDifferenceSet : ISet<long>
{
	private RangeSet LowerValues;
    private HashedSet MiddleValues;
	private RangeSet UpperValues;


	public ComplementOrDifferenceSet(RangeSet lowerValues, HashedSet middleValues, RangeSet upperValues)
    {
        LowerValues = lowerValues;
        MiddleValues = middleValues;
        UpperValues = upperValues;
    }


    public bool IsMember(long other)
    {
        throw new NotImplementedException();
    }

    public ISet<long> Union(ISet<long> other)
    {
        throw new NotImplementedException();
    }

    public ISet<long> Intersection(ISet<long> other)
    {
        throw new NotImplementedException();
    }

    public ISet<long> Difference(ISet<long> other)
    {
        throw new NotImplementedException();
    }

    public ISet<long> Complement()
    {
        throw new NotImplementedException();
    }

    public int CompareTo(ISet<long> other)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object? obj)
    {
        if (obj is ComplementOrDifferenceSet cods)
        {
            return LowerValues.Equals(cods.LowerValues) && MiddleValues.Equals(cods.MiddleValues) &&
                   UpperValues.Equals(cods.UpperValues);
        }
        return base.Equals(obj);
    }

    public override string ToString()
    {
        var str1 = $"lower: min {LowerValues.Min} max: {LowerValues.Max}\n";
        var str2 = MiddleValues.ToString() + "\n";
        var str3 = $"lower: min {UpperValues.Min} max: {UpperValues.Max}\n";
        return str1 + str2 + str3;
    }
}
