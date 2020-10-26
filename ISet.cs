using System;
using System.Collections.Generic;
using System.Text;

namespace SetTheory
{
    public interface ISet<T>
    {
        bool IsMember(T other);
        ISet<T> Union(ISet<T> other);
        ISet<T> Intersection(ISet<T> other);
        ISet<T> Difference(ISet<T> other);
        ISet<T> Complement();
        int CompareTo(ISet<T> other);
    }
}
