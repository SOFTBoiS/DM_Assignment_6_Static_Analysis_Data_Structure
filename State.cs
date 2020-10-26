using System;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Linq;

namespace DM_Assignment_6a_Static_Analysis_Data_Structure
{
    class State
    {
        IDictionary<string, SetTheory.ISet<int>> state;

        public State(IDictionary<string, SetTheory.ISet<int>> state)
        {
            this.state = state;
        }

        int CompareTo(IDictionary<string, SetTheory.ISet<int>> other)
        {
            //Get random kvpair from dictionary, to find an initial compareTo value
            var randomElement = this.state.ElementAt(0);

            var initCompareRes = randomElement.Value.CompareTo(other[randomElement.Key]);



            foreach(KeyValuePair<string, SetTheory.ISet<int>> variable in this.state)
            {
                var compareRes = variable.Value.CompareTo(other[variable.Key]);
                if (compareRes == 2) return 2;
                if (compareRes == -2) return -2;
                if(compareRes != initCompareRes) return -2;
            }

            return initCompareRes;
        }

        IDictionary<string, SetTheory.ISet<int>> Minimum()
        {
            throw new NotImplementedException();
        }

        IDictionary<string, SetTheory.ISet<int>> Max()
        {
            throw new NotImplementedException();
        }
    }
}
